using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Primitives;
using System.Collections.Specialized;
using System.Web;

namespace PlayingWithBenchmarkDotNet.Benchmark;

[MemoryDiagnoser]
public class QueryString_Benchmarks
{
    private readonly char[] _separatorsQuery = new char[] { '?', '&' };
    private readonly char[] _separatorsSplit = new char[] { '=' };

    private const string _queryKey1 = "key2";
    private const string _queryKey2 = "key2="; // To make sure the StartsWith find the proper key, add the '=' at the end

    private const string _query = "?view=aspnetcore-7.0&key1=value1&key2=value2";

    [Benchmark]
    public string Old_HttpUtility_ParseQueryString_Get()
    {
        NameValueCollection query = HttpUtility.ParseQueryString(_query);

        return query.Get(_queryKey1);
    }

    //[Benchmark] // Just uncomment it as the optimized version is currently unavailable
    //public string Old_HttpUtility_ParseQueryString_Remove()
    //{
    //    NameValueCollection query = HttpUtility.ParseQueryString(_query);

    //    query.Remove(_queryKey1);

    //    return query.ToString();
    //}

    // StringTokenizer: https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.primitives.stringtokenizer
    [Benchmark]
    public string Better_String_Tokenizer_Get()
    {
        var segments = new StringTokenizer(_query, _separatorsQuery);

        foreach (StringSegment segment in segments)
        {
            if (!segment.HasValue)
            {
                continue;
            }

            if (segment.StartsWith(_queryKey2, StringComparison.OrdinalIgnoreCase))
            {
                return segment.Split(_separatorsSplit).Last().Value;
            }
        }

        return string.Empty;
    }

    //[Benchmark]
    //public string Better_String_Tokenizer_Remove()
    //{
    //    // Use heap allocation if we have a long query string
    //    Span<char> chars = _query.Length < 256 ? stackalloc char[256] : new char[_query.Length];

    //    int length = 0;

    //    var segments = new StringTokenizer(_query, _separatorsQuery);

    //    foreach (StringSegment segment in segments)
    //    {
    //        if (!segment.HasValue || !segment.StartsWith(_queryKey2, StringComparison.OrdinalIgnoreCase))
    //        {
    //            continue;
    //        }

    //        if (length > 0)
    //        {
    //            chars[length++] = '&';
    //        }

    //        // The following parameters for TryWrite is in .NET 8
    //        chars[length..].TryWrite(segment.Value, out int written);

    //        length += written;
    //    }

    //    return new string(chars[length..]);
    //}

    // QueryStringEnumerable is in the namespace: Microsoft.AspNetCore.WebUtilities
    // https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.webutilities.querystringenumerable
    //public string Best_QueryString_Enumerable()
    //{
    //    var queryMemoryKey = _queryKey.AsMemory();

    //    foreach (EncodedNameValuePair pair in new QueryStringEnumerable(_query))
    //    {
    //        if (pair.EncodedName.Equals(queryMemoryKey)) // Note: Need to figure out why not hit this statement?
    //        {
    //            return pair.DecodeValue().ToString();
    //        }
    //    }

    //    return string.Empty;
    //}

    [Benchmark]
    public string String_Tokenizer_Decode()
    {
        string value = Better_String_Tokenizer_Get();

        return HttpUtility.UrlDecode(value);
    }
}