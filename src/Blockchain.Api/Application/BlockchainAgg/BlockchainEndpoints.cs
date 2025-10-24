using Microsoft.AspNetCore.Mvc;

using TheNoobs.DependencyInjection.Extensions.Modules.Abstractions;

namespace Blockchain.Api.Application.BlockchainAgg;

public class BlockchainEndpoints : IServiceModuleSetup, IApplicationModuleSetup
{
    public void Setup(IServiceCollection services, IConfiguration configuration)
    {
        const int difficult = 4;
        services.AddSingleton(new Domain.BlockchainAgg.Entities.Blockchain(difficult));
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

    private static IResult Add([FromServices] Domain.BlockchainAgg.Entities.Blockchain blockchain)
    {
        var block = blockchain.Create();
        return Results.Created($"/{block.Hash}", block.ToView());
    }
    
    private static IResult Get([FromServices] Domain.BlockchainAgg.Entities.Blockchain blockchain)
    {
        return Results.Ok(blockchain.ToView());
    }
}