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
    nuint Scan(nuint startAddress, int size, int offset, params string[] pattern);

    nuint ScanRel(int offset, params string[] pattern);
    nuint ScanRel(int offset, params byte[] pattern);
    nuint ScanRel(string moduleName, int offset, params string[] pattern);
    nuint ScanRel(string moduleName, int offset, params byte[] pattern);
    nuint ScanRel(Module module, int offset, params string[] pattern);
    nuint ScanRel(Module module, int offset, params byte[] pattern);
    nuint ScanRel(nuint startAddress, nuint endAddress, int offset, params string[] pattern);
    nuint ScanRel(nuint startAddress, int size, int offset, params string[] pattern);

    IEnumerable<nint> ScanAll(int offset, params string[] pattern);
    IEnumerable<nint> ScanAll(int offset, params byte[] pattern);
    IEnumerable<nint> ScanAll(string moduleName, int offset, params string[] pattern);
    IEnumerable<nint> ScanAll(string moduleName, int offset, params byte[] pattern);
    IEnumerable<nint> ScanAll(Module module, int offset, params string[] pattern);
    IEnumerable<nint> ScanAll(Module module, int offset, params byte[] pattern);
    IEnumerable<nint> ScanAll(nuint startAddress, nuint endAddress, int offset, params string[] pattern);
    IEnumerable<nint> ScanAll(nuint startAddress, int size, int offset, params string[] pattern);

    IEnumerable<nint> ScanAllRel(int offset, params string[] pattern);
    IEnumerable<nint> ScanAllRel(int offset, params byte[] pattern);
    IEnumerable<nint> ScanAllRel(string moduleName, int offset, params string[] pattern);
    IEnumerable<nint> ScanAllRel(string moduleName, int offset, params byte[] pattern);
    IEnumerable<nint> ScanAllRel(Module module, int offset, params string[] pattern);
    IEnumerable<nint> ScanAllRel(Module module, int offset, params byte[] pattern);
    IEnumerable<nint> ScanAllRel(nuint startAddress, nuint endAddress, int offset, params string[] pattern);
    IEnumerable<nint> ScanAllRel(nuint startAddress, int size, int offset, params string[] pattern);

    nuint ScanPages(int offset, params string[] pattern);
    nuint ScanPages(bool allPages, int offset, params string[] pattern);
    nuint ScanPagesRel(int offset, params string[] pattern);
    nuint ScanPagesRel(bool allPages, int offset, params string[] pattern);
    IEnumerable<nint> ScanPagesAll(int offset, params string[] pattern);
    IEnumerable<nint> ScanPagesAll(bool allPages, int offset, params string[] pattern);
    IEnumerable<nint> ScanPagesAllRel(int offset, params string[] pattern);
    IEnumerable<nint> ScanPagesAllRel(bool allPages, int offset, params string[] pattern);

    nuint Scan(Signature signature, int alignment = 1);
    nuint Scan(Signature signature, int size, int alignment = 1);
    nuint Scan(Signature signature, string moduleName, int alignment = 1);
    nuint Scan(Signature signature, string moduleName, int size, int alignment = 1);
    nuint Scan(Signature signature, Module module, int alignment = 1);
    nuint Scan(Signature signature, Module module, int size, int alignment = 1);
    nuint Scan(Signature signature, nuint startAddress, nuint endAddress, int alignment = 1);
    nuint Scan(Signature signature, nuint startAddress, int size, int alignment = 1);

    nuint ScanRel(Signature signature, int alignment = 1);
    nuint ScanRel(Signature signature, int size, int alignment = 1);
    nuint ScanRel(Signature signature, string moduleName, int alignment = 1);
    nuint ScanRel(Signature signature, string moduleName, int size, int alignment = 1);
    nuint ScanRel(Signature signature, Module module, int alignment = 1);
    nuint ScanRel(Signature signature, Module module, int size, int alignment = 1);
    nuint ScanRel(Signature signature, nuint startAddress, nuint endAddress, int alignment = 1);
    nuint ScanRel(Signature signature, nuint startAddress, int size, int alignment = 1);

    IEnumerable<nint> ScanAll(Signature signature, int alignment = 1);
    IEnumerable<nint> ScanAll(Signature signature, int size, int alignment = 1);
    IEnumerable<nint> ScanAll(Signature signature, string moduleName, int alignment = 1);
    IEnumerable<nint> ScanAll(Signature signature, string moduleName, int size, int alignment = 1);
    IEnumerable<nint> ScanAll(Signature signature, Module module, int alignment = 1);
    IEnumerable<nint> ScanAll(Signature signature, Module module, int size, int alignment = 1);
    IEnumerable<nint> ScanAll(Signature signature, nuint startAddress, nuint endAddress, int alignment = 1);
    IEnumerable<nint> ScanAll(Signature signature, nuint startAddress, int size, int alignment = 1);

    IEnumerable<nint> ScanAllRel(Signature signature, int alignment = 1);
    IEnumerable<nint> ScanAllRel(Signature signature, int size, int alignment = 1);
    IEnumerable<nint> ScanAllRel(Signature signature, string moduleName, int alignment = 1);
    IEnumerable<nint> ScanAllRel(Signature signature, string moduleName, int size, int alignment = 1);
    IEnumerable<nint> ScanAllRel(Signature signature, Module module, int alignment = 1);
    IEnumerable<nint> ScanAllRel(Signature signature, Module module, int size, int alignment = 1);
    IEnumerable<nint> ScanAllRel(Signature signature, nuint startAddress, nuint endAddress, int alignment = 1);
    IEnumerable<nint> ScanAllRel(Signature signature, nuint startAddress, int size, int alignment = 1);

    nuint ScanPages(Signature signature, int alignment = 1);
    nuint ScanPages(bool allPages, Signature signature, int alignment = 1);
    nuint ScanPagesRel(Signature signature, int alignment = 1);
    nuint ScanPagesRel(bool allPages, Signature signature, int alignment = 1);
    IEnumerable<nint> ScanPagesAll(Signature signature, int alignment = 1);
    IEnumerable<nint> ScanPagesAll(bool allPages, Signature signature, int alignment = 1);
    IEnumerable<nint> ScanPagesAllRel(Signature signature, int alignment = 1);
    IEnumerable<nint> ScanPagesAllRel(bool allPages, Signature signature, int alignment = 1);
}

