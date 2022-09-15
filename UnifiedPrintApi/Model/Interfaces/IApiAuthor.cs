namespace UnifiedPrintApi.Model.Interfaces;

public interface IApiAuthor
{
    string Name { get; }
    Uri Website { get; }
    Uri Thumbnail { get; }
}