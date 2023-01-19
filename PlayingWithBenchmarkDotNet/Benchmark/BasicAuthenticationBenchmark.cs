using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using PlayingWithBenchmarkDotNet.Code;

namespace PlayingWithBenchmarkDotNet.Benchmark;

[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser]
public class BasicAuthenticationBenchmark
{
    private string _authHeader;

    [GlobalSetup]
    public void Setup()
    {
        _authHeader = BasicAuthenticationHelper.GetTestAuthHeaderInput();
    }

    [Benchmark(Baseline = true)]
    public bool CheckBasicAuthorizationHeader()
        => BasicAuthenticationHelper.CheckBasicAuthorizationHeader(_authHeader);

    [Benchmark]
    public bool CheckBasicAuthorizationHeaderWithSpan()
        => BasicAuthenticationHelper.CheckBasicAuthorizationHeaderWithSpan(_authHeader);
}
