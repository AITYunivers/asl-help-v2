using System.Diagnostics;
using System.Linq;

using AslHelp.Core.Memory.Ipc;

using BenchmarkDotNet.Attributes;

using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.Benchmarks;

[MemoryDiagnoser(false)]
public class MemoryReadingBenchmarks
{
#nullable disable
    private Process _process;
    private ExternalMemoryManager _memory;
#nullable restore

    [GlobalSetup]
    public void Setup()
    {
        _process = Process.GetProcessesByName("Post Void").Single();
        _memory = new(_process);
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _memory.Dispose();
        _process.Dispose();
    }

    [Benchmark(Baseline = true)]
    public double ReadDouble_LiveSplit()
    {
        var dp = new DeepPointer(0x813390, 0x48, 0x10, 0x690, 0x30);
        return dp.Deref<double>(_process);
    }

    [Benchmark]
    public double ReadDouble_AslHelp()
    {
        return _memory.Read<double>(0x813390, 0x48, 0x10, 0x690, 0x30);
    }
}
