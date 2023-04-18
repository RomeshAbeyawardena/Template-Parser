using TemplateParser.Contracts;

namespace TemplateParser.Defaults;

public class DefaultTemplateExecutor : ITemplateExecutor
{
    private readonly IEnumerable<ITemplateProcessor> templateProcessors;

    public DefaultTemplateExecutor(IEnumerable<ITemplateProcessor> templateProcessors)
    {
        this.templateProcessors = templateProcessors;
    }

    public async Task Execute(IEnumerable<ITemplate> templates, CancellationToken cancellationToken)
    {
        foreach(var template in templates) 
        { 
            var processors = templateProcessors.Where(t => t.CanProcess(template));
            foreach(var processor in processors)
            {
                await processor.Process(template, cancellationToken);
            }
        }
    }
}