using BenchmarkDotNet.Attributes;
using PlayingWithBenchmarkDotNet.Code;

namespace PlayingWithBenchmarkDotNet.Benchmark;

[MemoryDiagnoser]
public class Base64EncoderBenchmark
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
