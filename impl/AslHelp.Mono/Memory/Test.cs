// using System.Collections.Generic;

// using AslHelp.Common.Exceptions;
// using AslHelp.Core.Extensions;
// using AslHelp.Core.Memory.SignatureScanning;
// using AslHelp.Mono.Memory.Ipc;

// namespace AslHelp.Mono.Memory;

// // just goofing around with some ideas

// internal interface IEngineManager
// {
//     IModuleManager Modules { get; }
//     ITypeManager Types { get; }
//     IFieldManager Fields { get; }
// }

// internal interface IModuleManager
// {
//     IEnumerable<nuint> Get();

//     string GetModuleName(nuint module);
//     string GetModulePath(nuint module);
// }

// internal interface IModule
// {
//     nuint Address { get; }
//     string Name { get; }
//     string FileName { get; }
// }

// internal interface ITypeManager
// {
//     IEnumerable<nuint> Get(nuint module);

//     string GetTypeName(nuint type);
//     string GetTypeNamespace(nuint type);
// }

// internal interface IType
// {
//     nuint Address { get; }
//     string Name { get; }
//     string Namespace { get; }
// }

// internal interface IFieldManager
// {
//     IEnumerable<nuint> Get(nuint type);

//     string GetFieldName(nuint field);
//     int GetFieldOffset(nuint field);
// }

// internal interface IField
// {
//     nuint Address { get; }
//     string Name { get; }
//     int Offset { get; }
// }

// internal interface IMonoModuleManager : IModuleManager
// {
//     nuint FindLoadedAssemblies();
// }

// internal class MonoV1Manager : IEngineManager
// {
//     public MonoV1Manager(IMonoMemoryManager memory)
//     {
//         Modules = new MonoV1Images(memory);
//     }

//     public IModuleManager Modules { get; }

//     public ITypeManager Types => throw new System.NotImplementedException();

//     public IFieldManager Fields => throw new System.NotImplementedException();
// }

// // impl needs some external information (loaded_assemblies) that i'm not sure how to best pass
// internal class MonoV1Images : IMonoModuleManager
// {
//     private readonly IMonoMemoryManager _memory;

//     public MonoV1Images(IMonoMemoryManager memory)
//     {
//         _memory = memory;
//     }

//     public nuint FindLoadedAssemblies()
//     {
//         if (!_memory.MonoModule.Symbols.TryGetValue("mono_assembly_foreach", out var monoAssemblyForeach)
//             || monoAssemblyForeach.Address == 0)
//         {
//             const string msg = "Unable to find symbol 'mono_assembly_foreach'.";
//             ThrowHelper.ThrowInvalidOperationException(msg);
//         }

//         bool is64Bit = _memory.Is64Bit;
//         Signature[] signatures = is64Bit
//             ? [new(3, "48 8B 0D")]
//             : [new(2, "FF 35"), new(2, "8B 0D")];

//         nuint loadedAssemblies = _memory.Scan(signatures, monoAssemblyForeach.Address, 0x100);
//         if (loadedAssemblies == 0)
//         {
//             const string msg = "Failed scanning for a reference to 'loaded_assemblies'.";
//             ThrowHelper.ThrowInvalidOperationException(msg);
//         }

//         return _memory.Read<nuint>(_memory.ReadRelative(loadedAssemblies));
//     }

//     public IEnumerable<nuint> Get()
//     {
//         nuint assemblies = FindLoadedAssemblies();

//         while (assemblies != 0)
//         {
//             nuint assembly = _memory.Read<nuint>(assemblies + _structs["GList"]["data"]);
//             nuint image = _memory.Read<nuint>(assembly + _structs["MonoAssembly"]["image"]);

//             yield return image;

//             assemblies = _memory.Read<nuint>(assemblies + _structs["GList"]["next"]);
//         }
//     }

//     public string GetModuleName(nuint module)
//     {
//         throw new System.NotImplementedException();
//     }

//     public string GetModulePath(nuint module)
//     {
//         throw new System.NotImplementedException();
//     }
// }
