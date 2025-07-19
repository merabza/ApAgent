using System.Collections.Generic;
using CliParameters.Cruders;

namespace ApAgent.Cruders;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed class FileBackupFolderCruder : SimpleNamesWithDescriptionsCruder
{
    private readonly Dictionary<string, string> _currentValuesDictionary;

    // ReSharper disable once ConvertToPrimaryConstructor
    public FileBackupFolderCruder(Dictionary<string, string> currentValuesDictionary) : base("Backup Folder Path",
        "Backup Folder Paths")
    {
        _currentValuesDictionary = currentValuesDictionary;
    }

    protected override Dictionary<string, string> GetDictionary()
    {
        return _currentValuesDictionary;
    }
}