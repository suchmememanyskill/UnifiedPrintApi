namespace UnifiedPrintApi.Model.Interfaces.Generic;

public static class GenericExtensions
{
    public static GenericApiAuthor Generic(this IApiAuthor author) => new(author);
    public static GenericApiDescription Generic(this IApiDescription desc) => new(desc);
    public static GenericApiPost Generic(this IApiPost post) => new(post);
    public static GenericApiPreviewPost Generic(this IApiPreviewPost post) => new(post);
}