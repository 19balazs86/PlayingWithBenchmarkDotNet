using System;
using System.Linq;

namespace PlayingWithBenchmarkDotNet.Code
{
  public class NameParser
  {
    public string GetLastName(string fullName)
    {
      string[] names = fullName.Split(" ");

      string lastName = names.LastOrDefault();

      return lastName ?? string.Empty;
    }

    public string GetLastNameUsingSubstring(string fullName)
    {
      int lastSpaceIndex = fullName.LastIndexOf(" ", StringComparison.Ordinal);

      return lastSpaceIndex == -1
        ? string.Empty
        : fullName.Substring(lastSpaceIndex + 1);
    }

    public ReadOnlySpan<char> GetLastNameWithSpan(ReadOnlySpan<char> fullName)
    {
      int lastSpaceIndex = fullName.LastIndexOf(' ');

      return lastSpaceIndex == -1
        ? ReadOnlySpan<char>.Empty
        : fullName.Slice(lastSpaceIndex + 1);
    }
  }
}
