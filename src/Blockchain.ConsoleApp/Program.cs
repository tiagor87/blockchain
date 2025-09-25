// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Text.Json;

using TheNoobs.Results.Extensions;

using Void = TheNoobs.Results.Types.Void;

var blockChain = BlockChain.Create(4)
    .Value;

var stopwatch = new Stopwatch();
stopwatch.Start();
for (var i = 1; i <= 200; i++)
{
    var body = blockChain.CreateBody(new BlockData())
        .Bind(x => blockChain.CreateBlockMining(x.Value))
        .Bind(x => x.Value.Mine())
        .Bind(x => blockChain.AddBlock(x.Value))
        .Tap(x =>
        {
            Console.WriteLine(JsonSerializer.Serialize(x.Value));
            return Void.Value;
        });
}
stopwatch.Stop();
Console.WriteLine(stopwatch.Elapsed);
Console.ReadLine();

