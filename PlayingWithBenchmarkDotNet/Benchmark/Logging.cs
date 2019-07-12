using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace PlayingWithBenchmarkDotNet.Benchmark
{
  [RankColumn]
  [Orderer(SummaryOrderPolicy.FastestToSlowest)]
  [MemoryDiagnoser]
  public class Logging
  {
    private readonly ILogger _logger = NullLogger.Instance;

    private readonly Action<ILogger, string, int, Exception> _definedLogger
      = LoggerMessage.Define<string, int>(
          logLevel: LogLevel.Information,
          eventId: 1,
          formatString: "Name: '{n}', Age: {a}");

    [Benchmark(Baseline = true)]
    public void LogStringInterpolation() => _logger.LogInformation($"Name: '{"Name"}', Age: {0}");

    [Benchmark]
    public void LogWithMessageAndParams() => _logger.LogInformation("Name: '{n}', Age: {a}", "Name", 0);

    [Benchmark]
    public void LogWithPredefined() => _definedLogger(_logger, "Name", 0, null);
  }
}
