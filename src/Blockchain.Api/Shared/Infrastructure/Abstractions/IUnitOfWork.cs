using TheNoobs.Results;

using Void = TheNoobs.Results.Types.Void;

namespace Blockchain.Api.Shared.Infrastructure.Abstractions;

public interface IUnitOfWork
{
    ValueTask<Result<Void>> CompleteAsync(CancellationToken cancellationToken);
}