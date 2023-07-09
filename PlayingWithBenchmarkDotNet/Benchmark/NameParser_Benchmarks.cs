using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using PlayingWithBenchmarkDotNet.Code;

namespace PlayingWithBenchmarkDotNet.Benchmark;

[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser]
public class NameParser_Benchmarks
{
    private const string _fullName = "Full Name Here";

    [Benchmark(Baseline = true)]
    public void GetLastName() => NameParser.GetLastName(_fullName);

    [Benchmark]
    public void GetLastNameUsingSubstring() => NameParser.GetLastNameUsingSubstring(_fullName);

    [Benchmark]
    public void GetLastNameWithSpan() => NameParser.GetLastNameWithSpan(_fullName);
}
