using System.Collections.Generic;
using System.Linq;
using ApAgentData.LibApAgentData.Models;
using AppCliTools.CliMenu;
using AppCliTools.CliParameters;
using AppCliTools.CliParameters.CliMenuCommands;
using ParametersManagement.LibParameters;

namespace ApAgent.Cruders;

public sealed class ReplacePairsSetCruder : ParCruder<ReplacePairsSet>
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public ReplacePairsSetCruder(IParametersManager parametersManager,
        Dictionary<string, ReplacePairsSet> currentValuesDictionary) : base(parametersManager, currentValuesDictionary,
        "Replace Pairs Set", "Replace Pairs Sets", true)
    {
    }

    public static ReplacePairsSetCruder Create(IParametersManager parametersManager)
    {
        var parameters = (ApAgentParameters)parametersManager.Parameters;
        return new ReplacePairsSetCruder(parametersManager, parameters.ReplacePairsSets);
    }

    //public საჭიროა ApAgent პროექტისათვის
    public override void FillDetailsSubMenu(CliMenuSet itemSubMenuSet, string itemName)
    {
        base.FillDetailsSubMenu(itemSubMenuSet, itemName);

        var parameters = (ApAgentParameters)ParametersManager.Parameters;
        Dictionary<string, ReplacePairsSet> replacePairsSets = parameters.ReplacePairsSets;
        ReplacePairsSet replacePairsSet = replacePairsSets[itemName];

        var detailsCruder = new ReplacePairsSetFileMaskCruder(ParametersManager, itemName);

        //var detailsCruder = new SimpleNamesWithDescriptionsFieldEditor<ReactAppTypeCruder>(
        //    nameof(ReplacePairsSet.PairsDict), ParametersManager);

        var newItemCommand =
            new NewItemCliMenuCommand(detailsCruder, itemName, $"Create New {detailsCruder.CrudName}");
        itemSubMenuSet.AddMenuItem(newItemCommand);

        foreach (ItemSubMenuCliMenuCommand detailListCommand in replacePairsSet.PairsDict.Select(mask =>
                     new ItemSubMenuCliMenuCommand(detailsCruder, mask.Key, itemName, true)))
        {
            itemSubMenuSet.AddMenuItem(detailListCommand);
        }
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
