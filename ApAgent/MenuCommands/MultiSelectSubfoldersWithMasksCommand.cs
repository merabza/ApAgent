﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using ApAgent.Counters;
using ApAgent.Cruders;
using CliMenu;
using LibMenuInput;
using SystemToolsShared;

namespace ApAgent.MenuCommands;

public sealed class MultiSelectSubfoldersWithMasksCommand : CliMenuCommand
{
    private readonly FileBackupFolderCruder _fileBackupFolderCruder;
    private readonly Dictionary<string, string> _masksAndFolders;

    // ReSharper disable once ConvertToPrimaryConstructor
    public MultiSelectSubfoldersWithMasksCommand(Dictionary<string, string> masksAndFolders,
        FileBackupFolderCruder fileBackupFolderCruder) : base("Multi Select Subfolders", EMenuAction.Reload)
    {
        _masksAndFolders = masksAndFolders;
        _fileBackupFolderCruder = fileBackupFolderCruder;
    }

    protected override bool RunBody()
    {
        var folderName = MenuInputer.InputFolderPath("Folder which subfolders you wont to add to backups folders list");

        if (string.IsNullOrWhiteSpace(folderName))
            return false;

        var dir = CheckFolder(folderName);
        if (dir == null)
            return false;

        //დადგინდეს ამ ფოლდერებიდან რომელიმე არის თუ არა დასაბექაპებელ სიაში. და თუ არის მისთვის ჩაირთოს ჭეშმარიტი
        var foldersChecks = dir.GetDirectories().OrderBy(o => o.Name)
            .ToDictionary(k => k.Name, v => _masksAndFolders.ContainsValue(v.FullName));
        //გამოვიდეს სიიდან ამრჩევი
        MenuInputer.MultipleInputFromList($"Select subfolders from {folderName}", foldersChecks);
        DictMaskCounter dictMaskCounter = new(_masksAndFolders);

        foreach (var kvp in foldersChecks)
        {
            var path = Path.Combine(folderName, kvp.Key);
            if (kvp.Value)
            {
                //ჩართული ჩავამატოთ თუ არ არსებობს
                if (!_masksAndFolders.ContainsValue(path))
                    _masksAndFolders.Add(dictMaskCounter.CountMask(path), path);
            }
            else
            {
                //გამორთული ამოვაკლოთ თუ არსებობს
                if (_masksAndFolders.ContainsValue(path))
                    _masksAndFolders.Remove(path);
            }
        }

        _fileBackupFolderCruder.Save("Changes saved");
        return true;
    }

    private DirectoryInfo? CheckFolder(string folderName)
    {
        DirectoryInfo dir = new(folderName);

        if (!dir.Exists)
        {
            StShared.WriteErrorLine($"folder {folderName} does not exists", true);
            return null;
        }

        //დადგინდეს ეს ფოლდერი თვითონ და ამ ფოლდერის რომელიმე მშობელი ფოლდერი არის თუ არა დასაბექაპებელ სიაში.
        //და თუ არის, მაშინ არ დავუშვათ ამ ფოლდერის ქვეფოლდერების სიის გამოძახება.
        //მიზეზი კი გამოვიტანოთ შეტყობინების სახით
        //არ შეიძლება ქვეფოლდერის არჩევა დასაბეკაპებლად, თუ მისი წინაპარი უკვე არჩეულია. ჯერ ამოიღეთ სიიდან წინაპარი
        if (_masksAndFolders.Any(kvp => Contains(kvp.Value, dir.FullName)))
        {
            StShared.WriteErrorLine($"folder {folderName} can not use because of existing backup folders", true);
            return null;
        }

        if (dir.GetDirectories().Length > 0)
            return dir;

        StShared.WriteErrorLine($"folder {folderName} have not subfolders", true);
        return null;
    }

    private static bool Contains(DirectoryInfo di1, DirectoryInfo di2)
    {
        while (true)
        {
            if (di2.FullName == di1.FullName) return true;
            if (di2.Parent == null) return false;
            di2 = di2.Parent;
        }
    }

    private static bool Contains(string dir1, string dir2)
    {
        DirectoryInfo di1 = new(dir1);
        DirectoryInfo di2 = new(dir2);
        return Contains(di1, di2);
    }
}