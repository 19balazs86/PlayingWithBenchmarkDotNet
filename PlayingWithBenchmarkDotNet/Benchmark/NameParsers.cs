using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using PlayingWithBenchmarkDotNet.Code;

namespace PlayingWithBenchmarkDotNet.Benchmark
{
  [RankColumn]
  [Orderer(SummaryOrderPolicy.FastestToSlowest)]
  [MemoryDiagnoser]
  public class NameParsers
  {
    private const string _fullName = "Full Name Here";

    private static readonly NameParser _parser = new NameParser();

    [Benchmark(Baseline = true)]
    public void GetLastName() => _parser.GetLastName(_fullName);

    [Benchmark]
    public void GetLastNameUsingSubstring() => _parser.GetLastNameUsingSubstring(_fullName);

    [Benchmark]
    public void GetLastNameWithSpan() => _parser.GetLastNameWithSpan(_fullName);
  }
}
