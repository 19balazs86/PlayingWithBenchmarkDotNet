using BenchmarkDotNet.Attributes;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace PlayingWithBenchmarkDotNet.Benchmark;

// Solving 1 of the Biggest Array issues
// Nick Chapsas: https://youtu.be/lT5n2o3kuno

/*
| Method                           | ItemCount | Mean       | Allocated |
|--------------------------------- |---------- |-----------:|----------:|
| Normal                           | 100       | 96.1604 ns |     824 B |
| With_UnsafeAs_ImmutableArray     | 100       |  0.0045 ns |         - |
| With_ImmutableCollectionsMarshal | 100       |  0.0254 ns |         - |
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