namespace UnifiedPrintApi.Model.Interfaces.Generic;

public class GenericFile
{
    public string Name { get; }
    public Uri Url { get; }

    public GenericFile(string name, Uri url)
    {
        Name = name;
        Url = url;
    }
}