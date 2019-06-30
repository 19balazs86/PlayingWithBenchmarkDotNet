using BenchmarkDotNet.Running;
using PlayingWithBenchmarkDotNet.Benchmark;

namespace PlayingWithBenchmarkDotNet
{
  public class Program
  {
    public static void Main(string[] args)
    {
      //var summary = BenchmarkRunner.Run<BenchmarkMd5VsSha256>();
      var summary = BenchmarkRunner.Run<NameParsers>();
    }
  }
}
