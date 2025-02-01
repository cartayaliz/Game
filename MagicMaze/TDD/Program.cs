using Xunit;
using Logic;

// DOC for Xunit Assert = https://github.com/xunit/xunit/tree/master/test/test.xunit.assert/Asserts
public class LogicTest
{

    [Fact]
    public void ProjectInit()
    {

        BasicConsoleVisual game = new BasicConsoleVisual();
        var gc = GameBuilder.TestGame(game);

        Assert.Equal(5, gc.players.Count);
        Assert.NotEqual(4, gc.players.Count);

    }

    [Fact]
    public void ProjectSave()
    {

        BasicConsoleVisual game = new BasicConsoleVisual();
        var gc = GameBuilder.TestGame(game);

        Assert.NotNull(gc);
    }
}
