using BenchmarkDotNet.Attributes;
using PlayingWithBenchmarkDotNet.Code;

namespace PlayingWithBenchmarkDotNet.Benchmark;

/*
|              Method |      Mean |    Error |   StdDev | Allocated |
|-------------------- |----------:|---------:|---------:|----------:|
|    ToGuidFromString | 144.26 ns | 2.948 ns | 3.509 ns |     256 B |
| ToGuidFromString_Op |  75.04 ns | 0.503 ns | 0.471 ns |         - |
|    ToStringFromGuid | 153.55 ns | 1.143 ns | 0.955 ns |     328 B |
| ToStringFromGuid_Op |  63.60 ns | 0.218 ns | 0.204 ns |      72 B |
*/

[ShortRunJob]
// [RankColumn]
// [Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser]
[HideColumns("Error", "StdDev", "Gen0", "RatioSD")]
public class Guider_Benchmarks
{
    private static readonly Guid _testIdAsGuid = Guid.Parse("4dfaaf49-1f03-41f8-b6cc-fd892c1d062f");
    private const string _testIdAsString       = "Sa-6TQMf_EG2zP2JLB0GLw";

    [Benchmark]
    public Guid ToGuidFromString()
    {
        return Guider.ToGuidFromString(_testIdAsString);
    }

    [Benchmark]
    public Guid ToGuidFromString_Op()
    {
        return Guider.ToGuidFromStringOp(_testIdAsString);
    }

    [Benchmark]
    public string ToStringFromGuid()
    {
        return Guider.ToStringFromGuid(_testIdAsGuid);
    }

    [Benchmark]
    public string ToStringFromGuid_Op()
    {
        return Guider.ToStringFromGuidOp(_testIdAsGuid);
    }
}
