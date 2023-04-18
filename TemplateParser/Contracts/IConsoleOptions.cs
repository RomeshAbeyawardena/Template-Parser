namespace TemplateParser.Contracts;

public interface IConsoleOptions
{
    IDictionary<string, string> Commands { get; set; }
    IDictionary<string, IDictionary<string, string>> Languages { get; set; }
}
