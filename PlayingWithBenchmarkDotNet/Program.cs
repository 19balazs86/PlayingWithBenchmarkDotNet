using BenchmarkDotNet.Running;
using PlayingWithBenchmarkDotNet.Benchmark;

//_ = BenchmarkRunner.Run<Md5VsSha256Benchmarks>();
//_ = BenchmarkRunner.Run<NameParserBenchmarks>();
//_ = BenchmarkRunner.Run<LoggingBenchmarks>();
//_ = BenchmarkRunner.Run<ReadJsonPropertyBenchmarks>();
//_ = BenchmarkRunner.Run<GuiderBenchmarks>();
//_ = BenchmarkRunner.Run<BasicAuthenticationBenchmark>();
//_ = BenchmarkRunner.Run<Base64EncoderBenchmark>();
//_ = BenchmarkRunner.Run<ToImmutableArrayBenchmarks>();
_ = BenchmarkRunner.Run<JSON_SourceGeneratorBenchmarks>();