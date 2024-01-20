namespace UnifiedPrintApi.Service.MakerWorld.Models;

public class MWRootModel
{
    public MWPagePropsModel PageProps { get; set; }
}

public class MWPagePropsModel
{
    public MWDesign Design { get; set; }
}