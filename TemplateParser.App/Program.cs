﻿// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using TemplateParser.Contracts;
using TemplateParser.Defaults;

var cfg = new DefaultConfig(new ConfigurationBuilder()
    .AddJsonFile("config.json").Build());

Console.WriteLine(cfg);

var config = new DefaultConsoleConfig(new ConfigurationBuilder()
    .AddCommandLine(args, cfg.Options?.Commands) .Build());


Console.WriteLine(config);

foreach(var (key, value) in config.ValueDictionary)
{
    Console.WriteLine("{0}:{1}", key, value);
}

string? directoryName;
IEnumerable<ITemplate> templates = Array.Empty<ITemplate>();
Console.WriteLine(AppContext.BaseDirectory);
if (!string.IsNullOrWhiteSpace(config.Input) && !string.IsNullOrWhiteSpace(directoryName = Path.GetDirectoryName(config.Input)))
{
    Console.WriteLine(directoryName);
    if(string.IsNullOrEmpty(directoryName))
    {
        directoryName = AppContext.BaseDirectory;
    }
    else if (directoryName.Equals("."))
    {
        directoryName = Environment.CurrentDirectory;
    }

    using var fileProvider = new PhysicalFileProvider(directoryName);
    var file = fileProvider.GetFileInfo(config.Input);
    if (!file.Exists)
    {
        return;
    }
    using var readStream = file.CreateReadStream();
    templates = TemplateParser.TemplateParser.Parse(readStream);
}

foreach (var template in templates)
{
    Console.WriteLine(template);
    //foreach(var (k,v) in template.Variables)
    //{
    //    Console.WriteLine("{0}, {1}", k, v);
    //}
}