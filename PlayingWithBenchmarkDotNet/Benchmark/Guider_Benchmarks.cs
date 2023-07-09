using BenchmarkDotNet.Attributes;
using PlayingWithBenchmarkDotNet.Code;

namespace PlayingWithBenchmarkDotNet.Benchmark;

[MemoryDiagnoser(displayGenColumns: false)]
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
