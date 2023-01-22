using BenchmarkDotNet.Running;
using PlayingWithBenchmarkDotNet.Benchmark;

//_ = BenchmarkRunner.Run<Md5VsSha256>();
//_ = BenchmarkRunner.Run<NameParsers>();
//_ = BenchmarkRunner.Run<Logging>();
//_ = BenchmarkRunner.Run<ReadJsonProperty>();
//_ = BenchmarkRunner.Run<GuiderBenchmarks>();
_ = BenchmarkRunner.Run<BasicAuthenticationBenchmark>();
//_ = BenchmarkRunner.Run<Base64EncoderBenchmark>();