namespace UnifiedPrintApi.Model.Interfaces.Generic;

public class GenericFile
{
    public string Name { get; set; }
    public Uri Url { get; set; }

    public GenericFile(string name, Uri url)
    {
        Name = name;
        Url = url;
    }

    public GenericFile(Uri uri)
    {
        Url = uri;
        Name = null;
        if (uri != null)
            Name = Url.AbsoluteUri.Split("/").Last();
    }

    public GenericFile()
    {
    }
}