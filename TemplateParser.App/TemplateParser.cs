namespace TemplateParser.App;

public static class TemplateParser
{
    public static IEnumerable<Template> Parse(Stream stream)
    {
        using var textReader = new StreamReader(stream);
        var templateList = new List<Template>();
        Template? currentTemplate = new(templateList);
        while (!textReader.EndOfStream)
        {
            var line = textReader.ReadLine();

            if(string.IsNullOrEmpty(line))
            {
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

    public static IEnumerable<Template> Parse(string template)
    {
        using (var memoryStream = new MemoryStream())
        {
            using var streamWriter = new StreamWriter(memoryStream);
            streamWriter.Write(template);
            streamWriter.Flush();
            memoryStream.Position = 0;
            return Parse(memoryStream);
        }
    }
}
