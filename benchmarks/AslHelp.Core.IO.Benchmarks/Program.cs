using AslHelp.Core.IO.Benchmarks;

#if DEBUG
ParseNativeStructsBenchmarks benchmarks = new();
benchmarks.ParseNativeStructs();
#else
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<ParseNativeStructsBenchmarks>();
#endif
