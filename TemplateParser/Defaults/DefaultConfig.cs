using Microsoft.Extensions.Configuration;
using TemplateParser.Contracts;

namespace TemplateParser.Defaults;

public record DefaultConfig : IConfig
{
    public DefaultConfig(IConfiguration configuration)
    {
        configuration.Bind(this);
    }

    public DefaultConsoleOptions? Options { get; set; }
    IConsoleOptions? IConfig.Options => Options;
}