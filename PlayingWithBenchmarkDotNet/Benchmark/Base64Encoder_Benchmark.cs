using BenchmarkDotNet.Attributes;
using PlayingWithBenchmarkDotNet.Code;

namespace PlayingWithBenchmarkDotNet.Benchmark;

/*
| Method                      | Mean      | Allocated |
|---------------------------- |----------:|----------:|
| Encode_Simplest             |  99.32 ns |     280 B |
| Encode_Simple               |  85.12 ns |     192 B |
| Encode_Complex              | 154.84 ns |     192 B |
| Decode_Simplest             | 190.69 ns |     240 B |
| Decode_Simple               | 120.37 ns |     152 B |
| Decode_Complex              | 139.45 ns |     152 B |
| Encode_UrlFriendly_Simplest | 189.30 ns |     472 B |
| Encode_UrlFriendly_Simple   | 192.99 ns |     192 B |
| Encode_UrlFriendly_Complex  | 203.99 ns |     192 B |
| Decode_UrlFriendly_Simplest | 234.74 ns |     432 B |
| Decode_UrlFriendly_Simple   | 215.82 ns |     152 B |
| Decode_UrlFriendly_Complex  | 189.61 ns |     152 B |
*/


[ShortRunJob]
// [RankColumn]
// [Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser]
[HideColumns("Error", "StdDev", "Gen0", "RatioSD")]
public class Base64Encoder_Benchmark
{
    private const string _input       = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    private const string _encodedText = "QUJDREVGR0hJSktMTU5PUFFSU1RVVldYWVphYmNkZWZnaGlqa2xtbm9wcXJzdHV2d3h5ejAxMjM0NTY3ODk=";

    private const string _urlFriendlyEncodedText = "QUJDREVGR0hJSktMTU5PUFFSU1RVVldYWVphYmNkZWZnaGlqa2xtbm9wcXJzdHV2d3h5ejAxMjM0NTY3ODk";

    // Microsoft.AspNetCore.WebUtilities -> WebEncoders Class can be used as well
    // https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.webutilities.webencoders

    [Benchmark]
    public string Encode_Simplest() => Base64UrlEncoder.Encode_Simplest(_input);

    [Benchmark]
    public string Encode_Simple() => Base64UrlEncoder.Encode_Simple(_input);

    [Benchmark]
    public string Encode_Complex() => Base64UrlEncoder.Encode_Complex(_input);

    [Benchmark]
    public string Decode_Simplest() => Base64UrlEncoder.Decode_Simplest(_encodedText);

    [Benchmark]
    public string Decode_Simple() => Base64UrlEncoder.Decode_Simple(_encodedText);

    [Benchmark]
    public string Decode_Complex() => Base64UrlEncoder.Decode_Complex(_encodedText);

    //[Benchmark]
    //public string Encode_UrlFriendly_Token() => Microsoft.IdentityModel.Tokens.Base64UrlEncoder.Encode(_input);

    [Benchmark]
    public string Encode_UrlFriendly_Simplest() => Base64UrlEncoder.UrlFriendlyEncode_Simplest(_input);

    [Benchmark]
    public string Encode_UrlFriendly_Simple() => Base64UrlEncoder.UrlFriendlyEncode_Simple(_input);

    [Benchmark]
    public string Encode_UrlFriendly_Complex() => Base64UrlEncoder.UrlFriendlyEncode_Complex(_input);

    [Benchmark]
    public string Decode_UrlFriendly_Simplest() => Base64UrlEncoder.UrlFriendlyDecode_Simplest(_urlFriendlyEncodedText);

    [Benchmark]
    public string Decode_UrlFriendly_Simple() => Base64UrlEncoder.UrlFriendlyDecode_Simple(_urlFriendlyEncodedText);

    [Benchmark]
    public string Decode_UrlFriendly_Complex() => Base64UrlEncoder.UrlFriendlyDecode_Complex(_urlFriendlyEncodedText);

    //[Benchmark]
    //public string Decode_UrlFriendly_Token() => Microsoft.IdentityModel.Tokens.Base64UrlEncoder.Decode(_urlFriendlyEncodedText);
}
