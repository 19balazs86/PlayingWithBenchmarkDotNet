using System.Buffers.Text;
using System.Text;

namespace PlayingWithBenchmarkDotNet.Code;

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

    public static string Encode_Simplest(string input)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
    }

    public static string Encode_Simplest_New(string input)
    {
        return Base64Url.EncodeToString(Encoding.UTF8.GetBytes(input));
    }

    public static string Encode_Simple(ReadOnlySpan<char> input)
    {
        Span<byte> inputAsUTF8Bytes = stackalloc byte[Encoding.UTF8.GetByteCount(input)];

        Encoding.UTF8.GetBytes(input, inputAsUTF8Bytes);

        return Convert.ToBase64String(inputAsUTF8Bytes);
    }

    public static string Encode_Complex(ReadOnlySpan<char> input)
    {
        Span<byte> inputAsUTF8Bytes = stackalloc byte[Encoding.UTF8.GetByteCount(input)];

        Encoding.UTF8.GetBytes(input, inputAsUTF8Bytes);

        int base64EncodedLength = Base64.GetMaxEncodedToUtf8Length(inputAsUTF8Bytes.Length);

        Span<byte> base64Bytes = stackalloc byte[base64EncodedLength];

        Base64.EncodeToUtf8(inputAsUTF8Bytes, base64Bytes, out _, out _);

        Span<char> finalChars = stackalloc char[base64Bytes.Length];

        for (int i = 0; i < base64Bytes.Length; i++)
            finalChars[i] = (char)base64Bytes[i];

        return new string(finalChars);
    }

    public static string Decode_Simplest(string input)
    {
        return Encoding.UTF8.GetString(Convert.FromBase64String(input));
    }

    public static string Decode_Simple(ReadOnlySpan<char> input)
    {
        Span<byte> base64Bytes = stackalloc byte[(input.Length * 3 + 3) / 4];

        Convert.TryFromBase64Chars(input, base64Bytes, out int bytesWritten);

        Span<char> finalChars = stackalloc char[bytesWritten];

        Encoding.UTF8.GetChars(base64Bytes[..bytesWritten], finalChars);

        return finalChars.TrimEnd('\0').ToString();
    }

    public static string Decode_Complex(ReadOnlySpan<char> base64Input)
    {
        Span<byte> base64bytes = stackalloc byte[base64Input.Length];

        for (int i = 0; i < base64Input.Length; i++)
            base64bytes[i] = (byte)base64Input[i];

        Span<byte> utf8Bytes = stackalloc byte[Base64.GetMaxDecodedFromUtf8Length(base64bytes.Length)];

        Base64.DecodeFromUtf8(base64bytes, utf8Bytes, out _, out int bytesWritten);

        Span<char> finalChars = stackalloc char[bytesWritten];

        Encoding.UTF8.GetChars(utf8Bytes[..bytesWritten], finalChars);

        return finalChars.TrimEnd('\0').ToString();
    }

    public static string UrlFriendlyEncode_Simplest(string input)
    {
        ReadOnlySpan<char> base64Chars = Convert.ToBase64String(Encoding.UTF8.GetBytes(input)).AsSpan();

        Span<char> finalChars = stackalloc char[base64Chars.TrimEnd('=').Length];

        for (int i = 0; i < finalChars.Length; i++)
        {
            finalChars[i] = base64Chars[i] switch
            {
                _charSlash => _charHyphen,
                _charPlus  => _charUnderscore,
                _          => base64Chars[i]
            };
        }

        return new string(finalChars);
    }

    public static string UrlFriendlyEncode_Simple(ReadOnlySpan<char> input)
    {
        Span<byte> utf8Bytes = stackalloc byte[Encoding.UTF8.GetByteCount(input)];

        Encoding.UTF8.GetBytes(input, utf8Bytes);

        Span<char> finalChars = stackalloc char[(utf8Bytes.Length + 2) / 3 * 4];

        Convert.TryToBase64Chars(utf8Bytes, finalChars, out int charsWritten);

        int paddingSize = finalChars[..charsWritten] switch // Check the last 2 characters
        {
            [.., _charEquals, _charEquals] => 2,
            [.., _charEquals]              => 1,
            _                              => 0
        };

        int finalLengthWithoutPadding = charsWritten - paddingSize;

        for (int i = 0; i < finalLengthWithoutPadding; i++)
        {
            finalChars[i] = finalChars[i] switch
            {
                _charSlash => _charHyphen,
                _charPlus  => _charUnderscore,
                _          => finalChars[i]
            };
        }

        return new string(finalChars[..finalLengthWithoutPadding]);
    }

    public static string UrlFriendlyEncode_Complex(ReadOnlySpan<char> input)
    {
        Span<byte> inputAsUTF8Bytes = stackalloc byte[Encoding.UTF8.GetByteCount(input)];

        Encoding.UTF8.GetBytes(input, inputAsUTF8Bytes);

        int base64EncodedLength = Base64.GetMaxEncodedToUtf8Length(inputAsUTF8Bytes.Length);

        Span<byte> base64Bytes = stackalloc byte[base64EncodedLength];

        Base64.EncodeToUtf8(inputAsUTF8Bytes, base64Bytes, out _, out _);

        int paddingSize = base64Bytes switch // Check the last 2 characters
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

    public static string UrlFriendlyDecode_Simplest(string input)
    {
        input = input.Replace(_charHyphen, _charSlash).Replace(_charUnderscore, _charPlus);

        int paddingSize = getUrlFriendlyPaddingSize(input.Length);

        for (int i = 0; i < paddingSize; i++)
            input += '=';

        return Encoding.UTF8.GetString(Convert.FromBase64String(input));
    }

    public static string UrlFriendlyDecode_Simple(ReadOnlySpan<char> input)
    {
        int paddingSize = getUrlFriendlyPaddingSize(input.Length);

        Span<char> base64Input = stackalloc char[input.Length + paddingSize];

        for (int i = 0; i < input.Length; i++)
        {
            base64Input[i] = input[i] switch
            {
                _charHyphen     => _charSlash,
                _charUnderscore => _charPlus,
                _               => input[i]
            };
        }

        for (int i = paddingSize; i > 0; i--)
            base64Input[^i] = _charEquals;

        Span<byte> base64Bytes = stackalloc byte[(base64Input.Length * 3 + 3) / 4];

        Convert.TryFromBase64Chars(base64Input, base64Bytes, out int bytesWritten);

        Span<char> finalChars = stackalloc char[bytesWritten];

        Encoding.UTF8.GetChars(base64Bytes[..bytesWritten], finalChars);

        return finalChars.TrimEnd('\0').ToString();
    }

    public static string UrlFriendlyDecode_Complex(ReadOnlySpan<char> base64Input)
    {
        int paddingSize = getUrlFriendlyPaddingSize(base64Input.Length);

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

        Base64.DecodeFromUtf8(base64bytes, utf8Bytes, out _, out int bytesWritten);

        Span<char> finalChars = stackalloc char[bytesWritten];

        Encoding.UTF8.GetChars(utf8Bytes[..bytesWritten], finalChars);

        return finalChars.TrimEnd('\0').ToString();
    }

    private static int getUrlFriendlyPaddingSize(int inputLength)
    {
        return (inputLength % 4) switch
        {
            0     => 0,
            int n => 4 - n
        };
    }
}
