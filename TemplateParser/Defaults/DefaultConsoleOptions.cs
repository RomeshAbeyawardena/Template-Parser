using TemplateParser.Contracts;

namespace TemplateParser.Defaults;

public record DefaultConsoleOptions : IConsoleOptions
{
    public DefaultConsoleOptions()
    {
        Commands = new Dictionary<string, string>();
        Keywords = new Dictionary<string, string>();
    }

    public IDictionary<string, string> Commands { get; set; }
    public IDictionary<string, string> Keywords { get; set; }
}