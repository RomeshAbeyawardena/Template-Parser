using Microsoft.Extensions.Configuration;
using TemplateParser.Contracts;

namespace TemplateParser.Defaults;

public record DefaultConsoleConfig : IConsoleConfig
{
    public DefaultConsoleConfig(IConfiguration configuration)
    {
        configuration.Bind(this);
    }

    public string? Input { get; set; }
    public string? KeyValues { get; set; }
    public string? OutputFiles { get; set; }
    public string? KeyValueSeparator { get; set; }
    public bool? Test { get; set; }
    public IDictionary<string, string> ValueDictionary
    {
        get
        {
            var dictionary = new Dictionary<string, string>();
            if (string.IsNullOrWhiteSpace(KeyValues))
            {
                return dictionary;
            }

            foreach(var keyValue in KeyValues.Split(KeyValueSeparator, StringSplitOptions.RemoveEmptyEntries))
            {
                var separatorIndex = keyValue.IndexOf('=');
                dictionary.Add(keyValue[..separatorIndex],
                    keyValue[(separatorIndex + 1)..]);
            }

            return dictionary;
        }
    }
}
