﻿using BenchmarkDotNet.Attributes;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PlayingWithBenchmarkDotNet.Benchmark;

// https://khalidabuhakmeh.com/speed-up-aspnet-core-json-apis-with-source-generators

/*
|                          Method |       Mean | Allocated |
|-------------------------------- |-----------:|----------:|
|                Serialize_Normal |   575.3 ns |     216 B |
|   Serialize_WithSourceGenerator |   389.1 ns |     216 B |
|              Deserialize_Normal | 1,154.6 ns |     352 B |
| Deserialize_WithSourceGenerator |   928.9 ns |     200 B |
*/

[ShortRunJob]
// [RankColumn]
// [Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser]
[HideColumns("Error", "StdDev", "Gen0", "RatioSD")]
public class JSON_SourceGenerator_Benchmarks
{
    private static readonly Person _person = new Person(1, "Person Name", 18, "Person's Address", null);

    private static readonly JsonSerializerOptions _jsonSerializerOptions = new () { WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    private readonly string _serializedPerson = JsonSerializer.Serialize(_person);

    [Benchmark]
    public string Serialize_Normal()
    {
        return JsonSerializer.Serialize(_person, _jsonSerializerOptions);
    }

    [Benchmark]
    public string Serialize_WithSourceGenerator()
    {
        return JsonSerializer.Serialize(_person, PersonSerializationContext.Default.Person);
    }

    [Benchmark]
    public Person Deserialize_Normal()
    {
        return JsonSerializer.Deserialize<Person>(_serializedPerson);
    }

    [Benchmark]
    public Person Deserialize_WithSourceGenerator()
    {
        return JsonSerializer.Deserialize(_serializedPerson, PersonSerializationContext.Default.Person);
    }
}

public record Person(int Id, string Name, int Age, string Address, string NullString);

[JsonSourceGenerationOptions(
    WriteIndented = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(Person))]
public sealed partial class PersonSerializationContext : JsonSerializerContext;
