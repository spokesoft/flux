using Flux.Console.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

var services = new ServiceCollection();
var registrar = new TypeRegistrar(services);
var app = new CommandApp(registrar);

app.Configure(builder =>
{
    builder.SetApplicationName("flux");
    builder.SetApplicationVersion("0.1.0-alpha.1");
});

return await app.RunAsync(args);