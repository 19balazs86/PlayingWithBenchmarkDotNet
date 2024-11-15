using BenchmarkDotNet.Attributes;
using System.Buffers;

namespace PlayingWithBenchmarkDotNet.Benchmark;

// Nick Chapsas video: https://youtu.be/q0VENoIXWso

/*
| Method                | ExampleText          | Mean       | Ratio | Allocated |
|---------------------- |--------------------- |-----------:|------:|----------:|
| IsBase64_Naive        | +FO7x[zW             |  58.360 ns |  1.00 |         - |
| IsBase64_CharArray    | +FO7x[zW             |  34.148 ns |  0.59 |         - |
| IsBase64_SearchValues | +FO7x[zW             |   4.432 ns |  0.08 |         - |
|                       |                      |            |       |           |
| IsBase64_Naive        | Gm6N1(...)ZUkf5 [64] | 923.094 ns | 1.000 |         - |
| IsBase64_CharArray    | Gm6N1(...)ZUkf5 [64] |  74.164 ns | 0.080 |         - |
| IsBase64_SearchValues | Gm6N1(...)ZUkf5 [64] |   7.223 ns | 0.008 |         - |
|                       |                      |            |       |           |
| IsBase64_Naive        | xlyUKFvk             |  76.554 ns |  1.00 |         - |
| IsBase64_CharArray    | xlyUKFvk             |  42.544 ns |  0.56 |         - |
| IsBase64_SearchValues | xlyUKFvk             |   3.873 ns |  0.05 |         - |
*/

[ShortRunJob]
// [RankColumn]
// [Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser]
[HideColumns("Error", "StdDev", "Gen0", "RatioSD")]
public class SearchValues_Char_Benchmark
{
    private const string _base64String = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

    private static readonly char[] _base64CharArray = [
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '+', '/'];

    private static readonly SearchValues<char> _base64SearchValues = SearchValues.Create(_base64String);

    //        Valid,     Invalid,      Valid
    [Params("xlyUKFvk", "+FO7x[zW", "Gm6N1lqEynFb4JIiyVu6N8SrkpmZE7Ye3v3L2fUO6J5X6kJ1OpJd55iGyGHZUkf5")]
    public string ExampleText { get; set; }

    [Benchmark(Baseline = true)]
    public bool IsBase64_Naive()
    {
        int length = ExampleText.Length;

        for (int i = 0; i < length; i++)
        {
            char c = ExampleText[i];

            if (!_base64CharArray.Contains(c))
            {
                return false;
            }
        }

        return true;
    }

    [Benchmark]
    public bool IsBase64_CharArray()
    {
        return ExampleText.AsSpan().IndexOfAnyExcept(_base64CharArray) == -1;
    }

    [Benchmark]
    public bool IsBase64_SearchValues()
    {
        return ExampleText.AsSpan().IndexOfAnyExcept(_base64SearchValues) == -1;
    }
}
