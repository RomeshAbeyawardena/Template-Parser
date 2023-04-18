// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System.Collections.Concurrent;
using TemplateParser;
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
var globalVariables = new ConcurrentDictionary<string, string>();
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
        globalVariables: globalVariables);
}

var templateExecutor = new DefaultTemplateExecutor(Template.Processors);
await templateExecutor.Execute(templates, CancellationToken.None);

foreach(var (key, value) in globalVariables)
{
    Console.WriteLine("{0}: {1}",key, value);
}