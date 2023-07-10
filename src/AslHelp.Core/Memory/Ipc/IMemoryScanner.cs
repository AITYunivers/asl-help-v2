using System.Collections.Generic;

using AslHelp.Core.Memory.SignatureScanning;

namespace AslHelp.Core.Memory.Ipc;

public interface IMemoryScanner
{
    nuint Scan(uint offset, params string[] pattern);
    nuint Scan(uint offset, params byte[] pattern);
    nuint Scan(string moduleName, uint offset, params string[] pattern);
    nuint Scan(string moduleName, uint offset, params byte[] pattern);
    nuint Scan(Module module, uint offset, params string[] pattern);
    nuint Scan(Module module, uint offset, params byte[] pattern);
    nuint Scan(nuint startAddress, nuint endAddress, uint offset, params string[] pattern);
    nuint Scan(nuint startAddress, uint size, uint offset, params string[] pattern);

    nuint ScanRel(uint offset, params string[] pattern);
    nuint ScanRel(uint offset, params byte[] pattern);
    nuint ScanRel(string moduleName, uint offset, params string[] pattern);
    nuint ScanRel(string moduleName, uint offset, params byte[] pattern);
    nuint ScanRel(Module module, uint offset, params string[] pattern);
    nuint ScanRel(Module module, uint offset, params byte[] pattern);
    nuint ScanRel(nuint startAddress, nuint endAddress, uint offset, params string[] pattern);
    nuint ScanRel(nuint startAddress, uint size, uint offset, params string[] pattern);

    IEnumerable<nuint> ScanAll(uint offset, params string[] pattern);
    IEnumerable<nuint> ScanAll(uint offset, params byte[] pattern);
    IEnumerable<nuint> ScanAll(string moduleName, uint offset, params string[] pattern);
    IEnumerable<nuint> ScanAll(string moduleName, uint offset, params byte[] pattern);
    IEnumerable<nuint> ScanAll(Module module, uint offset, params string[] pattern);
    IEnumerable<nuint> ScanAll(Module module, uint offset, params byte[] pattern);
    IEnumerable<nuint> ScanAll(nuint startAddress, nuint endAddress, uint offset, params string[] pattern);
    IEnumerable<nuint> ScanAll(nuint startAddress, uint size, uint offset, params string[] pattern);

    IEnumerable<nuint> ScanAllRel(uint offset, params string[] pattern);
    IEnumerable<nuint> ScanAllRel(uint offset, params byte[] pattern);
    IEnumerable<nuint> ScanAllRel(string moduleName, uint offset, params string[] pattern);
    IEnumerable<nuint> ScanAllRel(string moduleName, uint offset, params byte[] pattern);
    IEnumerable<nuint> ScanAllRel(Module module, uint offset, params string[] pattern);
    IEnumerable<nuint> ScanAllRel(Module module, uint offset, params byte[] pattern);
    IEnumerable<nuint> ScanAllRel(nuint startAddress, nuint endAddress, uint offset, params string[] pattern);
    IEnumerable<nuint> ScanAllRel(nuint startAddress, uint size, uint offset, params string[] pattern);

    nuint ScanPages(uint offset, params string[] pattern);
    nuint ScanPages(bool allPages, uint offset, params string[] pattern);
    nuint ScanPagesRel(uint offset, params string[] pattern);
    nuint ScanPagesRel(bool allPages, uint offset, params string[] pattern);
    IEnumerable<nuint> ScanPagesAll(uint offset, params string[] pattern);
    IEnumerable<nuint> ScanPagesAll(bool allPages, uint offset, params string[] pattern);
    IEnumerable<nuint> ScanPagesAllRel(uint offset, params string[] pattern);
    IEnumerable<nuint> ScanPagesAllRel(bool allPages, uint offset, params string[] pattern);

    nuint Scan(Signature signature, uint alignment = 1);
    nuint Scan(uint size, Signature signature, uint alignment = 1);
    nuint Scan(string moduleName, Signature signature, uint alignment = 1);
    nuint Scan(string moduleName, uint size, Signature signature, uint alignment = 1);
    nuint Scan(Module module, Signature signature, uint alignment = 1);
    nuint Scan(Module module, uint size, Signature signature, uint alignment = 1);
    nuint Scan(nuint startAddress, nuint endAddress, Signature signature, uint alignment = 1);
    nuint Scan(nuint startAddress, uint size, Signature signature, uint alignment = 1);

    nuint ScanRel(Signature signature, uint alignment = 1);
    nuint ScanRel(uint size, Signature signature, uint alignment = 1);
    nuint ScanRel(string moduleName, Signature signature, uint alignment = 1);
    nuint ScanRel(string moduleName, uint size, Signature signature, uint alignment = 1);
    nuint ScanRel(Module module, Signature signature, uint alignment = 1);
    nuint ScanRel(Module module, uint size, Signature signature, uint alignment = 1);
    nuint ScanRel(nuint startAddress, nuint endAddress, Signature signature, uint alignment = 1);
    nuint ScanRel(nuint startAddress, uint size, Signature signature, uint alignment = 1);

    IEnumerable<nuint> ScanAll(Signature signature, uint alignment = 1);
    IEnumerable<nuint> ScanAll(uint size, Signature signature, uint alignment = 1);
    IEnumerable<nuint> ScanAll(string moduleName, Signature signature, uint alignment = 1);
    IEnumerable<nuint> ScanAll(string moduleName, uint size, Signature signature, uint alignment = 1);
    IEnumerable<nuint> ScanAll(Module module, Signature signature, uint alignment = 1);
    IEnumerable<nuint> ScanAll(Module module, uint size, Signature signature, uint alignment = 1);
    IEnumerable<nuint> ScanAll(nuint startAddress, nuint endAddress, Signature signature, uint alignment = 1);
    IEnumerable<nuint> ScanAll(nuint startAddress, uint size, Signature signature, uint alignment = 1);

    IEnumerable<nuint> ScanAllRel(Signature signature, uint alignment = 1);
    IEnumerable<nuint> ScanAllRel(uint size, Signature signature, uint alignment = 1);
    IEnumerable<nuint> ScanAllRel(string moduleName, Signature signature, uint alignment = 1);
    IEnumerable<nuint> ScanAllRel(string moduleName, uint size, Signature signature, uint alignment = 1);
    IEnumerable<nuint> ScanAllRel(Module module, Signature signature, uint alignment = 1);
    IEnumerable<nuint> ScanAllRel(Module module, uint size, Signature signature, uint alignment = 1);
    IEnumerable<nuint> ScanAllRel(nuint startAddress, nuint endAddress, Signature signature, uint alignment = 1);
    IEnumerable<nuint> ScanAllRel(nuint startAddress, uint size, Signature signature, uint alignment = 1);

    nuint ScanPages(Signature signature, uint alignment = 1);
    nuint ScanPages(bool allPages, Signature signature, uint alignment = 1);
    nuint ScanPagesRel(Signature signature, uint alignment = 1);
    nuint ScanPagesRel(bool allPages, Signature signature, uint alignment = 1);
    IEnumerable<nuint> ScanPagesAll(Signature signature, uint alignment = 1);
    IEnumerable<nuint> ScanPagesAll(bool allPages, Signature signature, uint alignment = 1);
    IEnumerable<nuint> ScanPagesAllRel(Signature signature, uint alignment = 1);
    IEnumerable<nuint> ScanPagesAllRel(bool allPages, Signature signature, uint alignment = 1);
}

