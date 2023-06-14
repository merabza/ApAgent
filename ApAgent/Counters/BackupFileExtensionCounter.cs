using System;
using DbTools;

namespace ApAgent.Counters;

public sealed class BackupFileExtensionCounter
{
    private readonly EBackupType _backupType;

    public BackupFileExtensionCounter(EBackupType backupType)
    {
        _backupType = backupType;
    }

    public string Count()
    {
        return _backupType switch
        {
            EBackupType.Full => "bak",
            EBackupType.Diff => "dif",
            EBackupType.TrLog => "trn",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}