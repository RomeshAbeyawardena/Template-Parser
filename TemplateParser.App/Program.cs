// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;
using TemplateParser.App;

var cfg = new Config(new ConfigurationBuilder()
    .AddJsonFile("config.json").Build());

Console.WriteLine(cfg);

var config = new ConsoleConfig(new ConfigurationBuilder()
    .AddCommandLine(args, cfg.Options?.Commands) .Build());


Console.WriteLine(config);

var templates = TemplateParser.App.TemplateParser.Parse(@"Define:MapProfile
#BEGIN TEMPLATE#
using Automapper;
namespace $featureNode;

public class MappingProfile : Profile
{
    public MappingProfile() 
    {
        var s = $CowGoes;
    }
}
#END TEMPLATE#

Path:$feature
File:DeleteRequest.cs
#BEGIN TEMPLATE#
using Mediatr;
namespace $app.Features.$feature;
public record DeleteRequest : IRequest<Models.$feature> 
{
}
#END TEMPLATE#
File:GetRequest.cs
#BEGIN TEMPLATE#
using Mediatr;
namespace $app.Features.$feature;
public record GetRequest : IRequest<Models.$feature> 
{
}
#END TEMPLATE#
File:PostRequest.cs
#BEGIN TEMPLATE#
using Mediatr;
namespace $app.Features.$feature;
public record PostRequest : IRequest<Models.$feature> 
{
}
#END TEMPLATE#
File:PutRequest.cs
#BEGIN TEMPLATE#
using Mediatr;
namespace $app.Features.$feature;
public record PutRequest : IRequest<Models.$feature> 
{
}
#END TEMPLATE#

Path:$feature.Core
File:Delete.cs
#BEGIN TEMPLATE#
using Mediatr;
namespace $app.Core.Features.$feature;
public class Delete : RequestHandler<DeleteRequest, Models.$feature> 
{
}
#END TEMPLATE#
File:Get.cs
#BEGIN TEMPLATE#
using Mediatr;
namespace $app.Core.Features.$feature;
public class Get : RequestHandler<GetRequest, Models.$feature> 
{
}
#END TEMPLATE#
File:MappingProfile
#BEGIN TEMPLATE#
##Use:MapProfile
#Set:$featureNode=$app.Core.Features.$feature; 
#Set:$CowGoes=Moo
#END TEMPLATE#
File:Post.cs
#BEGIN TEMPLATE#
using Mediatr;
namespace $app.Core.Features.$feature;
public class Post : RequestHandler<PostRequest, Models.$feature> 
{
}
#END TEMPLATE#
File:Put.cs
#BEGIN TEMPLATE#
using Mediatr;
namespace $app.Core.Features.$feature;
public class Put : RequestHandler<PutRequest, Models.$feature> 
{
}
#END TEMPLATE#");
foreach (var template in templates)
{
    Console.WriteLine(template);
    foreach(var (k,v) in template.Variables)
    {
        Console.WriteLine("{0}, {1}", k, v);
    }
}