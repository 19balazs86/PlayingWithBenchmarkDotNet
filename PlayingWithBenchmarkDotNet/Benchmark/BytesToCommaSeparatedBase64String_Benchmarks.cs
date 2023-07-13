using BenchmarkDotNet.Attributes;
using System.Buffers;
using System.Buffers.Text;

namespace PlayingWithBenchmarkDotNet.Benchmark;

/*
|                  Method |     Mean |   Error |  StdDev |   Gen0 | Allocated |
|------------------------ |---------:|--------:|--------:|-------:|----------:|
|  Convert_ToBase64String | 213.3 ns | 3.60 ns | 3.36 ns | 0.0937 |     392 B |
| Using_ArrayBufferWriter | 192.2 ns | 0.47 ns | 0.40 ns | 0.1798 |     752 B |
|     Using_String_Create | 162.1 ns | 0.89 ns | 0.83 ns | 0.0515 |     216 B |
*/

[MemoryDiagnoser]
public class BytesToCommaSeparatedBase64String_Benchmarks
{
    private IReadOnlyList<Message> _messages;

    [GlobalSetup]
    public void Setup()
    {
        var bytes = new[]
        {
            "Optimize"u8.ToArray(),
            "the following"u8.ToArray(),
            "Base64String process"u8.ToArray()
        };

        _messages = bytes.Select(Message.Create).ToArray();
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
        int length = _messages.Count - 1;

        foreach (Message message in _messages)
        {
            length += Base64.GetMaxEncodedToUtf8Length(message.Payload.Length);
        }

        return string.Create(length, _messages, static (span, messages) =>
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
        });
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