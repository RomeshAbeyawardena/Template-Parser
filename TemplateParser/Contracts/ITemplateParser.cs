namespace TemplateParser.Contracts;

public interface ITemplateParser
{
    IEnumerable<ITemplate> Parse(Stream stream);
    IEnumerable<ITemplate> Parse(string template);
}
