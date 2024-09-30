using BenchmarkDotNet.Attributes;
using System.Buffers;

namespace PlayingWithBenchmarkDotNet.Benchmark;

// Nick Chapsas video: https://youtu.be/q0VENoIXWso

/*
 * | Method                | ExampleText     | Mean         | Allocated |
 * |---------------------- |-----------------|-------------:|----------:|
 * | IsBase64_SearchValues | +FO7x[zW        |     5.637 ns |         - |
 * | IsBase64_CharArray    | +FO7x[zW        |    44.547 ns |         - |
 * | IsBase64_Naive        | +FO7x[zW        |    66.513 ns |         - |
 * | IsBase64_SearchValues | Gm6N1(...)ZUkf5 |     8.586 ns |         - |
 * | IsBase64_CharArray    | Gm6N1(...)ZUkf5 |    72.708 ns |         - |
 * | IsBase64_Naive        | Gm6N1(...)ZUkf5 | 1,012.434 ns |         - |
 * | IsBase64_SearchValues | xlyUKFvk        |     5.247 ns |         - |
 * | IsBase64_CharArray    | xlyUKFvk        |    57.095 ns |         - |
 * | IsBase64_Naive        | xlyUKFvk        |    87.094 ns |         - |
 */

[ShortRunJob]
//[RankColumn]
//[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser]
[HideColumns("Error", "StdDev", "Gen0", "RatioSD")]
public class SearchValues_Benchmark
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

    [Benchmark]
    public bool IsBase64_SearchValues()
    {
        return ExampleText.AsSpan().IndexOfAnyExcept(_base64SearchValues) == -1;
    }

    [Benchmark]
    public bool IsBase64_CharArray()
    {
        return ExampleText.AsSpan().IndexOfAnyExcept(_base64CharArray) == -1;
    }

    [Benchmark]
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
}
