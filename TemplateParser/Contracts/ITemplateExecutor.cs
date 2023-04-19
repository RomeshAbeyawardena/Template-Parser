namespace TemplateParser.Contracts
{
    public interface ITemplateExecutor : IDisposable
    {
        Task Execute(IEnumerable<ITemplate> templates, CancellationToken cancellationToken);
    }
}
