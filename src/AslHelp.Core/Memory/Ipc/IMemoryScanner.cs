using System.Collections.Generic;

using AslHelp.Core.Memory.SignatureScanning;

namespace AslHelp.Core.Memory.Ipc;

public interface IMemoryScanner
{
    nuint Scan(int offset, params string[] pattern);
    nuint Scan(int offset, params byte[] pattern);
    nuint Scan(string moduleName, int offset, params string[] pattern);
    nuint Scan(string moduleName, int offset, params byte[] pattern);
    nuint Scan(Module module, int offset, params string[] pattern);
    nuint Scan(Module module, int offset, params byte[] pattern);
    nuint Scan(nuint startAddress, nuint endAddress, int offset, params string[] pattern);
    nuint Scan(nuint startAddress, uint size, int offset, params string[] pattern);

    nuint ScanRel(int offset, params string[] pattern);
    nuint ScanRel(int offset, params byte[] pattern);
    nuint ScanRel(string moduleName, int offset, params string[] pattern);
    nuint ScanRel(string moduleName, int offset, params byte[] pattern);
    nuint ScanRel(Module module, int offset, params string[] pattern);
    nuint ScanRel(Module module, int offset, params byte[] pattern);
    nuint ScanRel(nuint startAddress, nuint endAddress, int offset, params string[] pattern);
    nuint ScanRel(nuint startAddress, uint size, int offset, params string[] pattern);

    IEnumerable<nuint> ScanAll(int offset, params string[] pattern);
    IEnumerable<nuint> ScanAll(int offset, params byte[] pattern);
    IEnumerable<nuint> ScanAll(string moduleName, int offset, params string[] pattern);
    IEnumerable<nuint> ScanAll(string moduleName, int offset, params byte[] pattern);
    IEnumerable<nuint> ScanAll(Module module, int offset, params string[] pattern);
    IEnumerable<nuint> ScanAll(Module module, int offset, params byte[] pattern);
    IEnumerable<nuint> ScanAll(nuint startAddress, nuint endAddress, int offset, params string[] pattern);
    IEnumerable<nuint> ScanAll(nuint startAddress, uint size, int offset, params string[] pattern);

    IEnumerable<nuint> ScanAllRel(int offset, params string[] pattern);
    IEnumerable<nuint> ScanAllRel(int offset, params byte[] pattern);
    IEnumerable<nuint> ScanAllRel(string moduleName, int offset, params string[] pattern);
    IEnumerable<nuint> ScanAllRel(string moduleName, int offset, params byte[] pattern);
    IEnumerable<nuint> ScanAllRel(Module module, int offset, params string[] pattern);
    IEnumerable<nuint> ScanAllRel(Module module, int offset, params byte[] pattern);
    IEnumerable<nuint> ScanAllRel(nuint startAddress, nuint endAddress, int offset, params string[] pattern);
    IEnumerable<nuint> ScanAllRel(nuint startAddress, uint size, int offset, params string[] pattern);

    nuint ScanPages(int offset, params string[] pattern);
    nuint ScanPages(bool allPages, int offset, params string[] pattern);
    nuint ScanPagesRel(int offset, params string[] pattern);
    nuint ScanPagesRel(bool allPages, int offset, params string[] pattern);
    IEnumerable<nuint> ScanPagesAll(int offset, params string[] pattern);
    IEnumerable<nuint> ScanPagesAll(bool allPages, int offset, params string[] pattern);
    IEnumerable<nuint> ScanPagesAllRel(int offset, params string[] pattern);
    IEnumerable<nuint> ScanPagesAllRel(bool allPages, int offset, params string[] pattern);

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

