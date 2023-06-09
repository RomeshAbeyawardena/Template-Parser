﻿// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System.Collections.Concurrent;
using TemplateParser;
using TemplateParser.Contracts;
using TemplateParser.Defaults;

var cfg = new DefaultConfig(new ConfigurationBuilder()
    .AddJsonFile("config.json").Build());

var config = new DefaultConsoleConfig(new ConfigurationBuilder()
    .AddCommandLine(args, cfg.Options?.Commands) .Build());

string? directoryName;
IEnumerable<ITemplate> templates = Array.Empty<ITemplate>();
Console.WriteLine(AppContext.BaseDirectory);
var globalVariables = new DefaultGlobalVariables("$");
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
    templates = TemplateParserHelper.ParseFromFile(fileProvider, config.Input,
        globalVariables: globalVariables, config: cfg);
}

if (!config.Test.GetValueOrDefault())
{
    using var templateExecutor = new DefaultTemplateExecutor(Template.Processors, globalVariables);
    await templateExecutor.Execute(templates, CancellationToken.None);

    foreach (var (key, value) in globalVariables)
    {
        Console.WriteLine("{0}: {1}", key, value);
    }
}