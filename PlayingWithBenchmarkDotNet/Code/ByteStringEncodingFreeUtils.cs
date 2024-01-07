using System.Runtime.InteropServices;

namespace PlayingWithBenchmarkDotNet.Code;

public static class ByteStringEncodingFreeUtils
{
    public static byte[] GetBytesOld(string str)
    {
        var bytes = new byte[str.Length * sizeof(char)];
        Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        return bytes;
    }

    public static string GetStringOld(byte[] bytes)
    {
        var chars = new char[bytes.Length / sizeof(char)];
        Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
        return new string(chars);
    }

    public static byte[] GetBytesWithSpan(ReadOnlySpan<char> charSpan)
    {
        ReadOnlySpan<byte> byteSpan = MemoryMarshal.AsBytes(charSpan);

        var bytes = new byte[byteSpan.Length];

        byteSpan.CopyTo(bytes);

        return bytes;
    }

    public static string GetStringWithSpan(ReadOnlySpan<byte> bytes)
    {
        ReadOnlySpan<char> charsSpan = MemoryMarshal.Cast<byte, char>(bytes);

        return new string(charsSpan);
    }
}
