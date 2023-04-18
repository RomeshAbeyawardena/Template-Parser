using TemplateParser.Contracts;
using TemplateParser.Defaults;

namespace TemplateParser;

public static class TemplateParser
{
    public static IEnumerable<ITemplate> Parse(Stream stream, ITemplateParser? templateParser = null) 
    {
        templateParser ??= new DefaultTemplateParser();

        return templateParser.Parse(stream);
    }

    public static IEnumerable<ITemplate> Parse(string template, ITemplateParser? templateParser = null)
    {
        templateParser ??= new DefaultTemplateParser();

        return templateParser.Parse(template);
    }
}
