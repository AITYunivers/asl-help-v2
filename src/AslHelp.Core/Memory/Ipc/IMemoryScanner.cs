using System.Collections.Generic;

using AslHelp.Core.Memory.SignatureScanning;

namespace AslHelp.Core.Memory.Ipc;

public interface IMemoryScanner
{
    nuint Scan(int offset, params string[] pattern);
    nuint Scan(string moduleName, int offset, params string[] pattern);
    nuint Scan(Module module, int offset, params string[] pattern);
    nuint Scan(nuint startAddress, nuint endAddress, int offset, params string[] pattern);
    nuint Scan(nuint startAddress, uint size, int offset, params string[] pattern);
    uint Scan(byte[] buffer, int offset, params string[] pattern);

    IEnumerable<nuint> ScanAll(int offset, params string[] pattern);
    IEnumerable<nuint> ScanAll(string moduleName, int offset, params string[] pattern);
    IEnumerable<nuint> ScanAll(Module module, int offset, params string[] pattern);
    IEnumerable<nuint> ScanAll(nuint startAddress, nuint endAddress, int offset, params string[] pattern);
    IEnumerable<nuint> ScanAll(nuint startAddress, uint size, int offset, params string[] pattern);
    IEnumerable<uint> ScanAll(byte[] buffer, int offset, params string[] pattern);

    nuint ScanPages(int offset, params string[] pattern);
    nuint ScanPages(bool allPages, int offset, params string[] pattern);
    IEnumerable<nuint> ScanPagesAll(int offset, params string[] pattern);
    IEnumerable<nuint> ScanPagesAll(bool allPages, int offset, params string[] pattern);

    nuint Scan(Signature signature, uint alignment = 1);
    nuint Scan(Signature signature, uint size, uint alignment = 1);
    nuint Scan(Signature signature, string moduleName, uint alignment = 1);
    nuint Scan(Signature signature, string moduleName, uint size, uint alignment = 1);
    nuint Scan(Signature signature, Module module, uint alignment = 1);
    nuint Scan(Signature signature, Module module, uint size, uint alignment = 1);
    nuint Scan(Signature signature, nuint startAddress, nuint endAddress, uint alignment = 1);
    uint Scan(Signature signature, byte[] buffer, uint alignment = 1);

    IEnumerable<nuint> ScanAll(Signature signature, uint alignment = 1);
    IEnumerable<nuint> ScanAll(Signature signature, uint size, uint alignment = 1);
    IEnumerable<nuint> ScanAll(Signature signature, string moduleName, uint alignment = 1);
    IEnumerable<nuint> ScanAll(Signature signature, string moduleName, uint size, uint alignment = 1);
    IEnumerable<nuint> ScanAll(Signature signature, Module module, uint alignment = 1);
    IEnumerable<nuint> ScanAll(Signature signature, Module module, uint size, uint alignment = 1);
    IEnumerable<nuint> ScanAll(Signature signature, nuint startAddress, nuint endAddress, uint alignment = 1);
    IEnumerable<nuint> ScanAll(Signature signature, nuint startAddress, uint size, uint alignment = 1);
    IEnumerable<uint> ScanAll(Signature signature, byte[] buffer, uint alignment = 1);
}
