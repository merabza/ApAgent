using System.Collections.Generic;
using System.Linq;
using CliMenu;
using CliParameters;
using CliParameters.CliMenuCommands;
using LibApAgentData.Models;
using LibParameters;

namespace ApAgent.Cruders;

public sealed class ReplacePairsSetCruder : ParCruder<ReplacePairsSet>
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public ReplacePairsSetCruder(IParametersManager parametersManager,
        Dictionary<string, ReplacePairsSet> currentValuesDictionary) : base(parametersManager, currentValuesDictionary,
        "Replace Pairs Set", "Replace Pairs Sets", true)
    {
    }

    //public საჭიროა ApAgent პროექტისათვის
    public override void FillDetailsSubMenu(CliMenuSet itemSubMenuSet, string recordName)
    {
        base.FillDetailsSubMenu(itemSubMenuSet, recordName);

        var parameters = (ApAgentParameters)ParametersManager.Parameters;
        var replacePairsSets = parameters.ReplacePairsSets;
        var replacePairsSet = replacePairsSets[recordName];

        var detailsCruder = new ReplacePairsSetFileMaskCruder(ParametersManager, recordName);

        //var detailsCruder = new SimpleNamesWithDescriptionsFieldEditor<ReactAppTypeCruder>(
        //    nameof(ReplacePairsSet.PairsDict), ParametersManager);


        NewItemCliMenuCommand newItemCommand = new(detailsCruder, recordName, $"Create New {detailsCruder.CrudName}");
        itemSubMenuSet.AddMenuItem(newItemCommand);

        foreach (var detailListCommand in replacePairsSet.PairsDict.Select(mask =>
                     new ItemSubMenuCliMenuCommand(detailsCruder, mask.Key, recordName, true)))
            itemSubMenuSet.AddMenuItem(detailListCommand);
    }

    //protected override Dictionary<string, ItemData> GetCrudersDictionary()
    //{
    //    var parameters = (ApAgentParameters)ParametersManager.Parameters;
    //    return parameters.ReplacePairsSets.ToDictionary(p => p.Key, p => (ItemData)p.Value);
    //}

    //public override bool ContainsRecordWithKey(string recordKey)
    //{
    //    var parameters = (ApAgentParameters)ParametersManager.Parameters;
    //    var replacePairsSet = parameters.ReplacePairsSets;
    //    return replacePairsSet.ContainsKey(recordKey);
    //}

    //public override void UpdateRecordWithKey(string recordName, ItemData newRecord)
    //{
    //    var newFileStorage = (ReplacePairsSet)newRecord;
    //    var parameters = (ApAgentParameters)ParametersManager.Parameters;
    //    parameters.ReplacePairsSets[recordName] = newFileStorage;
    //}

    //protected override void AddRecordWithKey(string recordName, ItemData newRecord)
    //{
    //    var newFileStorage = (ReplacePairsSet)newRecord;
    //    var parameters = (ApAgentParameters)ParametersManager.Parameters;
    //    parameters.ReplacePairsSets.Add(recordName, newFileStorage);
    //}

    //protected override void RemoveRecordWithKey(string recordKey)
    //{
    //    var parameters = (ApAgentParameters)ParametersManager.Parameters;
    //    var replacePairsSets = parameters.ReplacePairsSets;
    //    replacePairsSets.Remove(recordKey);
    //}

    //protected override ItemData CreateNewItem(string? recordKey, ItemData? defaultItemData)
    //{
    //    return new ReplacePairsSet();
    //}
}