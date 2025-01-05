// See https://aka.ms/new-console-template for more information 
using Logic;
// GameCenter game = new GameCenter(5);
// Console.WriteLine("Hi logic {0} !!!", game.number_of_players);

Player player1 = new Player('b', 1);
Player player2 = new Player('a', 2);


Board board = new Board(5);

board.setPlayer(0, 4, player1);
board.setPlayer(4, 3, player2);

board.matrix[0, 0] = new CellObs(0, 0);
board.matrix[4, 4] = new CellWinn(4, 4);

GameCenter gc = new GameCenter(board, new List<Player> { player1, player2 });


System.Console.WriteLine(gc.ToString());

while (true)
{
    var key = Console.ReadKey(true);
    if (key.Key == ConsoleKey.Enter)
    {
        gc.nextTurn();
    }
    else if (key.Key == ConsoleKey.UpArrow)
    {
        gc.currentPlayerAction(0);
    }
    else if (key.Key == ConsoleKey.RightArrow)
    {
        gc.currentPlayerAction(1);
    }
    else if (key.Key == ConsoleKey.DownArrow)
    {
        gc.currentPlayerAction(2);
    }
    else if (key.Key == ConsoleKey.LeftArrow)
    {
        gc.currentPlayerAction(3);
    }
    else
    {
        break;
    }

    Console.Clear();
    System.Console.WriteLine(gc.ToString());

}