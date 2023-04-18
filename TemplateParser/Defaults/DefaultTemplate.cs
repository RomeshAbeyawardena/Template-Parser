using System.Text.RegularExpressions;
using System.Text;
using TemplateParser.Contracts;
using TemplateParser.Extensions;

namespace TemplateParser.Defaults;

public partial record DefaultTemplate : ITemplate
{
    private const string COMMAND_BASE_PATH = "BASE_PATH";
    private const string COMMAND_GLOBAL_VAR = "GLOBAL_VAR";
    private bool writingContent = false;
    private readonly IEnumerable<ITemplate> templateItems;
    private readonly IDictionary<string, string> globalVariables;
    private readonly StringBuilder contentBuilder;

    private void ProcessSETCommand(string command, string parameters)
    {
        switch (command)
        {
            case COMMAND_BASE_PATH:
                globalVariables.AddOrUpdate(command, parameters);
                break;
            case COMMAND_GLOBAL_VAR:
                foreach(var parameter in parameters.Split(";", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    var parameterSeparatorIndex = parameter.IndexOf('=');
                    var parameterName = parameter.Substring(0, parameterSeparatorIndex);
                    var parameterValue = parameter.Substring(parameterSeparatorIndex + 1);
                    globalVariables.AddOrUpdate(parameterName, parameterValue);
                }
                break;
        }
    }

    private string ReplaceVariables(string content, IDictionary<string, string> variables)
    {
        var templateContent = content;
        foreach (var (k, v) in variables)
        {
            templateContent = templateContent.Replace(k, v);
        }

        foreach(var (k,v) in globalVariables)
        {
            templateContent = templateContent.Replace($"${k}", v);
        }
        return templateContent;
    }

    public DefaultTemplate(IEnumerable<ITemplate> templateItems, IDictionary<string, string> globalVariables)
    {
        this.contentBuilder = new();
        this.templateItems = templateItems;
        this.globalVariables = globalVariables;
        Variables = new Dictionary<string, string>();
    }

    public DefaultTemplate(IEnumerable<ITemplate> templateItems, IDictionary<string, string> globalVariables, ITemplate? previousTemplate)
        : this(templateItems, globalVariables)
    {
        Path = previousTemplate?.Path;
        FileName = previousTemplate?.FileName;
    }

    public IDictionary<string, string> Variables { get; set; }
    public string? UseTemplateName { get; set; }
    public string? TemplateName { get; set; }
    public string? Path { get; set; }
    public string? FileName { get; set; }
    public string? Content { get; set; }
    public TemplateType Type { get; set; }

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

        if(!writingContent && line.StartsWith("SET", StringComparison.InvariantCultureIgnoreCase))
        {
            var setCommandBody = line[line.LastIndexOf('-')..];
            var commandSeparatorIndex = setCommandBody.IndexOf(":");
            var setCommandName = setCommandBody[1..commandSeparatorIndex];
            var setCommandParameters = setCommandBody[(commandSeparatorIndex + 1)..];

            this.ProcessSETCommand(setCommandName, setCommandParameters);
        } 

        else if (!writingContent && line.StartsWith("Define", StringComparison.InvariantCultureIgnoreCase))
        {
            Type = TemplateType.InMemoryTemplate;
            TemplateName = line[(line.LastIndexOf(":") + 1)..];   
        }

        else if (!writingContent && line.StartsWith("Path", StringComparison.InvariantCultureIgnoreCase))
        {
            Type = TemplateType.FilePathTemplate;
            Path = line[(line.LastIndexOf(":") + 1)..];
        }

        else if (!writingContent && line.StartsWith("File", StringComparison.InvariantCultureIgnoreCase))
        {
            Type = TemplateType.FileTemplate;
            FileName = line[(line.LastIndexOf(":") + 1)..];
        }

        else if (!writingContent && line.StartsWith("#BEGIN TEMPLATE#", StringComparison.InvariantCultureIgnoreCase))
        {
            writingContent = true;
        }

        else if (line.EndsWith("#END TEMPLATE#", StringComparison.InvariantCultureIgnoreCase))
        {
            var regex = MyRegex();
            contentBuilder.Append(regex.Replace(line, string.Empty));

            if (!string.IsNullOrWhiteSpace(UseTemplateName))
            {
                var templateContent = (templateItems
                    .FirstOrDefault(f => f.TemplateName == UseTemplateName)
                    ?? throw new NullReferenceException("Template not found")).Content
                    ?? throw new NullReferenceException("Template content not set");

                contentBuilder.Append(ReplaceVariables(templateContent, Variables));
            }
            Content = contentBuilder.ToString();
            writingContent = false;
            isEndOfTemplate = true;
        }
        else if (writingContent)
        {
            if (!string.IsNullOrWhiteSpace(UseTemplateName))
            {
                if (line.StartsWith("#SET", StringComparison.InvariantCultureIgnoreCase))
                {
                    var variableDefinition = line[(line.LastIndexOf(':') + 1)..];

                    var lastIndex = variableDefinition.LastIndexOf('=');
                    Variables.Add(
                        variableDefinition[..lastIndex],
                        variableDefinition[(lastIndex + 1)..]);
                }
            }
            else if (line.StartsWith("##USE", StringComparison.InvariantCultureIgnoreCase))
            {
                UseTemplateName = line[(line.LastIndexOf(':') + 1)..];
            }
            else
                contentBuilder.Append(ReplaceVariables(line, Variables));
        }

        return isEndOfTemplate;
    }

    [GeneratedRegex("(#END TEMPLATE#)")]
    private static partial Regex MyRegex();
}
