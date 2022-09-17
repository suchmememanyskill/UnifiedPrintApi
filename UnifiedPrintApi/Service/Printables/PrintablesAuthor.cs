using UnifiedPrintApi.Model.Interfaces;
using UnifiedPrintApi.Service.Printables.Model;

namespace UnifiedPrintApi.Service.Printables;

public class PrintablesAuthor : IApiAuthor
{
    private PrintablesUser _user;

    public PrintablesAuthor(PrintablesUser user)
    {
        _user = user;
    }

    public string Name => _user.Username;
    public Uri Website => _user.ToUri();
    public Uri Thumbnail => (string.IsNullOrWhiteSpace(_user.AvatarFilePath)) ? null : _user.AvatarUri();
}