using System.Buffers.Text;
using System.Runtime.InteropServices;

namespace PlayingWithBenchmarkDotNet.Code;

// YouTube - Nick: https://youtu.be/B2yOjLyEZk0
// https://www.stevejgordon.co.uk/using-high-performance-dotnetcore-csharp-techniques-to-base64-encode-a-guid
public static class Guider
{
    private const char _charEquals     = '=';
    private const char _charSlash      = '/';
    private const char _charHyphen     = '-';
    private const char _charPlus       = '+';
    private const char _charUnderscore = '_';
    private const byte _byteEquals     = (byte)'=';
    private const byte _byteSlash      = (byte)'/';
    private const byte _bytePlus       = (byte)'+';

    public static string ToStringFromGuid(Guid id)
    {
        return Convert.ToBase64String(id.ToByteArray())
            .Replace("/", "-")
            .Replace("+", "_")
            .Replace("=", string.Empty);
    }

    public static Guid ToGuidFromString(string id)
    {
        id = id.Replace("-", "/").Replace("_", "+") + "==";

        byte[] base64StrArray = Convert.FromBase64String(id);

        return new Guid(base64StrArray);
    }

    public static string ToStringFromGuidOp(Guid id)
    {
        Span<byte> idBytes     = stackalloc byte[16];
        Span<byte> base64Bytes = stackalloc byte[24]; // With padding -> 4 * Math.Ceiling(X / 3.0) where X = 16

        MemoryMarshal.TryWrite(idBytes, in id);

        Base64.EncodeToUtf8(idBytes, base64Bytes, out _, out _);

        Span<char> finalChars = stackalloc char[22]; // Without padding -> Math.Ceiling(X * 8 / 6.0) where X = 16

        for (int i = 0; i < 22; i++)
        {
            finalChars[i] = base64Bytes[i] switch
            {
                _byteSlash => _charHyphen,
                _bytePlus => _charUnderscore,
                _ => (char)base64Bytes[i]
            };
        }

        return new string(finalChars);
    }

    public static Guid ToGuidFromStringOp(ReadOnlySpan<char> id)
    {
        Span<char> base64Chars = stackalloc char[24];

        for (int i = 0; i < 22; i++)
        {
            base64Chars[i] = id[i] switch
            {
                _charHyphen     => _charSlash,
                _charUnderscore => _charPlus,
                _               => id[i]
            };
        }

        base64Chars[22] = _charEquals;
        base64Chars[23] = _charEquals;

        // (3 * (length_in_chars / 4)) - number_of_padding_chars
        Span<byte> idBytes = stackalloc byte[16];

        Convert.TryFromBase64Chars(base64Chars, idBytes, out _);

        return new Guid(idBytes);
    }
}
