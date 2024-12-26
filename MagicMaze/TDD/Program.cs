using Xunit;
using Logic;

// DOC for Xunit Assert = https://github.com/xunit/xunit/tree/master/test/test.xunit.assert/Asserts
public class LogicTest
{

    [Fact]
    public void ProjectInit()
    {

        var gameCenter = new GameCenter(5);
        Assert.Equal(5, gameCenter.number_of_players);
        Assert.NotEqual(4, gameCenter.number_of_players);

    }

    [Fact]
    public void ProjectSave()
    {

        var gameCenter = new GameCenter(5);
        Assert.NotNull(gameCenter);
    }
}
