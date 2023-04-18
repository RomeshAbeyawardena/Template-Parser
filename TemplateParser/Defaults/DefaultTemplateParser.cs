﻿using TemplateParser.Contracts;

namespace TemplateParser.Defaults;

public class DefaultTemplateParser : ITemplateParser
{
    private readonly IDictionary<string, string> globalVariables;

    public DefaultTemplateParser(IDictionary<string, string> globalVariables)
    {
        this.globalVariables = globalVariables;
    }

    public IEnumerable<ITemplate> Parse(Stream stream)
    {
        using var textReader = new StreamReader(stream);
        var templateList = new List<ITemplate>();
        DefaultTemplate? currentTemplate = new(templateList, globalVariables);
        while (!textReader.EndOfStream)
        {
            var line = textReader.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            if (currentTemplate.ParseLine(line))
            {
                templateList.Add(currentTemplate);
                currentTemplate = new (templateList, globalVariables, currentTemplate);
            }
        }
        return templateList;
    }

    public IEnumerable<ITemplate> Parse(string template)
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
