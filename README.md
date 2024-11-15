# Playing with BenchmarkDotNet

- This repository serves as a sandbox for experimenting with BenchmarkDotNet
- Useful resources, tips, and tricks for writing high-performance code

## Resources

- [Official](https://benchmarkdotnet.org/articles/overview.html) 📓*Guides, Features, Configs, Samples* | [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet) 👤*dotnet*
- [Performance Improvements in .NET 9](https://youtu.be/aLQpnpSxosg) 📽️*.NET Conf 2024 - (Regex.EnumerateMatches and EnumerateSplits, AlternateLookup, SearchValues)*
- [Writing High-Performance code](https://youtu.be/2SXr48OYxbA) 📽️*1 hour - Steve Gordon* | [Blog post series](https://www.stevejgordon.co.uk/writing-high-performance-csharp-and-dotnet-code)
- [Compile-time logging source generation](https://learn.microsoft.com/en-us/dotnet/core/extensions/logger-message-generator) 📚
  - [Source generator for logging](https://dev.to/dgenezini/dont-box-your-logs-21a1) 📓*DanielGenezini*
  - [High-Performance Logging](https://www.stevejgordon.co.uk/high-performance-logging-in-net-core) 📓Steve Gordon
- [Improve memory allocation when creating HttpContent](https://hashnode.devindran.com/how-to-improve-memory-allocation-when-creating-httpcontent) 📓*Devindran Ramadass - Using the CommunityToolkit.HighPerformance package*
- "Michael's coding spot" blog posts
  - [Best practices to find, fix, and avoid performance problems](https://michaelscodingspot.com/performance-problems-in-csharp-dotnet)
  - [Best practices to find, fix, and avoid memory leaks](https://michaelscodingspot.com/find-fix-and-avoid-memory-leaks-in-c-net-8-best-practices)
- [StringBuilder performance pitfalls](https://www.meziantou.net/stringbuilder-performance-pitfalls.htm) 📓*Meziantou blog*
- Memory, Span, Pipelines
  - [Memory and Span usage guidelines](https://docs.microsoft.com/en-us/dotnet/standard/memory-and-spans/memory-t-usage-guidelines) 📚
  - [System.IO.Pipelines: High performance IO in .NET](https://devblogs.microsoft.com/dotnet/system-io-pipelines-high-performance-io-in-net) 📓 *DavidFowler, MS Devblogs*