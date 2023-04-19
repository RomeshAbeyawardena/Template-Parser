using IOPath = System.IO.Path;
using TemplateParser.Contracts;
using System.IO;

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
        var lastDirIndex = directoryInfo.FullName.LastIndexOf(IOPath.PathSeparator);
        Path = directoryInfo.FullName[..lastDirIndex];
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

        Directory.CreateDirectory(IOPath.Combine(Path, Name));
        return Task.CompletedTask;
    }
}
