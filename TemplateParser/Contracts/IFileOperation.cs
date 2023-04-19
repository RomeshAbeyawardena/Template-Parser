namespace TemplateParser.Contracts;
public interface IFileOperation : IDisposable
{
    string? Content { get; set; }
    Task Create(CancellationToken cancellationToken);
    bool Exists { get; }
    bool IsFile { get; }
    string? Name { get; }
    string? Path { get; }
    string? FullName { get; }
}
