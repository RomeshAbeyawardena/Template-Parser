using IOPath = System.IO.Path;
using TemplateParser.Contracts;
using System.IO;
using Microsoft.Extensions.FileProviders;

namespace TemplateParser.Defaults;

public class DefaultPhysicalDirectoryOperation : DefaultPhysicalFileOperation, IDirectoryOperation
{
    public DefaultPhysicalDirectoryOperation(string pathName)
        : this(new DirectoryInfo(pathName))
    {
    }

    public DefaultPhysicalDirectoryOperation(DirectoryInfo directoryInfo)
    {
        Exists = directoryInfo.Exists;
        IsFile = false;
        Name = directoryInfo.Name;
        var lastDirIndex = directoryInfo.FullName.LastIndexOf(IOPath.DirectorySeparatorChar);
        Path = directoryInfo.FullName[..lastDirIndex];
        FullName = directoryInfo.FullName;
    }

    public override string? Content { get => base.Content; set => throw new NotSupportedException(); }

    public override Task Create(CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(Path))
        {
            throw new NullReferenceException();
        }

        if (string.IsNullOrWhiteSpace(Name))
        {
            throw new NullReferenceException();
        }

        if (string.IsNullOrWhiteSpace(FullName))
        {
            throw new NullReferenceException();
        }

        Directory.CreateDirectory(FullName);
        return Task.CompletedTask;
    }

    public IDirectoryContents GetDirectories(string subpath)
    {
        return (FileProvider ?? throw new NullReferenceException()).GetDirectoryContents(subpath);
    }
}
