using BenchmarkDotNet.Attributes;
using System.Buffers;
using System.Buffers.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PlayingWithBenchmarkDotNet.Benchmark;

/*
|                  Method |     Mean | Allocated |
|------------------------ |---------:|----------:|
|  Convert_ToBase64String | 213.3 ns |     392 B |
| Using_ArrayBufferWriter | 192.2 ns |     752 B |
|     Using_String_Create | 162.1 ns |     216 B |

Extra example
|                            Method |     Mean | Allocated |
|---------------------------------- |---------:|----------:|
| Serialize_JSON_With_String_Create | 174.5 ns |     216 B |
|     Serialize_JSON_With_Converter | 332.7 ns |     176 B |
*/

[ShortRunJob]
// [RankColumn]
// [Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser]
[HideColumns("Error", "StdDev", "Gen0", "RatioSD")]
public class BytesToCommaSeparatedBase64String_Benchmarks
{
    private IReadOnlyList<Message> _messages;
    private Request _request;
    private JsonSerializerOptions _jsonSerializerOptions;

    [GlobalSetup]
    public void Setup()
    {
        byte[][] bytes =
        [
            "Optimize"u8.ToArray(),
            "the following"u8.ToArray(),
            "Base64String process"u8.ToArray()
        ];

        Message[] messagesArray = bytes.Select(Message.Create).ToArray();

        _messages = messagesArray;

        _request = new Request { Messages = messagesArray };

        _jsonSerializerOptions = new JsonSerializerOptions();
        _jsonSerializerOptions.Converters.Add(new MessagesJsonConverter());
    }

    [Benchmark]
    public string Convert_ToBase64String()
    {
        return string.Join(",", _messages.Select(m => Convert.ToBase64String(m.Payload)));
    }

    [Benchmark]
    public string Using_ArrayBufferWriter()
    {
        var writer = new ArrayBufferWriter<char>();

        foreach (Message message in _messages)
        {
            bool needComma = writer.WrittenCount > 0;

            // Get the predicted length of base64 output
            int length = Base64.GetMaxEncodedToUtf8Length(message.Payload.Length);

            if (needComma)
            {
                length++;
            }

            Span<char> payloadBase64Chars = writer.GetSpan(length);

            if (needComma)
            {
                payloadBase64Chars[0] = ',';

                payloadBase64Chars = payloadBase64Chars[1..]; // .Slice(1)
            }

            // Base64 encode directly into the buffer
            Convert.TryToBase64Chars(message.Payload, payloadBase64Chars, out int charsWritten);

            // Tell the writer how much we have written
            writer.Advance(charsWritten + (needComma ? 1 : 0));
        }

        return new string(writer.WrittenSpan);
    }

    [Benchmark]
    public string Using_String_Create()
    {
        int length = _messages.Count - 1; // Commas

        foreach (Message message in _messages)
        {
            length += Base64.GetMaxEncodedToUtf8Length(message.Payload.Length);
        }

        return string.Create(length, _messages, stringCreateSpanAction);

        static void stringCreateSpanAction(Span<char> span, IEnumerable<Message> messages)
        {
            int offset = 0;

            foreach (Message message in messages)
            {
                if (offset > 0)
                {
                    span[offset++] = ',';
                }

                Convert.TryToBase64Chars(message.Payload, span[offset..], out int charsWritten);

                offset += charsWritten;
            }
        }
    }

    // [Benchmark] // This is just an extra example
    public string Serialize_JSON_With_String_Create()
    {
        const string prefix = "{\"messages\": \"";
        const string suffix = "\"}";

        if (_messages.Count == 0)
        {
            return prefix + suffix;
        }

        int length = prefix.Length + suffix.Length +
            _messages.Count - 1; // Commas

        foreach (Message message in _messages)
        {
            length += Base64.GetMaxEncodedToUtf8Length(message.Payload.Length);
        }

        return string.Create(length, _messages, stringCreateSpanAction);

        static void stringCreateSpanAction(Span<char> span, IReadOnlyList<Message> messages)
        {
            prefix.CopyTo(span);

            int offset = prefix.Length;

            for (int i = 0; i < messages.Count; i++)
            {
                if (i > 0)
                {
                    span[offset++] = ',';
                }

                Convert.TryToBase64Chars(messages[i].Payload, span[offset..], out int charsWritten);

                offset += charsWritten;
            }

            suffix.CopyTo(span[offset..]);
        }
    }

    // [Benchmark] // This is just an extra example
    public string Serialize_JSON_With_Converter()
    {
        return JsonSerializer.Serialize(_request, _jsonSerializerOptions);
    }
}

public sealed class Message
{
    public byte[] Payload { get; init; }

    public static Message Create(byte[] payload)
    {
        return new Message
        {
            Payload = payload
        };
    }
}

public sealed class Request
{
    [JsonPropertyName("messages")]
    public Message[] Messages { get; set; }
}

public sealed class MessagesJsonConverter : JsonConverter<Message[]>
{
    public override Message[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, Message[] messages, JsonSerializerOptions options)
    {
        if (messages.Length == 0)
        {
            writer.WriteStringValue("");

            return;
        }

        int length = messages.Length - 1; // Commas

        foreach (Message message in messages)
        {
            length += Base64.GetMaxEncodedToUtf8Length(message.Payload.Length);
        }

        var arrayPool = ArrayPool<byte>.Shared;

        byte[] buffer   = arrayPool.Rent(length);
        Span<byte> span = buffer.AsSpan();

        const byte commyByte = (byte)',';

        for (int i = 0; i < messages.Length; i++)
        {
            if (i > 0)
            {
                span[0] = commyByte;
                span = span[1..];
            }

            OperationStatus status = Base64.EncodeToUtf8(messages[i].Payload, span, out _, out int bytesWritten);

            if (status != OperationStatus.Done)
            {
                throw new InvalidOperationException("Wrong status");
            }

            span = span[bytesWritten..];
        }

        writer.WriteStringValue(buffer.AsSpan(0, length));

        arrayPool.Return(buffer);
    }
}
