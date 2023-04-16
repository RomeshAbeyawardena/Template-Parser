namespace TemplateParser.App;

public static class TemplateParser
{
    public static IEnumerable<Template> Parse(string template)
    {
        bool canRead = true;
        var textReader = new StringReader(template);
        var templateList = new List<Template>();
        Template? currentTemplate = new(templateList);
        while (canRead)
        {
            var line = textReader.ReadLine();
            if (line == null)
            {
                canRead = false;
                continue;
            }

            if (currentTemplate.ParseLine(line))
            {
                templateList.Add(currentTemplate);
                currentTemplate = new Template(templateList, currentTemplate);
            }
        }
        return templateList;
    }
}
