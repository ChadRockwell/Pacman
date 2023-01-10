namespace Pacman.Tests;

using System.IO;
using System.Threading.Tasks;

using Pacman.Engine;

using Xunit;

public sealed class PacmanTests
{
    [Theory]
    [InlineData("generic.txt", 6, 1, 27)]
    [InlineData("edge.txt", -1, -1, 0)]
    [InlineData("runtime.txt", 2142, 147, 148)]
    public async Task PacmanEngine_TestsAsync(string file, int x, int y, int coins)
    {
        using var reader = File.OpenText(file);
        var config = await PacmanConfigLoader.Load(reader.BaseStream);

        // execute engine
        PacmanEngine engine = new(
            config.Columns,
            config.Rows,
            config.Commands,
            config.InitialPosition,
            config.Walls);

        var (FinalPosition, CountOfCoins) = engine.Execute();

        Assert.Equal(x, FinalPosition.X);
        Assert.Equal(y, FinalPosition.Y);
        Assert.Equal(coins, CountOfCoins);
    }
}
