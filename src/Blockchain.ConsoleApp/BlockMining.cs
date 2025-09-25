using System.Security.Cryptography;
using System.Text;

using TheNoobs.Results;
using TheNoobs.Results.Extensions;

namespace Blockchain.ConsoleApp;

public class BlockMining
{
    private readonly BlockChain _blockchain;
    private readonly BlockBody _body;

    private BlockMining(BlockChain blockchain, BlockBody body)
    {
        _blockchain = blockchain;
        _body = body;
    }
    
    public static Result<BlockMining> Create(BlockChain blockchain, BlockBody body)
    {
        return new BlockMining(blockchain, body);
    }

    public Result<Block> Mine()
    {
        var proveOfWorkPrefix = "".PadRight(_blockchain.Difficulty, _blockchain.PrefixProveOfWork);

        var nonce = -1;
        var hashBlock = new StringBuilder();
        var aux = new StringBuilder();
        bool verified;
        do
        {
            aux.Clear();
            hashBlock.Clear();
            nonce++;
            
            hashBlock.Append(_body.Serialize()
                .Bind<byte[], byte[]>(x => SHA256.HashData(x.Value))
                .Bind<byte[], string>(x => Convert.ToHexString(x.Value))
                .Value);
            aux
                .Append(hashBlock.ToString())
                .Append(nonce);
             
            var powHash = SHA256.HashData(Encoding.UTF8.GetBytes(aux.ToString()));
            verified = Convert.ToHexString(powHash).StartsWith(proveOfWorkPrefix);
        } while (!verified);

        return BlockHeader.Create(nonce, _body)
            .Bind(x => Block.Create(x, _body));
    }
}