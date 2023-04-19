using TemplateParser.Contracts;

namespace TemplateParser.Defaults;

public class DefaultTemplateExecutor : ITemplateExecutor
{
    private readonly IEnumerable<ITemplateProcessor> templateProcessors;
    private readonly IDictionary<string, string> globalVariables;

    public DefaultTemplateExecutor(IEnumerable<ITemplateProcessor> templateProcessors,
        IDictionary<string, string> globalVariables)
    {
        this.templateProcessors = templateProcessors;
        this.globalVariables = globalVariables;
    }

    public void Dispose()
    {
        foreach(var templateProcessor in templateProcessors)
        {
            templateProcessor.Dispose();
        }
        GC.SuppressFinalize(this);
    }

    public async Task Execute(IEnumerable<ITemplate> templates, CancellationToken cancellationToken)
    {
        foreach(var template in templates) 
        { 
            var processors = templateProcessors.Where(t => t.CanProcess(template));
            foreach(var processor in processors)
            {
                if (processor.GlobalVariables != null)
                {
                    processor.GlobalVariables = globalVariables;
                }

                await processor.Process(template, cancellationToken);
            }
        }
    }
}