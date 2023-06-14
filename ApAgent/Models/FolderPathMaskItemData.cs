using LibParameters;

namespace ApAgent.Models;

public sealed class FolderPathMaskItemData : ItemData
{
    public string? Path { get; set; }
    public string? Mask { get; set; }
}