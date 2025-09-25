using System.Collections.ObjectModel;

using Blockchain.ConsoleApp;

using TheNoobs.Results;
using TheNoobs.Results.Extensions;
using TheNoobs.Results.Types;

using Void = TheNoobs.Results.Types.Void;

public class BlockChain
{
    private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);
    private readonly ISet<Block> _blocks = new HashSet<Block>();
    public const char PREFIX_PROVE_OF_WORK = '0';
    
    private BlockChain(int difficulty, Block genesisBlock)
    {
        Difficulty = difficulty;
        _blocks.Add(genesisBlock);
    }
    
    public char PrefixProveOfWork { get; } = PREFIX_PROVE_OF_WORK;
    public int Difficulty { get; }
    public IReadOnlySet<Block> Blocks => new ReadOnlySet<Block>(_blocks);
    
    public static Result<BlockChain> Create(int difficulty)
    {
        return CreateGenesisBlock()
            .Bind<Block, BlockChain>(x => new BlockChain(difficulty, x));
    }

    public Result<BlockBody> CreateBody(BlockData data)
    {
        _semaphoreSlim.Wait();
        try
        {
            return Last()
                .Bind(x => BlockBody.Create(
                    x.Value.Body.Sequence + 1,
                    data,
                    x.Value.Header.Hash));
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    public Result<BlockMining> CreateBlockMining(BlockBody body)
    {
        return BlockMining.Create(this, body);
    }

    public Result<Block> AddBlock(Block block)
    {
        _semaphoreSlim.Wait();
        try
        {
            var last = Last();
            if (!last.IsSuccess)
            {
                return last.Fail;
            }

            if (last.Value.Header.Hash != block.Body.PreviousHash)
            {
                return new InvalidInputFail("Invalid block");
            }

            return ProveOfWork.Create(block.Body, block.Header.Nonce)
                .Bind(x => x.Value.IsValid(this))
                .Bind<Void, Block>(_ =>
                {
                    _blocks.Add(block);
                    return block;
                });
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    private Result<Block> Last()
    {
        var last = _blocks.LastOrDefault();

        if (last is null)
        {
            return new NotFoundFail("Last block not found");
        }
        
        return last;
    }

    private static Result<Block> CreateGenesisBlock()
    {
        return BlockBody.Create(0, new BlockData(), string.Empty)
            .Bind(x => BlockHeader.Create(0, x))
            .Bind(x => Block.Create(x, x.GetValue<BlockBody>()));
    }
}