namespace UnifiedPrintApi.Model.Interfaces.Generic;

public class GenericApiDescription : IApiDescription
{
    private IApiDescription _original;
    public string Name { get; }
    public string Color { get; }
    public List<SortType> SortTypes { get; }
    public Uri Site { get; }
    public string Description { get; }

    public IApiPreviewPosts GetPosts(SortType type, int page, int perPage) =>
        _original.GetPosts(type, page, perPage);

    public IApiPreviewPosts GetPostsBySearch(string search, int page, int perPage) =>
        _original.GetPostsBySearch(search, page, perPage);

    public IApiPost GetPostById(string search) => _original.GetPostById(search);

    public GenericApiDescription(IApiDescription description)
    {
        Name = description.Name;
        Color = description.Color;
        SortTypes = description.SortTypes;
        Site = description.Site;
        Description = description.Description;
        _original = description;
    }
}