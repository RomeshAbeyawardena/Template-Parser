namespace TemplateParser.App;

public record ConsoleOptions
{
    public ConsoleOptions()
    {
        Commands = new Dictionary<string, string>();
        Keywords = new Dictionary<string, string>();
    }

    public IDictionary<string, string> Commands { get; set; }
    public IDictionary<string, string> Keywords { get; set; }
}