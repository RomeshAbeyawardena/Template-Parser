using System.Text;
using System.Text.RegularExpressions;

namespace TemplateParser.App;

public partial record Template
{
    private bool writingContent = false;
    private readonly IEnumerable<Template> templateItems;
    private readonly StringBuilder contentBuilder;
    public Template(IEnumerable<Template> templateItems)
    {
        this.contentBuilder = new();
        this.templateItems = templateItems;
        Variables = new Dictionary<string, string>();
    }

    public Template(IEnumerable<Template> templateItems, Template? previousTemplate)
        : this(templateItems)
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
        if (!writingContent && line.StartsWith("Define", StringComparison.InvariantCultureIgnoreCase))
        {
            //    innerTemplate = new Template() {
            Type = TemplateType.InMemoryTemplate;
            TemplateName = line[(line.LastIndexOf(":") + 1)..];
            //    };
        }

        if (!writingContent && line.StartsWith("Path", StringComparison.InvariantCultureIgnoreCase))
        {
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
                var templateContent = (templateItems.FirstOrDefault(f => f.TemplateName == UseTemplateName) ?? throw new NullReferenceException("Template not found")).Content ?? throw new NullReferenceException("Template content not set");
                foreach (var (k, v) in Variables)
                {
                    templateContent = templateContent.Replace(k, v);
                }

                contentBuilder.Append(templateContent);
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
                contentBuilder.Append(line);
        }

        return isEndOfTemplate;
    }

    [GeneratedRegex("(#END TEMPLATE#)")]
    private static partial Regex MyRegex();
}
