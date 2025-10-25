using System.Net;

using Blockchain.Api.BlockchainAgg.Domain.Abstractions;
using Blockchain.Api.Shared.Application.Abstractions;

using TheNoobs.Results.Extensions;

namespace Blockchain.Api.BlockchainAgg.Application;

using Microsoft.AspNetCore.Mvc;

public class BlockchainEndpoints : EndpointsBase
{
    public override void Setup(IServiceCollection services, IConfiguration configuration)
    {
        int difficult = configuration.GetValue<int>("Blockchain:Difficult");
        services.AddSingleton(new Domain.ValueObjects.Blockchain(difficult));
    }

    protected override void ConfigureEndpoints(IEndpointRouteBuilder builder)
    {
        builder.MapPost("/", AddAsync);
        builder.MapGet("/", GetAsync);
    }

    private static ValueTask<IResult> AddAsync(
        [FromServices] IMineBlock.IHandler handler,
        CancellationToken cancellationToken)
    {
        return handler.HandleAsync(cancellationToken)
            .MatchAsync(
                x => Success(x, HttpStatusCode.Created),
                Fail);
    }
    
    private static ValueTask<IResult> GetAsync(
        [FromServices] IListBlocks.IHandler handler,
        CancellationToken cancellationToken)
    {
        return handler.HandleAsync(cancellationToken)
            .MatchAsync(
                x => Success(x, HttpStatusCode.OK),
                Fail);
    }
}