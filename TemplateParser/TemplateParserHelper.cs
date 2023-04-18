using Microsoft.Extensions.FileProviders;
using TemplateParser.Contracts;
using TemplateParser.Defaults;
using TemplateParser.Extensions;

namespace TemplateParser;

public static class TemplateParserHelper
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

    public static IEnumerable<ITemplate> ParseFromFile(IFileProvider fileProvider, string fileName, ITemplateParser? templateParser = null)
    {
        templateParser ??= new DefaultTemplateParser();

        return templateParser.ParseFromFile(fileProvider, fileName);
    }
}
