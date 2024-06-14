using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using System.Collections.Immutable;

namespace PlayingWithBenchmarkDotNet.Benchmark;

/*
| Method                       | Mean      | Rank | Allocated |
|----------------------------- |----------:|-----:|----------:|
| GetValue_Switch_Bitwise      |  3.515 ms |    1 |       2 B |
| getValue_Switch_ToUpper      |  7.098 ms |    2 |       3 B |
| GetValue_Dictionary          | 16.229 ms |    3 |       6 B |
| GetValue_ImmutableDictionary | 17.209 ms |    4 |       6 B |
 */

//[ShortRunJob]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser(displayGenColumns: false)]
public class UpperCase_Benchmark
{
    #region Fields
    private const string _alphabet = "AaBbCcDdEeFfGgHhIiJj";
    private static readonly int _alphabetLength = _alphabet.Length;

    private static readonly ImmutableArray<string> _alphabetStringArray =
    [
        "A", "a", "B", "b", "C", "c", "D", "d", "E", "e",
        "F", "f", "G", "g", "H", "h", "I", "i", "J", "j"
    ];

    private static readonly Dictionary<string, int> _alphabetDictionary = new Dictionary<string, int>(10, StringComparer.OrdinalIgnoreCase)
    {
        ["A"] = 0, ["B"] = 1, ["C"] = 2, ["D"] = 3, ["E"] = 4, ["F"] = 5, ["G"] = 6, ["H"] = 7, ["I"] = 8, ["J"] = 9,
    };

    private static readonly ImmutableDictionary<string, int> _alphabetImmutableDictionary =
        _alphabetDictionary
            .ToImmutableDictionary()
            .WithComparers(StringComparer.OrdinalIgnoreCase);

    private const int _iterations = 1_000_000;

    #endregion

    [Benchmark]
    public void GetValue_Switch_Bitwise()
    {
        for (int i = 0; i < _iterations; i++)
        {
            getValueSwitchBitwise(_alphabet[i % _alphabetLength]);
        }
    }

    [Benchmark]
    public void getValue_Switch_ToUpper()
    {
        for (int i = 0; i < _iterations; i++)
        {
            getValueSwitchToUpper(_alphabet[i % _alphabetLength]);
        }
    }

    [Benchmark]
    public void GetValue_Dictionary()
    {
        for (int i = 0; i < _iterations; i++)
        {
            getValueDictionary(_alphabetStringArray[i % _alphabetLength]);
        }
    }

    [Benchmark]
    public void GetValue_ImmutableDictionary()
    {
        for (int i = 0; i < _iterations; i++)
        {
            getValueImmutableDictionary(_alphabetStringArray[i % _alphabetLength]);
        }
    }

    private static int getValueSwitchBitwise(char c) => (c & ~32) switch // ToLower: (c | 32)
    {
        'A' => 0,
        'B' => 1,
        'C' => 2,
        'D' => 3,
        'E' => 4,
        'F' => 5,
        'G' => 6,
        'H' => 7,
        'I' => 8,
        'J' => 9,
        _ => throw new ArgumentOutOfRangeException(),
    };

    private static int getValueSwitchToUpper(char c) => char.ToUpper(c) switch
    {
        'A' => 0,
        'B' => 1,
        'C' => 2,
        'D' => 3,
        'E' => 4,
        'F' => 5,
        'G' => 6,
        'H' => 7,
        'I' => 8,
        'J' => 9,
        _ => throw new ArgumentOutOfRangeException(),
    };

    private static int getValueDictionary(string c) => _alphabetDictionary[c];

    private static int getValueImmutableDictionary(string c) => _alphabetImmutableDictionary[c];
}
