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
