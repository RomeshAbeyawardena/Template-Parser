﻿using TemplateParser.Contracts;

namespace TemplateParser.Defaults;

public class DefaultTemplateExecutor : ITemplateExecutor
{
    private readonly IEnumerable<ITemplateProcessor> templateProcessors;
    private readonly IGlobalVariables globalVariables;

    public DefaultTemplateExecutor(IEnumerable<ITemplateProcessor> templateProcessors,
        IGlobalVariables globalVariables)
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
            var processors = templateProcessors
                .Where(t => t.CanProcess(template))
                .OrderByDescending(t => t.OrderIndex);
            foreach(var processor in processors)
            {
                if (processor.GlobalVariables == null)
                {
                    processor.GlobalVariables = globalVariables;
                }

                await processor.Process(template, cancellationToken);
            }
        }
    }
}