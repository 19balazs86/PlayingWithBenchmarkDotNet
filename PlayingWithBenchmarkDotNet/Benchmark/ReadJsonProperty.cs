using BenchmarkDotNet.Attributes;

namespace PlayingWithBenchmarkDotNet.Benchmark
{
  [MemoryDiagnoser]
  public class ReadJsonProperty
  {
    private const string _jsonText = "{\"Glossary\":{\"Title\":\"example glossary\",\"GlossDiv\":{\"Title\":\"S\",\"GlossList\":{\"GlossEntry\":{\"ID\":\"SGML\",\"SortAs\":\"SGML\",\"GlossTerm\":\"Standard Generalized Markup Language\",\"Acronym\":\"SGML\",\"Abbrev\":\"ISO 8879:1986\",\"GlossDef\":{\"para\":\"A meta-markup language, used to create markup languages such as DocBook.\",\"GlossSeeAlso\":[\"GML\",\"XML\"]}}}}}}";

    [Benchmark]
    public string WithNewtonsoftJson()
    {
      dynamic o = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(_jsonText);

      return o.Glossary.GlossDiv.Title;
    }

    [Benchmark]
    public string WithSystemTextJson()
    {
      System.Text.Json.JsonDocument jsonDocument = System.Text.Json.JsonDocument.Parse(_jsonText);

      return jsonDocument.RootElement.GetProperty("Glossary").GetProperty("GlossDiv").GetProperty("Title").GetRawText();
    }
  }
}
