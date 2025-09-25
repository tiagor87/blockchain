using TheNoobs.Results;
using TheNoobs.Results.Extensions;
using TheNoobs.Results.Types;

using Void = TheNoobs.Results.Types.Void;

namespace Blockchain.ConsoleApp;

public class ProveOfWork
{
    private ProveOfWork(string blockHash, long nonce, string hash)
    {
        BlockHash = blockHash;
        Nonce = nonce;
        Hash = hash;
    }
    
    public string BlockHash { get; }
    public long Nonce { get; }
    public string Hash { get; }

    public Result<Void> IsValid(BlockChain blockChain)
    {
        var initial = new string(blockChain.PrefixProveOfWork, blockChain.Difficulty);
        return Hash.StartsWith(initial)
            ? Void.Value
            : new InvalidInputFail("Invalid prove of work");
    }
    

    public static Result<ProveOfWork> Create(BlockBody blockBody, long nonce)
    {
        var hashBlock = blockBody
            .GetHash();

        if (!hashBlock.IsSuccess)
        {
            return hashBlock.Fail;
        }
            
        return hashBlock.Bind<string, string>(x => $"{x.Value}{nonce}")
            .Bind<string, string>(x => x.Value.GetHash())
            .Bind<string, ProveOfWork>(x => new ProveOfWork(hashBlock.Value, nonce, x));
    }
}