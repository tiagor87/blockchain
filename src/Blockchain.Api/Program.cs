using Blockchain.Api;

using TheNoobs.DependencyInjection.Extensions.Modules;

var builder = WebApplication.CreateBuilder(args);

builder.AddInjections(AssemblyMarker.Self);

var app = builder.Build();

app.UseInjections(AssemblyMarker.Self);

app.Run();