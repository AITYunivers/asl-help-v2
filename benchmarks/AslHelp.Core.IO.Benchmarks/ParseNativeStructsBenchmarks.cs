using AslHelp.Core.IO.Parsing;

using BenchmarkDotNet.Attributes;

namespace AslHelp.Core.IO.Benchmarks;

[MemoryDiagnoser(false)]
public class ParseNativeStructsBenchmarks
{
    private NativeStructMap? _result;

    [Benchmark]
    public void ParseNativeStructs()
    {
        _result = NativeStructMap.Parse("Mono", "mono", "v1", true, typeof(global::Mono).Assembly);
        System.Console.WriteLine(_result);
    }
}
