using BenchmarkDotNet.Attributes;
using PlayingWithBenchmarkDotNet.Code;

namespace PlayingWithBenchmarkDotNet.Benchmark;

// Byte representation of string without Encoding
// https://code-maze.com/csharp-consistent-byte-representation-of-strings-without-encoding/

/*
| Method              | Mean      | Allocated |
|-------------------- |----------:|----------:|
| GetBytes_Old        | 31.312 ns |     208 B |
| GetString_Old       | 31.683 ns |     208 B |
| GetBytes_with_Span  |  9.006 ns |     104 B |
| GetString_with_Span | 14.877 ns |     104 B |
*/

[ShortRunJob]
[MemoryDiagnoser]
[HideColumns("Error", "StdDev", "Gen0", "RatioSD")]
public class ByteStringEncodingFree_Benchmarks
{
    private const string _text = "This is the string we convert to bytes!";

    private static readonly byte[] _bytes = ByteStringEncodingFreeUtils.GetBytesWithSpan(_text);

    [Benchmark]
    public byte[] GetBytes_Old()
    {
        return ByteStringEncodingFreeUtils.GetBytesOld(_text);
    }

    [Benchmark]
    public string GetString_Old()
    {
        return ByteStringEncodingFreeUtils.GetStringOld(_bytes);
    }

    [Benchmark]
    public byte[] GetBytes_with_Span()
    {
        return ByteStringEncodingFreeUtils.GetBytesWithSpan(_text);
    }

    [Benchmark]
    public string GetString_with_Span()
    {
        return ByteStringEncodingFreeUtils.GetStringWithSpan(_bytes);
    }
}