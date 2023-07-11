using BenchmarkDotNet.Running;
using PlayingWithBenchmarkDotNet.Benchmark;

//_ = BenchmarkRunner.Run<Md5VsSha256_Benchmarks>();
//_ = BenchmarkRunner.Run<NameParser_Benchmarks>();
//_ = BenchmarkRunner.Run<Logging_Benchmarks>();
//_ = BenchmarkRunner.Run<ReadJsonProperty_Benchmarks>();
//_ = BenchmarkRunner.Run<Guider_Benchmarks>();
//_ = BenchmarkRunner.Run<BasicAuthentication_Benchmark>();
//_ = BenchmarkRunner.Run<Base64Encoder_Benchmark>();
//_ = BenchmarkRunner.Run<ToImmutableArray_Benchmarks>();
//_ = BenchmarkRunner.Run<QueryString_Benchmarks>();
_ = BenchmarkRunner.Run<JSON_SourceGenerator_Benchmarks>();