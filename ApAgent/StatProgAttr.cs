using SystemToolsShared;

namespace ApAgent;

public static class StatProgAttr
{
    public static void SetAttr()
    {
        ProgramAttributes.Instance.SetAttribute("AppName", "ApAgent");
        ProgramAttributes.Instance.SetAttribute("AppKey", "8959D94B-596E-48C1-A644-29667AEE2250");
    }
}