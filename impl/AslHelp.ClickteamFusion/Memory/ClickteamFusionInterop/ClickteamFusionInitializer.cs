using AslHelp.Core.Memory.SignatureScanning;
using AslHelp.ClickteamFusion.Memory.Ipc;
using System.Text;
using System.Collections.Generic;
using System;

namespace AslHelp.ClickteamFusion.Memory.ClickteamFusionInterop;

internal class ClickteamFusionInitializer
{
    public nuint InitializeCCN(IClickteamFusionMemoryManager memory)
    {
        Debug.Info("Initialize CCN Called");
        string headerBytes = "";
        foreach (byte b in Encoding.ASCII.GetBytes("PAMU"))
        {
            headerBytes += b.ToString("X2") + " ";
        }
        
        nuint header = 0;

        Debug.Info("Scanning for Headers");
        IEnumerable<nuint> results = memory.ScanPagesAll(true, 0, headerBytes.Trim());
        foreach (nuint item in results)
        {
            int runtimeVersion = memory.Read<int>(item + 4);
            if (runtimeVersion == 770)
            {
                Debug.Info("Found Header at " + item);
                header = item;
                break;
            }
        }
        
        Debug.Info("Finished Header Scan");

        if (header == 0)
        {
            return 0;
        }

        headerBytes = "";
        foreach (byte b in BitConverter.GetBytes((int)header))
        {
            headerBytes += b.ToString("X2") + " ";
        }

        Debug.Info("Scanning for PAMU with " + headerBytes);
        Signature headerSignature = new(headerBytes);
        IEnumerable<nuint> headerResults = memory.ScanAll(headerSignature);
        foreach (nuint result in headerResults)
        {
            if (!memory.TryReadString(out var output, 4, memory.Read<nuint>(result)) || output != "PAMU")
            {
                continue;
            }

            Debug.Info("Found PAMU at " + result);
            return result;
        }

        return 0;
    }
}
