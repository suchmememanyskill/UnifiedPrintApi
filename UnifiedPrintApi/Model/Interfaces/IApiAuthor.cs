namespace UnifiedPrintApi.Model.Interfaces;

public interface IApiAuthor
{
    string Name { get; }
    string Website { get; }
    Uri Thumbnail { get; }
}