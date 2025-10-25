using Blockchain.Api.BlockchainAgg.Domain.Entities;

using TheNoobs.Results;

namespace Blockchain.Api.BlockchainAgg.Domain.Abstractions;

public interface IListBlocks
{
    public interface IHandler
    {
        ValueTask<Result<IEnumerable<IBlockView>>> HandleAsync(CancellationToken cancellationToken);
    }
}