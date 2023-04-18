using TemplateParser.Contracts;

namespace TemplateParser.Defaults;

public record DefaultConsoleOptions : IConsoleOptions
{
    public DefaultConsoleOptions()
    {
        Commands = new Dictionary<string, string>();
        Languages = new Dictionary<string, IDictionary<string, string>>();
    }

    public IDictionary<string, string> Commands { get; set; }
    public IDictionary<string, IDictionary<string, string>> Languages { get; set; }
}