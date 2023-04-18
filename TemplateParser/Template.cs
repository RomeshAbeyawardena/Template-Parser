using TemplateParser.Contracts;
using TemplateParser.Defaults.Processors.Template;

namespace TemplateParser;

public static class Template
{
    public static IEnumerable<ITemplateProcessor> Processors => new ITemplateProcessor[]
    {
        new DefaultFilePathTemplateProcessor(),
        new DefaultFileTemplateProcessor(),
        new DefaultInMemoryTemplateProcessor(),
        new DefaultOutputToConsoleTemplateProcessor()
    };
}
