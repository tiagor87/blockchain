// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Text.Json;

var blockChain = BlockChain.Create(4)
    .Value;

var stopwatch = new Stopwatch();
stopwatch.Start();
for (var i = 1; i <= 100; i++)
{
    var body = blockChain.CreateBody(new BlockData())
        .Value;

    var mining = blockChain.CreateBlockMining(body)
        .Value;

    var block = mining.Mine()
        .Value;

    blockChain.AddBlock(block);
}
stopwatch.Stop();
foreach (var block in blockChain.Blocks)
{
    Console.WriteLine(JsonSerializer.Serialize(block));
}
Console.WriteLine(stopwatch.Elapsed);
Console.ReadLine();

