using Microsoft.Extensions.Configuration;

namespace TemplateParser.App;

public record Config
{
    public Config(IConfiguration configuration)
    {
        configuration.Bind(this);
    }

    public ConsoleOptions? Options { get; set; }
}

public record ConsoleOptions
{
    public ConsoleOptions()
    {
        Commands = new Dictionary<string, string>();
    }

    public IDictionary<string, string> Commands { get; set; }
}