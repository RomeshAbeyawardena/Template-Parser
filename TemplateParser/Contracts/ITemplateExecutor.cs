namespace TemplateParser.Contracts
{
    public interface ITemplateExecutor
    {
        Task Execute(IEnumerable<ITemplate> templates, CancellationToken cancellationToken);
    }
}
