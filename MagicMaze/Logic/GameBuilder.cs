using System;
using System.Collections;

namespace Logic
{
    public class GameBuilder
    {

        public static GameCenter TestGame(IVisual visual)
        {


            Player player1 = new PlayerIntelligent();
            // Player player2 = new PlayerFast();
            // Player player3 = new PlayerExplorer();


            Board board = new Board(10);

            board.setPlayer(0, 4, player1);

            // board.setPlayer(4, 3, player2);
            // board.setPlayer(2, 5, player3);

            board.matrix[0, 0] = new CellObs(0, 0);
            board.matrix[9, 9] = new CellWinn(9, 9);
            board.matrix[3, 3] = new CellSpeed1(3, 3);
            board.matrix[2, 2] = new CellSpeed2(2, 2);
            board.matrix[1, 3] = new CellVision(1, 3);
            board.matrix[1, 1] = new CellBridge(1, 1);


            GameCenter gc = new GameCenter(board, new List<Player> { player1 }, visual);

            return gc;

        }
    }

}