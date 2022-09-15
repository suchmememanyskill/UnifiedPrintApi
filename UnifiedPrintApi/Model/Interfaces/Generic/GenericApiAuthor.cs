namespace UnifiedPrintApi.Model.Interfaces.Generic;

public class GenericApiAuthor : IApiAuthor
{
    public string Name { get; set; }
    public Uri Website { get; set; }
    public Uri Thumbnail { get; set; }

    public GenericApiAuthor(IApiAuthor author)
    {
        Name = author.Name;
        Website = author.Website;
        Thumbnail = author.Thumbnail;
    }

    public GenericApiAuthor()
    {
    }
}