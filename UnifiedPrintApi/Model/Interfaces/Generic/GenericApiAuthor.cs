namespace UnifiedPrintApi.Model.Interfaces.Generic;

public class GenericApiAuthor : IApiAuthor
{
    public string Name { get; }
    public Uri Website { get; }
    public Uri Thumbnail { get; }

    public GenericApiAuthor(IApiAuthor author)
    {
        Name = author.Name;
        Website = author.Website;
        Thumbnail = author.Thumbnail;
    }
}