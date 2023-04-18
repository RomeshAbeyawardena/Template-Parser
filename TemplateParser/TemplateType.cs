namespace TemplateParser;

[Flags]
public enum TemplateType
{
    InMemoryTemplate = 1,
    FileTemplate = 2,
    FilePathTemplate = 4
}
