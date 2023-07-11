﻿using BenchmarkDotNet.Attributes;
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

    private static readonly Uri _url = new Uri("https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.webutilities.querystringenumerable?view=aspnetcore-7.0&key1=value1&key2=value2");

    [Benchmark]
    public string Old_HttpUtility_ParseQueryString()
    {
        NameValueCollection query = HttpUtility.ParseQueryString(_url.Query);

        return query.Get(_queryKey1);
    }

    // StringTokenizer: https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.primitives.stringtokenizer
    [Benchmark]
    public string Better_String_Tokenizer()
    {
        var segments = new StringTokenizer(_url.Query, _separatorsQuery);

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

    // QueryStringEnumerable is in the namespace: Microsoft.AspNetCore.WebUtilities
    // https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.webutilities.querystringenumerable
    //public string Best_QueryString_Enumerable()
    //{
    //    var queryMemoryKey = _queryKey.AsMemory();

    //    foreach (EncodedNameValuePair pair in new QueryStringEnumerable(_url.Query))
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
        string value = Better_String_Tokenizer();

        return HttpUtility.UrlDecode(value);
    }
}