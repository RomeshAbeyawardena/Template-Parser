using Microsoft.Extensions.FileProviders;

namespace TemplateParser.Contracts;

public interface IDirectoryOperation : IFileOperation
{
    IDirectoryContents GetDirectories(string subpath);
}
