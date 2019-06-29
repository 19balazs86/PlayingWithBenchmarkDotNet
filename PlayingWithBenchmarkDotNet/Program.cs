using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace PlayingWithBenchmarkDotNet
{
  public class Program
  {
    public static void Main(string[] args)
    {
      //Summary summary = BenchmarkRunner.Run<BenchmarkMd5VsSha256>();
      Summary summary = BenchmarkRunner.Run<BenchmarkNameParsers>();
    }
  }
}
