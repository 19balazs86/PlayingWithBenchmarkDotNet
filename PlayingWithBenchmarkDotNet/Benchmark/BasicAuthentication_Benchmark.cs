using BenchmarkDotNet.Attributes;
using PlayingWithBenchmarkDotNet.Code;

namespace PlayingWithBenchmarkDotNet.Benchmark;

/*
| Method                                | Mean       | Ratio | Allocated | Alloc Ratio |
|-------------------------------------- |-----------:|------:|----------:|------------:|
| CheckBasicAuthorizationHeaderWithSpan |   302.9 ns |  0.18 |         - |        0.00 |
| CheckBasicAuthorizationHeader         | 1,667.0 ns |  1.00 |     664 B |        1.00 |
*/


[ShortRunJob]
// [RankColumn]
// [Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser]
[HideColumns("Error", "StdDev", "Gen0", "RatioSD")]
public class BasicAuthentication_Benchmark
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
