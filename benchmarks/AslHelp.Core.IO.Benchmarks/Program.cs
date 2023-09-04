using AslHelp.Core.IO.Benchmarks;
using AslHelp.Mono.MonoInterop.MonoV1;

#if DEBUG
MonoV1Manager manager = new();
#else
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<ParseNativeStructsBenchmarks>();
#endif
