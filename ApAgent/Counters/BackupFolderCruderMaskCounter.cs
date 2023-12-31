﻿using ApAgent.Cruders;

namespace ApAgent.Counters;

public sealed class BackupFolderCruderMaskCounter : MaskCounter
{
    private readonly FileBackupFolderCruder _fileBackupFolderCruder;

    public BackupFolderCruderMaskCounter(FileBackupFolderCruder fileBackupFolderCruder)
    {
        _fileBackupFolderCruder = fileBackupFolderCruder;
    }

    protected override bool MaskExists(string mask)
    {
        return _fileBackupFolderCruder.ContainsRecordWithKey(mask);
    }
}