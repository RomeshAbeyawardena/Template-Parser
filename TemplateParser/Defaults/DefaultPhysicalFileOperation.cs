using Microsoft.Extensions.FileProviders;
using TemplateParser.Contracts;
using IOPath = System.IO.Path;
namespace TemplateParser.Defaults;

public class DefaultPhysicalFileOperation : IFileOperation
{
    private readonly IFileProvider? fileProvider;

    protected IFileProvider? FileProvider => fileProvider;

    protected virtual void Dispose()
    {
        (fileProvider as IDisposable)?.Dispose();
    }

    void IDisposable.Dispose()
    {
        Dispose();
        GC.SuppressFinalize(this);
    }

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
        FullName = IOPath.GetFullPath(fileInfo.FullName);
        fileProvider = new PhysicalFileProvider(Path ?? throw new NullReferenceException());
    }

    public bool Exists { get; protected set; }
    public bool IsFile { get; protected set; }
    public string? Name { get; protected set; }
    public string? Path { get; protected set; }
    public virtual string? Content { get; set; }
    public string? FullName { get; protected set; }
    
    public Stream ReadStream => (fileProvider ?? throw new NullReferenceException())
        .GetFileInfo(FullName ?? throw new NullReferenceException()).CreateReadStream();

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

        if (string.IsNullOrWhiteSpace(FullName))
        {
            throw new NullReferenceException();
        }

        return File.WriteAllTextAsync(FullName, Content, cancellationToken);
    }
}
