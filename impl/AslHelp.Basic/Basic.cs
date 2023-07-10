using System;

using AslHelp.Core.Helpers.Asl;

public partial class Basic : AslHelperBase
{
    protected override void OnExitImpl()
    {
        for (int i = 0; i < _fileWatchers.Count; i++)
        {
            _fileWatchers[i].Dispose();
        }

        _fileWatchers.Clear();
    }

    protected override void OnShutdownImpl(bool closing)
    {
        AppDomain.CurrentDomain.AssemblyResolve -= AssemblyResolve;

        _logger.Stop();
        _logger.Clear();

        for (int i = 0; i < _fileWatchers.Count; i++)
        {
            _fileWatchers[i].Dispose();
        }

        _fileWatchers.Clear();

        if (!closing)
        {
            Texts.RemoveAll();
        }
    }
}
