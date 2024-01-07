using BenchmarkDotNet.Attributes;
using PlayingWithBenchmarkDotNet.Code;

namespace PlayingWithBenchmarkDotNet.Benchmark;

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
    public void GetBytes_Old()
    {
        ByteStringEncodingFreeUtils.GetBytesOld(_text);
    }

    [Benchmark]
    public void GetString_Old()
    {
        ByteStringEncodingFreeUtils.GetStringOld(_bytes);
    }

    [Benchmark]
    public void GetBytesWithSpan()
    {
        ByteStringEncodingFreeUtils.GetBytesWithSpan(_text);
    }

    [Benchmark]
    public void GetStringWithSpan()
    {
        ByteStringEncodingFreeUtils.GetStringWithSpan(_bytes);
    }
}