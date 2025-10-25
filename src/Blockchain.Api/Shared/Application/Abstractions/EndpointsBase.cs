using System.Net;

using Blockchain.Api.BlockchainAgg.Domain.Entities;

using Microsoft.AspNetCore.Mvc;

using TheNoobs.DependencyInjection.Extensions.Modules.Abstractions;
using TheNoobs.Results;
using TheNoobs.Results.Abstractions;
using TheNoobs.Results.Types;

using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Blockchain.Api.Shared.Application.Abstractions;

public abstract class EndpointsBase : IServiceModuleSetup, IApplicationModuleSetup
{
    public virtual void Setup(IServiceCollection services, IConfiguration configuration)
    {
        // Do nothing
    }

    public void Setup(IApplicationBuilder appBuilder)
    {
        if (appBuilder is not IEndpointRouteBuilder endpointRouteBuilder)
        {
            throw new InvalidCastException("Invalid application builder.");
        }
        
        ConfigureEndpoints(endpointRouteBuilder);
    }
    
    protected abstract void ConfigureEndpoints(IEndpointRouteBuilder builder);
    
    protected static IResult Success(TheNoobs.Results.Abstractions.IResult result, HttpStatusCode statusCode)
    {
        if (result.IsSuccess)
        {
            return Results.Json(result.GetValue(), statusCode: (int)statusCode);
        }

        return Fail(result.Fail);
    }
    
    protected static IResult Fail<T>(T fail) where T : Fail
    {
        return fail switch
        {
            ValidationFail validationFail => Results.Problem(new ProblemDetails()
            {
                Title = fail.Message, Status = StatusCodes.Status400BadRequest, Detail = fail.Exception?.Message
            }),
            BadRequestFail badRequest => Results.Problem(new ProblemDetails()
            {
                Title = fail.Message, Status = StatusCodes.Status400BadRequest, Detail = fail.Exception?.Message
            }),
            NotFoundFail notFound => Results.Problem(new ProblemDetails()
            {
                Title = fail.Message, Status = StatusCodes.Status404NotFound, Detail = fail.Exception?.Message
            }),
            UnauthorizedFail unauthorized => Results.Problem(new ProblemDetails()
            {
                Title = fail.Message, Status = StatusCodes.Status401Unauthorized, Detail = fail.Exception?.Message
            }),
            ThirdPartyServiceErrorFail thirdPartyServiceError => Results.Problem(new ProblemDetails()
            {
                Title = fail.Message, Status = StatusCodes.Status424FailedDependency, Detail = fail.Exception?.Message
            }),
            _ => Results.Problem(new ProblemDetails()
            {
                Title = fail.Message, Status = (int)StatusCodes.Status500InternalServerError, Detail = fail.Exception?.Message
            })
        };
    }
}