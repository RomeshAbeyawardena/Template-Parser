using Microsoft.Extensions.Configuration;

namespace TemplateParser.Defaults;

public record DefaultConfig
{
    public DefaultConfig(IConfiguration configuration)
    {
        configuration.Bind(this);
    }

    public DefaultConsoleOptions? Options { get; set; }
}
