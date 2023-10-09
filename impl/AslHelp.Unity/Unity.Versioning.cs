using System;
using System.Buffers.Binary;
using System.IO;

using AslHelp.Common.Exceptions;
using AslHelp.Common.Extensions;
using AslHelp.Unity.Memory.MonoInterop;

public partial class Unity
{
    private Version? _unityVersion;
    public Version? UnityVersion
    {
        get
        {
            if (_unityVersion is not null)
            {
                return _unityVersion;
            }

            if (Memory is null)
            {
                return null;
            }

            string? versionString = null;

            string dataDirectory = $"{Memory.MainModule.FileName[..^4]}_Data";

            string ggmPath = Path.Combine(dataDirectory, "globalgamemanagers");
            string mdPath = Path.Combine(dataDirectory, "mainData");
            string du3dPath = Path.Combine(dataDirectory, "data.unity3d");

            bool ggmExists = File.Exists(ggmPath), mdExists = File.Exists(mdPath), du3dExists = File.Exists(du3dPath);

            if (ggmExists || mdExists)
            {
                using FileStream stream = File.OpenRead(ggmExists ? ggmPath : mdPath);

                Span<byte> buffer = stackalloc byte[sizeof(uint)];

                stream.ReadExactly(buffer);
                uint metaDataSize = BinaryPrimitives.ReadUInt32BigEndian(buffer);

                stream.ReadExactly(buffer);
                uint fileSize = BinaryPrimitives.ReadUInt32BigEndian(buffer);

                stream.ReadExactly(buffer);
                uint fileVersion = BinaryPrimitives.ReadUInt32BigEndian(buffer);

                stream.Position += 4;

                if (fileVersion >= 9)
                {
                    stream.Position += 4;
                }
                else
                {
                    stream.Position = fileSize - metaDataSize + 1;
                }

                if (fileVersion >= 22)
                {
                    stream.Position += 28;
                }

                if (fileVersion >= 7)
                {
                    using BinaryReader reader = new(stream);
                    versionString = reader.ReadNullTerminatedString();
                }
            }
            else if (du3dExists)
            {
                using FileStream stream = File.OpenRead(du3dPath);
                using BinaryReader reader = new(stream);

                reader.ReadNullTerminatedString();
                stream.Position += 4;
                reader.ReadNullTerminatedString();

                versionString = reader.ReadNullTerminatedString();
            }

            if (versionString is null)
            {
                const string msg = "Failed to determine Unity version.";
                ThrowHelper.ThrowInvalidOperationException(msg);
            }

            if (Version.TryParse(versionString.Replace('f', '.'), out Version? version))
            {
                _unityVersion = version;
            }

            return _unityVersion;
        }
    }

    private Il2CppGlobalMetadataHeader? _il2CppMetadata;
    public Il2CppGlobalMetadataHeader? Il2CppMetadata
    {
        get
        {
            if (_il2CppMetadata is not null)
            {
                return _il2CppMetadata;
            }

            if (Memory is null)
            {
                return null;
            }

            string dataDirectory = $"{Memory.MainModule.FileName[..^4]}_Data";
            string globalMetadataPath = Path.Combine(dataDirectory, "il2cpp_data", "Metadata", "global-metadata.dat");

            if (!File.Exists(globalMetadataPath))
            {
                const string msg = "'global-metadata.dat' for IL2CPP game not found.";
                ThrowHelper.ThrowFileNotFoundException(msg);
            }

            using FileStream stream = File.OpenRead(globalMetadataPath);

            var metadata = stream.Read<Il2CppGlobalMetadataHeader>();
            if ((uint)metadata.Sanity != 0xFAB11BAF)
            {
                const string msg = "'global-metadata.dat' is not a valid IL2CPP metadata file.";
                ThrowHelper.ThrowInvalidOperationException(msg);
            }

            _il2CppMetadata = metadata;
            return metadata;
        }
    }
}
