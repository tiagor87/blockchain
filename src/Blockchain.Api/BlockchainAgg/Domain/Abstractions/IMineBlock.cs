using Blockchain.Api.BlockchainAgg.Domain.Entities;

using TheNoobs.Results;

namespace Blockchain.Api.BlockchainAgg.Domain.Abstractions;

public interface IMineBlock
{
    public interface IHandler
    {
        ValueTask<Result<IBlockView>> HandleAsync(CancellationToken cancellationToken);
    }
}