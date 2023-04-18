using Microsoft.Extensions.FileProviders;
using TemplateParser.Contracts;

namespace TemplateParser.Extensions;

public static class TemplateParserExtensions
{
    public static IEnumerable<ITemplate> ParseFromFile(this ITemplateParser template, IFileProvider fileProvider, string fileName)
    {
        var file = fileProvider.GetFileInfo(fileName);
        
        if (!file.Exists)
        {
            return Array.Empty<ITemplate>();
        }
        
        using var readStream = file.CreateReadStream();
        return TemplateParserHelper.Parse(readStream);
    }
}
