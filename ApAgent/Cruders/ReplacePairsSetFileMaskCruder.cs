using System.Collections.Generic;
using System.Linq;
using ApAgent.Models;
using CliParameters;
using CliParameters.FieldEditors;
using LibApAgentData.Models;
using LibParameters;

namespace ApAgent.Cruders;

public sealed class ReplacePairsSetFileMaskCruder : ParCruder
{
    private readonly string _replacePairSetName;

    public ReplacePairsSetFileMaskCruder(IParametersManager parametersManager, string replacePairSetName) : base(
        parametersManager, "Replace Mask", "Replace Masks", true)
    {
        _replacePairSetName = replacePairSetName;
        FieldEditors.Add(new TextFieldEditor(nameof(TextPairItemData.Text1)));
        FieldEditors.Add(new TextFieldEditor(nameof(TextPairItemData.Text2)));
    }

    private Dictionary<string, string> GetFileMasks()
    {
        var parameters = (ApAgentParameters)ParametersManager.Parameters;
        var replacePairSets = parameters.ReplacePairsSets;
        return replacePairSets.TryGetValue(_replacePairSetName, out var replacePairSet) ? replacePairSet.PairsDict : [];
    }

    protected override Dictionary<string, ItemData> GetCrudersDictionary()
    {
        return GetFileMasks().ToDictionary(k => k.Key,
            v => (ItemData)new TextPairItemData { Text1 = v.Key, Text2 = v.Value });
    }

    protected override ItemData CreateNewItem(string? recordKey, ItemData? defaultItemData)
    {
        return new TextPairItemData();
    }


    public override bool ContainsRecordWithKey(string recordKey)
    {
        var fileMasks = GetFileMasks();
        return fileMasks.ContainsKey(recordKey);
    }

    protected override void RemoveRecordWithKey(string recordKey)
    {
        var fileMasks = GetFileMasks();
        fileMasks.Remove(recordKey);
    }

    protected override void AddRecordWithKey(string recordName, ItemData newRecord)
    {
        var parameters = (ApAgentParameters)ParametersManager.Parameters;
        var replacePairSets = parameters.ReplacePairsSets;
        if (!replacePairSets.TryGetValue(_replacePairSetName, out var replacePairSet))
            return;

        if (newRecord is TextPairItemData textPairItemData && !string.IsNullOrWhiteSpace(textPairItemData.Text1) &&
            !string.IsNullOrWhiteSpace(textPairItemData.Text2))
            replacePairSet.PairsDict.Add(textPairItemData.Text1, textPairItemData.Text2);
    }
}