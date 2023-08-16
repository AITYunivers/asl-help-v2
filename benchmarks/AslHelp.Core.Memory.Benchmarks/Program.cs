using AslHelp.Core.Memory.Benchmarks;

#if DEBUG
using System;

MemoryReadingBenchmarks bm = new();
bm.Setup();

double d1 = bm.ReadDouble_LiveSplit();
Console.WriteLine(d1);

double d2 = bm.ReadDouble_AslHelp();
Console.WriteLine(d2);

bm.Cleanup();
#else
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<MemoryReadingBenchmarks>();
#endif
