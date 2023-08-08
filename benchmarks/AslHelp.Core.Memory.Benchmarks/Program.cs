
using System;

using AslHelp.Core.Memory.Benchmarks;

#if DEBUG
MemoryReadingBenchmarks bm = new();
bm.Setup();

double d1 = bm.ReadDouble_External();
Console.WriteLine(d1);

double d2 = bm.ReadDouble_Pipe();
Console.WriteLine(d2);

bm.Cleanup();
#else
MemoryReadingBenchmarks bm = new();
bm.Setup();

double d1 = bm.ReadDouble_External();
double d2 = bm.ReadDouble_Pipe();

Console.WriteLine(d1);
Console.WriteLine(d2);

bm.Cleanup();
#endif
