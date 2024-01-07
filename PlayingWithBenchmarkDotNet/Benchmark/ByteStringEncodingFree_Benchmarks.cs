using BenchmarkDotNet.Attributes;
using PlayingWithBenchmarkDotNet.Code;

namespace PlayingWithBenchmarkDotNet.Benchmark;

// Byte representation of string without Encoding
// https://code-maze.com/csharp-consistent-byte-representation-of-strings-without-encoding/

/*
| Method            | Mean     | Allocated |
|------------------ |---------:|----------:|
| GetBytes_Old      | 31.23 ns |     208 B |
| GetString_Old     | 32.20 ns |     208 B |
| GetBytesWithSpan  | 11.49 ns |     104 B |
| GetStringWithSpan | 11.56 ns |     104 B |
*/

[MemoryDiagnoser]
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
    public byte[] GetBytesWithSpan()
    {
        return ByteStringEncodingFreeUtils.GetBytesWithSpan(_text);
    }

    [Benchmark]
    public string GetStringWithSpan()
    {
        return ByteStringEncodingFreeUtils.GetStringWithSpan(_bytes);
    }
}