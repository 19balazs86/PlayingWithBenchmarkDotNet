# Playing with BenchmarkDotNet
This repository is a sandbox to try out the [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet). You can find useful resources, tips and tricks to write high-performance code.

#### Resources
- [Overview](https://benchmarkdotnet.org/articles/overview.html) 📓*Official, Guides, Features, Configs, Samples*.
- Steve Gordon
  - [Writing High-Performance code](https://youtu.be/2SXr48OYxbA) 📽️1 hour| [Blog post series](https://www.stevejgordon.co.uk/writing-high-performance-csharp-and-dotnet-code)
  - [An Introduction to optimising code using Span](https://www.stevejgordon.co.uk/an-introduction-to-optimising-code-using-span-t) 📓
- [Compile-time logging source generation](https://learn.microsoft.com/en-us/dotnet/core/extensions/logger-message-generator) 📚
  - [Source generator for logging](https://dev.to/dgenezini/dont-box-your-logs-21a1) 📓*DanielGenezini*
  - [High-Performance Logging](https://www.stevejgordon.co.uk/high-performance-logging-in-net-core) 📓Steve Gordon
- [Improve memory allocation when creating HttpContent](https://hashnode.devindran.com/how-to-improve-memory-allocation-when-creating-httpcontent) 📓*Devindran Ramadass - Using the CommunityToolkit.HighPerformance package*
- "Michael's coding spot" blog posts
  - [Best practices to find, fix, and avoid performance problems](https://michaelscodingspot.com/performance-problems-in-csharp-dotnet/)
  - [Techniques to avoid GC pressure and improve performance](https://michaelscodingspot.com/avoid-gc-pressure/)
  - [Best practices to find, fix, and avoid memory leaks](https://michaelscodingspot.com/find-fix-and-avoid-memory-leaks-in-c-net-8-best-practices/)
  - [Ways you can cause memory leaks](https://michaelscodingspot.com/ways-to-cause-memory-leaks-in-dotnet/)
- [StringBuilder performance pitfalls](https://www.meziantou.net/stringbuilder-performance-pitfalls.htm) 📓*Meziantou blog*
- Memory, Span, Pipelines
  - [Memory and Span usage guidelines](https://docs.microsoft.com/en-us/dotnet/standard/memory-and-spans/memory-t-usage-guidelines) 📚
  - [System.IO.Pipelines: High performance IO in .NET](https://devblogs.microsoft.com/dotnet/system-io-pipelines-high-performance-io-in-net) 📓 *DavidFowler, MS Devblogs*
  - [Exploring Spans and Pipelines](https://github.com/timiskhakov/ExploringSpansAndPipelines) 👤*TimIskhakov*