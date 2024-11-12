using BenchmarkDotNet.Attributes;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace PlayingWithBenchmarkDotNet.Benchmark;

// Solving 1 of the Biggest Array issues
// Nick Chapsas: https://youtu.be/lT5n2o3kuno

/*
| Method                           | ItemCount | Mean       | Ratio | Allocated | Alloc Ratio |
|--------------------------------- |---------- |-----------:|------:|----------:|------------:|
| Normal                           | 100       | 99.7384 ns | 1.000 |     824 B |        1.00 |
| CollectionExpression             | 100       | 58.2143 ns | 0.584 |     824 B |        1.00 |
| With_UnsafeAs_ImmutableArray     | 100       |  0.0000 ns | 0.000 |         - |        0.00 |
| With_ImmutableCollectionsMarshal | 100       |  0.0000 ns | 0.000 |         - |        0.00 |
*/

[ShortRunJob]
// [RankColumn]
// [Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser]
[HideColumns("Error", "StdDev", "Gen0", "RatioSD")]
public class ToImmutableArray_Benchmarks
{
    private static readonly Random _random = new Random(500);

    private User[] _users;

    [Params(100)]
    public int ItemCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _users = Enumerable.Range(0, ItemCount)
            .Select(_ => new User { Age = _random.Next(100) })
            .ToArray();
    }

    [Benchmark(Baseline = true)]
    public ImmutableArray<User> Normal()
    {
        return _users.ToImmutableArray();
    }

    [Benchmark]
    public ImmutableArray<User> CollectionExpression()
    {
        return [.._users];
    }

    [Benchmark]
    public ImmutableArray<User> With_UnsafeAs_ImmutableArray()
    {
        return Unsafe.As<User[], ImmutableArray<User>>(ref _users);
    }

    [Benchmark]
    public ImmutableArray<User> With_ImmutableCollectionsMarshal()
    {
        return ImmutableCollectionsMarshal.AsImmutableArray(_users);
    }
}

public sealed class User
{
    public int Age { get; init; }
}
