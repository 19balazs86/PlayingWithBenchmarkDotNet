using BenchmarkDotNet.Running;
using PlayingWithBenchmarkDotNet.Benchmark;

//_ = BenchmarkRunner.Run<Logging_Benchmarks>();
//_ = BenchmarkRunner.Run<Guider_Benchmarks>();
//_ = BenchmarkRunner.Run<BasicAuthentication_Benchmark>();
//_ = BenchmarkRunner.Run<Base64Encoder_Benchmark>();
//_ = BenchmarkRunner.Run<ToImmutableArray_Benchmarks>();
//_ = BenchmarkRunner.Run<QueryString_Benchmarks>();
// _ = BenchmarkRunner.Run<BytesToCommaSeparatedBase64String_Benchmarks>();
//_ = BenchmarkRunner.Run<JSON_SourceGenerator_Benchmarks>();
//_ = BenchmarkRunner.Run<ByteStringEncodingFree_Benchmarks>();
_ = BenchmarkRunner.Run<ArrayBatching_Benchmark>();