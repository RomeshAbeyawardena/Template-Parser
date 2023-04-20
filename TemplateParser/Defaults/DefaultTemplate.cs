using System.Text.RegularExpressions;
using System.Text;
using TemplateParser.Contracts;
using TemplateParser.Extensions;

namespace TemplateParser.Defaults;

public partial record DefaultTemplate : ITemplate
{
    private bool writingContent = false;
    private readonly IEnumerable<ITemplate> templateItems;
    private readonly IGlobalVariables globalVariables;
    private readonly IConfig config;
    private readonly StringBuilder contentBuilder;
    private IDictionary<string, string> currentLanguage;

    private string GetKeywordOrDefault(string keyWord, string? prepend = null)
    {
        if (currentLanguage.TryGetValue(keyWord, out var kW))
        {
            keyWord = kW;
        }

        if (!string.IsNullOrWhiteSpace(prepend))
        {
            return $"{prepend}{keyWord}";
        }

        return keyWord;
    }

    private void ProcessVariable(string command, string parameters)
    {
        switch (command)
        {
            case CommandVariables.COMMAND_FILE_LANGUAGE:
                if (config.Options != null
                    && config.Options.Languages.TryGetValue(parameters, out var lang))
                {
                    currentLanguage = lang;
                }
                break;
            case CommandVariables.COMMAND_BASE_PATH:
                globalVariables[command] = parameters;
                break;
            case CommandVariables.COMMAND_GLOBAL_VAR:
                foreach (var parameter in parameters.Split(";", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    var parameterSeparatorIndex = parameter.IndexOf('=');
                    var parameterName = parameter[..parameterSeparatorIndex];
                    var parameterValue = parameter[(parameterSeparatorIndex + 1)..];
                    globalVariables[parameterName] = parameterValue;
                }
                break;
        }
    }

    private string ReplaceVariables(string content, IGlobalVariables variables)
    {
        var templateContent = variables.ReplaceWithVariables(content);

        templateContent = globalVariables.ReplaceWithVariables(templateContent);

        return templateContent;
    }

    public DefaultTemplate(IEnumerable<ITemplate> templateItems, IGlobalVariables globalVariables, IConfig config)
    {
        this.contentBuilder = new();
        this.templateItems = templateItems;
        this.globalVariables = globalVariables;
        this.config = config;
        Variables = new DefaultGlobalVariables("$");
        currentLanguage = new Dictionary<string, string>();
        UsedTemplates = new List<string>();
    }

    public DefaultTemplate(IEnumerable<ITemplate> templateItems, IGlobalVariables globalVariables, ITemplate? previousTemplate, IConfig config)
        : this(templateItems, globalVariables, config)
    {
        Path = previousTemplate?.Path;
        FileName = previousTemplate?.FileName;
    }

    public IGlobalVariables Variables { get; set; }
    public IList<string> UsedTemplates { get; set; }
    public string? TemplateName { get; set; }
    public string? Path { get; set; }
    public string? FileName { get; set; }
    public string? Content { get; set; }
    public TemplateType? Type { get; set; }

    /// <summary>
    /// Parses current line into template format
    /// </summary>
    /// <param name="line"></param>
    /// <returns><see cref="true"/> when the current template is fully generated
    /// <see cref="false"/> when incomplete
    /// </returns>
    /// <exception cref="NullReferenceException"></exception>
    public bool ParseLine(string line)
    {
        bool isEndOfTemplate = false;

        if (!writingContent && line.StartsWith(GetKeywordOrDefault(Language.SET_VARIABLE), StringComparison.InvariantCultureIgnoreCase))
        {
            var setCommandBody = line[line.LastIndexOf('-')..];
            var commandSeparatorIndex = setCommandBody.IndexOf(":");
            var setCommandName = setCommandBody[1..commandSeparatorIndex];
            var setCommandParameters = setCommandBody[(commandSeparatorIndex + 1)..];

            this.ProcessVariable(setCommandName, setCommandParameters);
        }

        else if (!writingContent && line.StartsWith(GetKeywordOrDefault(Language.DEFINE_TEMPLATE), StringComparison.InvariantCultureIgnoreCase))
        {
            Type = TemplateType.InMemoryTemplate;
            TemplateName = line[(line.LastIndexOf(":") + 1)..];
        }

        else if (!writingContent && line.StartsWith(GetKeywordOrDefault(Language.DEFINE_PATH), StringComparison.InvariantCultureIgnoreCase))
        {
            if (!Type.HasValue)
            {
                Type = TemplateType.FilePathTemplate;
            }
            Path = line[(line.LastIndexOf(":") + 1)..];
        }

        else if (!writingContent && line.StartsWith(GetKeywordOrDefault(Language.DEFINE_FILENAME), StringComparison.InvariantCultureIgnoreCase))
        {
            if (!Type.HasValue)
            {
                Type = TemplateType.FileTemplate;
            }

            FileName = line[(line.LastIndexOf(":") + 1)..];
        }

        else if (!writingContent && line.StartsWith(GetKeywordOrDefault($"#{Language.START_SEQUENCE} {Language.TEMPLATE_AREA}#"), StringComparison.InvariantCultureIgnoreCase))
        {
            writingContent = true;
        }

        else if (line.EndsWith(GetKeywordOrDefault($"#{Language.END_SEQUENCE} {Language.TEMPLATE_AREA}#"), StringComparison.InvariantCultureIgnoreCase))
        {
            var regex = EndTemplateRegex();
            contentBuilder.Append(regex.Replace(line, string.Empty));

            if (UsedTemplates.Count > 0)
            {
                foreach (var template in UsedTemplates)
                {
                    var templateContent = (templateItems
                        .FirstOrDefault(f => f.TemplateName == template)
                        ?? throw new NullReferenceException("Template not found")).Content
                        ?? throw new NullReferenceException("Template content not set");

                    contentBuilder.Append(ReplaceVariables(templateContent, Variables));
                }
            }
            Content = contentBuilder.ToString();
            writingContent = false;
            isEndOfTemplate = true;
        }
        else if (writingContent)
        {
            if (UsedTemplates.Count > 0 
                && line.StartsWith(GetKeywordOrDefault(Language.SET_VARIABLE, "#"), StringComparison.InvariantCultureIgnoreCase))
            {
                var variableDefinition = line[(line.LastIndexOf(':') + 1)..];

                var lastIndex = variableDefinition.LastIndexOf('=');
                Variables[variableDefinition[..lastIndex]] =
                    variableDefinition[(lastIndex + 1)..];

            }
            else if (line.StartsWith(GetKeywordOrDefault(Language.USE_TEMPLATE, "##"), StringComparison.InvariantCultureIgnoreCase))
            {
                UsedTemplates.Add(line[(line.LastIndexOf(':') + 1)..]);
            }
            else
                contentBuilder.Append(ReplaceVariables(line, Variables));
        }

        return isEndOfTemplate;
    }

    [GeneratedRegex($"(#{Language.END_SEQUENCE} {Language.TEMPLATE_AREA}#)")]
    private static partial Regex EndTemplateRegex();
}
