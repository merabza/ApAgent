namespace ApAgent.Counters;

public sealed class DateMaskCounter
{
    public string Count()
    {
        return "_yyyy_MM_dd_HHmmss_fffffff";
    }
}