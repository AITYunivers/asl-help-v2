namespace AslHelp.Core.Memory.IO;

public interface IMemoryScanner
{
    nint Scan(int offset, params string[] pattern);
    nint Scan(int offset, params byte[] pattern);
    nint Scan(string moduleName, int offset, params string[] pattern);
    nint Scan(string moduleName, int offset, params byte[] pattern);
    nint Scan(Module module, int offset, params string[] pattern);
    nint Scan(Module module, int offset, params byte[] pattern);
    nint Scan(nint startAddress, nint endAddress, int offset, params string[] pattern);
    nint Scan(nint startAddress, int size, int offset, params string[] pattern);

    nint ScanRel(int offset, params string[] pattern);
    nint ScanRel(int offset, params byte[] pattern);
    nint ScanRel(string moduleName, int offset, params string[] pattern);
    nint ScanRel(string moduleName, int offset, params byte[] pattern);
    nint ScanRel(Module module, int offset, params string[] pattern);
    nint ScanRel(Module module, int offset, params byte[] pattern);
    nint ScanRel(nint startAddress, nint endAddress, int offset, params string[] pattern);
    nint ScanRel(nint startAddress, int size, int offset, params string[] pattern);

    IEnumerable<nint> ScanAll(int offset, params string[] pattern);
    IEnumerable<nint> ScanAll(int offset, params byte[] pattern);
    IEnumerable<nint> ScanAll(string moduleName, int offset, params string[] pattern);
    IEnumerable<nint> ScanAll(string moduleName, int offset, params byte[] pattern);
    IEnumerable<nint> ScanAll(Module module, int offset, params string[] pattern);
    IEnumerable<nint> ScanAll(Module module, int offset, params byte[] pattern);
    IEnumerable<nint> ScanAll(nint startAddress, nint endAddress, int offset, params string[] pattern);
    IEnumerable<nint> ScanAll(nint startAddress, int size, int offset, params string[] pattern);

    IEnumerable<nint> ScanAllRel(int offset, params string[] pattern);
    IEnumerable<nint> ScanAllRel(int offset, params byte[] pattern);
    IEnumerable<nint> ScanAllRel(string moduleName, int offset, params string[] pattern);
    IEnumerable<nint> ScanAllRel(string moduleName, int offset, params byte[] pattern);
    IEnumerable<nint> ScanAllRel(Module module, int offset, params string[] pattern);
    IEnumerable<nint> ScanAllRel(Module module, int offset, params byte[] pattern);
    IEnumerable<nint> ScanAllRel(nint startAddress, nint endAddress, int offset, params string[] pattern);
    IEnumerable<nint> ScanAllRel(nint startAddress, int size, int offset, params string[] pattern);

    nint ScanPages(int offset, params string[] pattern);
    nint ScanPages(bool allPages, int offset, params string[] pattern);
    nint ScanPagesRel(int offset, params string[] pattern);
    nint ScanPagesRel(bool allPages, int offset, params string[] pattern);
    IEnumerable<nint> ScanPagesAll(int offset, params string[] pattern);
    IEnumerable<nint> ScanPagesAll(bool allPages, int offset, params string[] pattern);
    IEnumerable<nint> ScanPagesAllRel(int offset, params string[] pattern);
    IEnumerable<nint> ScanPagesAllRel(bool allPages, int offset, params string[] pattern);

    nint Scan(Signature signature, int alignment = 1);
    nint Scan(Signature signature, int size, int alignment = 1);
    nint Scan(Signature signature, string moduleName, int alignment = 1);
    nint Scan(Signature signature, string moduleName, int size, int alignment = 1);
    nint Scan(Signature signature, Module module, int alignment = 1);
    nint Scan(Signature signature, Module module, int size, int alignment = 1);
    nint Scan(Signature signature, nint startAddress, nint endAddress, int alignment = 1);
    nint Scan(Signature signature, nint startAddress, int size, int alignment = 1);

    nint ScanRel(Signature signature, int alignment = 1);
    nint ScanRel(Signature signature, int size, int alignment = 1);
    nint ScanRel(Signature signature, string moduleName, int alignment = 1);
    nint ScanRel(Signature signature, string moduleName, int size, int alignment = 1);
    nint ScanRel(Signature signature, Module module, int alignment = 1);
    nint ScanRel(Signature signature, Module module, int size, int alignment = 1);
    nint ScanRel(Signature signature, nint startAddress, nint endAddress, int alignment = 1);
    nint ScanRel(Signature signature, nint startAddress, int size, int alignment = 1);

    IEnumerable<nint> ScanAll(Signature signature, int alignment = 1);
    IEnumerable<nint> ScanAll(Signature signature, int size, int alignment = 1);
    IEnumerable<nint> ScanAll(Signature signature, string moduleName, int alignment = 1);
    IEnumerable<nint> ScanAll(Signature signature, string moduleName, int size, int alignment = 1);
    IEnumerable<nint> ScanAll(Signature signature, Module module, int alignment = 1);
    IEnumerable<nint> ScanAll(Signature signature, Module module, int size, int alignment = 1);
    IEnumerable<nint> ScanAll(Signature signature, nint startAddress, nint endAddress, int alignment = 1);
    IEnumerable<nint> ScanAll(Signature signature, nint startAddress, int size, int alignment = 1);

    IEnumerable<nint> ScanAllRel(Signature signature, int alignment = 1);
    IEnumerable<nint> ScanAllRel(Signature signature, int size, int alignment = 1);
    IEnumerable<nint> ScanAllRel(Signature signature, string moduleName, int alignment = 1);
    IEnumerable<nint> ScanAllRel(Signature signature, string moduleName, int size, int alignment = 1);
    IEnumerable<nint> ScanAllRel(Signature signature, Module module, int alignment = 1);
    IEnumerable<nint> ScanAllRel(Signature signature, Module module, int size, int alignment = 1);
    IEnumerable<nint> ScanAllRel(Signature signature, nint startAddress, nint endAddress, int alignment = 1);
    IEnumerable<nint> ScanAllRel(Signature signature, nint startAddress, int size, int alignment = 1);

    nint ScanPages(Signature signature, int alignment = 1);
    nint ScanPages(bool allPages, Signature signature, int alignment = 1);
    nint ScanPagesRel(Signature signature, int alignment = 1);
    nint ScanPagesRel(bool allPages, Signature signature, int alignment = 1);
    IEnumerable<nint> ScanPagesAll(Signature signature, int alignment = 1);
    IEnumerable<nint> ScanPagesAll(bool allPages, Signature signature, int alignment = 1);
    IEnumerable<nint> ScanPagesAllRel(Signature signature, int alignment = 1);
    IEnumerable<nint> ScanPagesAllRel(bool allPages, Signature signature, int alignment = 1);
}
