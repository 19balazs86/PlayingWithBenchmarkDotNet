using System.Buffers.Text;
using System.Text;

namespace PlayingWithBenchmarkDotNet.Code;

// TODO: Needs to work on it, something is odd...
public static class Base64UrlEncoder
{
    private const char _charEquals     = '=';
    private const char _charSlash      = '/';
    private const char _charHyphen     = '-';
    private const char _charPlus       = '+';
    private const char _charUnderscore = '_';
    private const byte _byteEquals     = (byte)'=';
    private const byte _byteSlash      = (byte)'/';
    private const byte _bytePlus       = (byte)'+';

    public static string Encode(ReadOnlySpan<char> input)
    {
        Span<byte> inputAsUTF8Bytes = stackalloc byte[Encoding.UTF8.GetByteCount(input)];

        Encoding.UTF8.GetBytes(input, inputAsUTF8Bytes);

        int base64EncodedLength = Base64.GetMaxEncodedToUtf8Length(inputAsUTF8Bytes.Length);

        Span<byte> base64Bytes = stackalloc byte[base64EncodedLength];

        Base64.EncodeToUtf8(inputAsUTF8Bytes, base64Bytes, out _, out _);

        int paddingSize = base64Bytes switch
        {
            [.., _byteEquals, _byteEquals] => 2,
            [.., _byteEquals]              => 1,
            _                              => 0
        };

        Span<char> finalChars = stackalloc char[base64EncodedLength - paddingSize];

        for (int i = 0; i < finalChars.Length; i++)
        {
            finalChars[i] = base64Bytes[i] switch
            {
                _byteSlash => _charHyphen,
                _bytePlus  => _charUnderscore,
                _          => (char)base64Bytes[i]
            };
        }

        return new string(finalChars);
    }

    public static string Decode(ReadOnlySpan<char> base64Input)
    {
        int paddingSize = (base64Input.Length % 4) switch
        {
            0     => 0,
            int n => 4 - n
        };

        Span<byte> base64bytes = stackalloc byte[base64Input.Length + paddingSize];

        for (int i = 0; i < base64Input.Length; i++)
        {
            base64bytes[i] = base64Input[i] switch
            {
                _charHyphen     => _byteSlash,
                _charUnderscore => _bytePlus,
                _               => (byte)base64Input[i]
            };
        }

        for (int i = paddingSize; i > 0; i--)
            base64bytes[^i] = _byteEquals;

        Span<byte> utf8Bytes = stackalloc byte[Base64.GetMaxDecodedFromUtf8Length(base64bytes.Length)];

        Base64.DecodeFromUtf8(base64bytes, utf8Bytes, out int consumed, out int bytesWritten);

        Span<char> finalChars = stackalloc char[bytesWritten];

        Encoding.UTF8.GetChars(utf8Bytes[..bytesWritten], finalChars);

        return finalChars.ToString();
    }
}
