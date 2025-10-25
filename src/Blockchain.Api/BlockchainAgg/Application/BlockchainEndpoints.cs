namespace Blockchain.Api.BlockchainAgg.Application;

using Microsoft.AspNetCore.Mvc;

using TheNoobs.DependencyInjection.Extensions.Modules.Abstractions;

public class BlockchainEndpoints : IServiceModuleSetup, IApplicationModuleSetup
{
    public void Setup(IServiceCollection services, IConfiguration configuration)
    {
        const int difficult = 4;
        services.AddSingleton(new Domain.ValueObjects.Blockchain(difficult));
    }
    
    public void Setup(IApplicationBuilder appBuilder)
    {
        if (appBuilder is not IEndpointRouteBuilder endpoints)
        {
            throw new InvalidCastException();
        }

        endpoints.MapPost("/", Add);
        endpoints.MapGet("/", Get);
    }

    private static IResult Add([FromServices] Domain.ValueObjects.Blockchain blockchain)
    {
        var block = blockchain.Create();
        return Results.Created($"/{block.Hash}", block.ToView());
    }
    
    private static IResult Get([FromServices] Domain.ValueObjects.Blockchain blockchain)
    {
        return Results.Ok(blockchain.ToView());
    }
}