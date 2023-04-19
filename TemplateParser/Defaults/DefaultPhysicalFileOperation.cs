using TemplateParser.Contracts;
using IOPath = System.IO.Path;
namespace TemplateParser.Defaults;

public class DefaultPhysicalFileOperation : IFileOperation
{
    public DefaultPhysicalFileOperation()
    {
        
    }

    public DefaultPhysicalFileOperation(string path, string name)
        : this(new FileInfo(IOPath.Combine(path, name)))
    {
        
    }

    public DefaultPhysicalFileOperation(FileInfo fileInfo)
    {
        Exists = fileInfo.Exists;
        IsFile = fileInfo.Exists;
        Name = fileInfo.Name;
        Path = IOPath.GetDirectoryName(fileInfo.FullName);
    }

    public bool Exists { get; protected set; }
    public bool IsFile { get; protected set; }
    public string? Name { get; protected set; }
    public string? Path { get; protected set; }
    public virtual string? Content { get; set; }

    public virtual Task Create(CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(Path))
        {
            throw new NullReferenceException();
        }

        if (string.IsNullOrWhiteSpace(Name))
        {
            throw new NullReferenceException();
        }

        return File.WriteAllTextAsync(IOPath.Combine(Path, Name), Content, cancellationToken);
    }
}
