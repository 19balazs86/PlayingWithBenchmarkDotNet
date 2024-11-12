using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace PlayingWithBenchmarkDotNet.Benchmark;

/* The idea of this topic coming from Dev Leader: https://youtu.be/_MLajy6jw9o
| Method                     | Mean       | Ratio | Rank | Allocated |
|--------------------------- |-----------:|------:|-----:|----------:|
| Span                       |   303.0 us |  0.04 |    1 |         - |
| MemoryChunkExtention       |   320.6 us |  0.04 |    1 |      72 B |
| ArraySegment               |   691.1 us |  0.08 |    2 |         - |
| ArraySegmentChunkExtention |   710.3 us |  0.08 |    2 |      72 B |
| Chunk                      | 8,375.2 us |  1.00 |    3 | 4114310 B |
*/

[ShortRunJob]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser]
[HideColumns("Error", "StdDev", "Gen0", "RatioSD")]
public class ArrayBatching_Benchmark
{
    private int[] _numbers;

    private readonly int _collectionSize = 1_000_000;

    private readonly int _batchSize = 500;

    [GlobalSetup]
    public void Setup()
    {
        _numbers = Enumerable.Range(1, _collectionSize).ToArray();
    }

    [Benchmark(Baseline = true)]
    public void Chunk()
    {
        foreach (IEnumerable<int> chunk in _numbers.Chunk(_batchSize))
        {
            foreach(int value in chunk)
            {
                // Do something
            }
        }
    }

    [Benchmark]
    public void ArraySegment()
    {
        for (int i = 0; i < _collectionSize; i += _batchSize)
        {
            int count = Math.Min(_batchSize, _collectionSize - i);

            var arraySegment = new ArraySegment<int>(_numbers, i, count);

            foreach (int number in arraySegment)
            {
                // Do something
            }
        }
    }

    [Benchmark]
    public void Span()
    {
        ReadOnlySpan<int> spanNumbers = _numbers;

        for (int i = 0; i < _collectionSize; i += _batchSize)
        {
            int count = Math.Min(_batchSize, _collectionSize - i);

            var spanSlice = spanNumbers.Slice(i, count);

            foreach (int number in spanSlice)
            {
                // Do something
            }
        }
    }

    [Benchmark]
    public void ArraySegmentChunkExtention()
    {
        foreach (ArraySegment<int> chunkSegment in _numbers.ChunkWithArraySegment(_batchSize))
        {
            foreach (int value in chunkSegment)
            {
                // Do something
            }
        }
    }

    [Benchmark]
    public void MemoryChunkExtention()
    {
        foreach (ReadOnlyMemory<int> chunkMemory in _numbers.ChunkWithMemory(_batchSize))
        {
            foreach (int value in chunkMemory.Span)
            {
                // Do something
            }
        }
    }
}


file static class ChunkExtensions
{
    public static IEnumerable<ArraySegment<int>> ChunkWithArraySegment(this int[] numbers, int batchSize)
    {
        int collectionSize = numbers.Length;

        for (int i = 0; i < collectionSize; i += batchSize)
        {
            int count = Math.Min(batchSize, collectionSize - i);

            yield return new ArraySegment<int>(numbers, i, count);
        }
    }

    // This does not work with Span but with Memory
    public static IEnumerable<ReadOnlyMemory<int>> ChunkWithMemory(this int[] numbers, int batchSize)
    {
        int collectionSize = numbers.Length;

        for (int i = 0; i < collectionSize; i += batchSize)
        {
            int count = Math.Min(batchSize, collectionSize - i);

            yield return new ReadOnlyMemory<int>(numbers, i, count);
        }
    }
}
