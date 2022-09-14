namespace UnifiedPrintApi.Model.Interfaces.Generic;

public class GenericApiDescription : IApiDescription
{
    public string Name { get; }
    public string Color { get; }
    public List<SortType> SortTypes { get; }
    public Uri Site { get; }
    public string Description { get; }

    public GenericApiDescription(IApiDescription description)
    {
        Name = description.Name;
        Color = description.Color;
        SortTypes = description.SortTypes;
        Site = description.Site;
        Description = description.Description;
    }
}