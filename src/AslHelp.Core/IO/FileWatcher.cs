using System;
using System.IO;

namespace AslHelp.Core.IO;

public sealed class FileWatcher : IDisposable
{
    private readonly string _filePath;

    private bool _isConnected;
    private StreamReader _reader;

    public FileWatcher(string filePath)
    {
        _filePath = filePath;
    }

    private string _line;
    public string Line
    {
        get
        {
            if (!_isConnected && !TryConnect())
            {
                return null;
            }

            if (_reader.ReadLine() is string line)
            {
                _line = line;
            }

            return _line;
        }
    }

    private bool TryConnect()
    {
        if (_isConnected)
        {
            return true;
        }

        if (File.Exists(_filePath))
        {
            FileStream stream = File.Open(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            _reader = new(stream);

            while (_reader.ReadLine() is string line)
            {
                _line = line;
            }

            _isConnected = true;

            return true;
        }

        return false;
    }

    public void Dispose()
    {
        _reader?.Dispose();
        _reader = null;
    }
}
