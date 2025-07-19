using System.IO;

namespace ApAgent.Counters;

public /*open*/ class MaskCounter
{
    protected virtual bool MaskExists(string mask)
    {
        return false;
    }

    public string CountMask(string path)
    {
        var dir = new DirectoryInfo(path);
        var mask = dir.Name;

        var startDefVal = mask;
        var index = 1;
        while (MaskExists(mask))
        {
            index++;
            mask = $"{startDefVal}{index}";
        }

        return mask;
    }
}