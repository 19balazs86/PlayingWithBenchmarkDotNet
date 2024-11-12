using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace PlayingWithBenchmarkDotNet.Benchmark;

/*
| Method                  | Mean       | Ratio | Allocated | Alloc Ratio |
|------------------------ |-----------:|------:|----------:|------------:|
| LogStringInterpolation  | 68.6516 ns | 1.000 |      72 B |        1.00 |
| LogWithMessageAndParams | 42.8291 ns | 0.624 |      64 B |        0.89 |
| LogWithPredefined       |  1.1355 ns | 0.017 |         - |        0.00 |
| LogWithSourceGenerator  |  0.3120 ns | 0.005 |         - |        0.00 |
*/

[ShortRunJob]
// [RankColumn]
// [Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser]
[HideColumns("Error", "StdDev", "Gen0", "RatioSD")]
public class Logging_Benchmarks
{
    private readonly ILogger _logger = NullLogger.Instance;

    private readonly Action<ILogger, string, int, Exception> _definedLogger = LoggerMessage.Define<string, int>(
        logLevel: LogLevel.Information,
        eventId: 1,
        formatString: "Hello -> Name: '{name}', Age: {age}");

    private const string _name = "My name";
    private const int _age     = 30;

    [Benchmark(Baseline = true)]
    public void LogStringInterpolation() => _logger.LogInformation($"Name: '{_name}', Age: {_age}");

    [Benchmark]
    public void LogWithMessageAndParams() => _logger.LogInformation("Name: '{name}', Age: {age}", _name, _age);

    [Benchmark]
    public void LogWithPredefined() => _definedLogger(_logger, _name, _age, null);

    [Benchmark]
    public void LogWithSourceGenerator() => _logger.Hello(_name, _age);
}

public static partial class LogWithSourceGenerator
{
    [LoggerMessage(eventId: 1, LogLevel.Information, "Name: '{name}', Age: {age}")]
    public static partial void Hello(this ILogger logger, string name, int age);
}
