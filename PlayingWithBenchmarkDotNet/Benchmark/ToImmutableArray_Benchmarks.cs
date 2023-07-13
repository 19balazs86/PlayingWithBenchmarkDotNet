using BenchmarkDotNet.Attributes;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace PlayingWithBenchmarkDotNet.Benchmark;

// Solving 1 of the Biggest Array issues
// Nick Chapsas: https://youtu.be/lT5n2o3kuno

/*
|                       Method | ItemCount |        Mean |     Error |    StdDev |   Gen0 | Allocated |
|----------------------------- |---------- |------------:|----------:|----------:|-------:|----------:|
|                       Normal |       100 | 112.5971 ns | 1.0058 ns | 0.8917 ns | 0.1969 |     824 B |
| With_UnsafeAs_ImmutableArray |       100 |   0.0000 ns | 0.0000 ns | 0.0000 ns |      - |         - |
*/

[MemoryDiagnoser]
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

    [Benchmark]
    public ImmutableArray<User> Normal()
    {
        return _users.ToImmutableArray();
    }

    [Benchmark]
    public ImmutableArray<User> With_UnsafeAs_ImmutableArray()
    {
        return Unsafe.As<User[], ImmutableArray<User>>(ref _users);
    }

    // Coming in .NET 8
    //[Benchmark]
    //public ImmutableArray<User> With_ImmutableCollectionsMarshal()
    //{
    //    return ImmutableCollectionsMarshal.AsImmutableArray(_users);
    //}
}

public sealed class User
{
    public int Age { get; init; }
}