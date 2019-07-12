using BenchmarkDotNet.Running;
using PlayingWithBenchmarkDotNet.Benchmark;

namespace PlayingWithBenchmarkDotNet
{
  public class Program
  {
    public static void Main(string[] args)
    {
      //var summary = BenchmarkRunner.Run<Md5VsSha256>();
      //var summary = BenchmarkRunner.Run<NameParsers>();
      //var summary = BenchmarkRunner.Run<Logging>();
      var summary = BenchmarkRunner.Run<ReadJsonProperty>();
    }
  }
}
