using BenchmarkDotNet.Attributes;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace PlayingWithBenchmarkDotNet.Benchmark;

// Solving 1 of the Biggest Array issues
// Nick Chapsas: https://youtu.be/lT5n2o3kuno

[MemoryDiagnoser]
public class ToImmutableArrayBenchmarks
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