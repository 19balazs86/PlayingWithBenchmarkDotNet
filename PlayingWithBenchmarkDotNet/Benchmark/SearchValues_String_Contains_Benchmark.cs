using BenchmarkDotNet.Attributes;
using System.Buffers;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace PlayingWithBenchmarkDotNet.Benchmark;

// Performance Improvements in .NET 9
// .NET Conf 2024: https://youtu.be/aLQpnpSxosg

/*
| Method                  | DwarfName | Mean     | Ratio |  Allocated |
|------------------------ |---------- |---------:|------:|  ---------:|
| Array_Contains          | Chuckle   | 237.4 us |  1.00 |          - |
| Array_BinarySearch      | Chuckle   | 254.5 us |  1.07 |        1 B |
| HashSet_Contains        | Chuckle   | 802.3 us |  3.38 |  400.001 B |
| ReadOnlySet_Contains    | Chuckle   | 890.0 us |  3.75 |  400.002 B |
| ImmutableArray_Contains | Chuckle   | 321.3 us |  1.35 |  240.001 B |
| SearchValues_Contains   | Chuckle   | 128.2 us |  0.54 |          - |
|                         |           |          |       |            |
| Array_Contains          | Happy     | 108.5 us |  1.00 |          - |
| Array_BinarySearch      | Happy     | 234.9 us |  2.16 |          - |
| HashSet_Contains        | Happy     | 344.7 us |  3.18 |  400.000 B |
| ReadOnlySet_Contains    | Happy     | 391.7 us |  3.61 |  400.001 B |
| ImmutableArray_Contains | Happy     | 189.7 us |  1.75 |  240.000 B |
| SearchValues_Contains   | Happy     | 142.3 us |  1.31 |          - |
|                         |           |          |       |            |
| Array_Contains          | SNEEZY    | 300.9 us |  1.00 |          - |
| Array_BinarySearch      | SNEEZY    | 307.9 us |  1.02 |        1 B |
| HashSet_Contains        | SNEEZY    | 845.8 us |  2.81 |  400.001 B |
| ReadOnlySet_Contains    | SNEEZY    | 869.4 us |  2.89 |  400.001 B |
| ImmutableArray_Contains | SNEEZY    | 363.0 us |  1.21 |  240.000 B |
| SearchValues_Contains   | SNEEZY    | 186.2 us |  0.62 |          - |
*/

[ShortRunJob]
// [RankColumn]
// [Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser]
[HideColumns("Error", "StdDev", "Gen0", "RatioSD")]
public class SearchValues_String_Contains_Benchmark
{
    private const int _iterations = 10_000;

    private static readonly string[]               _dwarfsArray        = ["Doc", "Grumpy", "Happy", "Sleepy", "Dopey", "Bashful", "Sneezy"];
    private static readonly HashSet<string>        _dwarfsHashSet      = _dwarfsArray.ToHashSet();
    private static readonly ReadOnlySet<string>    _dwarfsReadOnlySet  = new(_dwarfsArray.ToHashSet());
    private static readonly ImmutableArray<string> _dwarfsImmutable    = ImmutableCollectionsMarshal.AsImmutableArray(_dwarfsArray);
    private static readonly SearchValues<string>   _dwarfsSearchValues = SearchValues.Create(_dwarfsArray, StringComparison.OrdinalIgnoreCase);

    [Params("Chuckle", "Happy", "SNEEZY")]
    public string DwarfName { get; set; }

    [Benchmark(Baseline = true)]
    public void Array_Contains()
    {
        for (int i = 0; i < _iterations; i++)
        {
            _ = _dwarfsArray.Contains(DwarfName, StringComparer.OrdinalIgnoreCase);
        }
    }

    [Benchmark]
    public void Array_BinarySearch()
    {
        for (int i = 0; i < _iterations; i++)
        {
            _ = Array.BinarySearch(_dwarfsArray, DwarfName, StringComparer.OrdinalIgnoreCase) != -1;
        }
    }

    [Benchmark]
    public void HashSet_Contains()
    {
        for (int i = 0; i < _iterations; i++)
        {
            _ = _dwarfsHashSet.Contains(DwarfName, StringComparer.OrdinalIgnoreCase);
        }
    }

    [Benchmark]
    public void ReadOnlySet_Contains()
    {
        for (int i = 0; i < _iterations; i++)
        {
            _ = _dwarfsReadOnlySet.Contains(DwarfName, StringComparer.OrdinalIgnoreCase);
        }
    }

    [Benchmark]
    public void ImmutableArray_Contains()
    {
        for (int i = 0; i < _iterations; i++)
        {
            _ = _dwarfsImmutable.Contains(DwarfName, StringComparer.OrdinalIgnoreCase);
        }
    }

    [Benchmark]
    public void SearchValues_Contains()
    {
        for (int i = 0; i < _iterations; i++)
        {
            _ = _dwarfsSearchValues.Contains(DwarfName); // Defined as OrdinalIgnoreCase
        }
    }
}
