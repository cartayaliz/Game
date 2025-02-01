using System;
using System.Collections;

namespace Logic
{
    public class GameBuilder
    {

        public static List<(int, int)[]> Generate(int playerCount, int n, int specialCeldCount)
        {
            var solve = new List<(int, int)[]>();
            List<(int, int)> posiblePoint = new List<(int, int)>();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    posiblePoint.Add((i, j));
                }
            }

            var array = posiblePoint.ToArray();
            Random.Shared.Shuffle(array);
            int idx = 0;

            // create random player position
            var players = new (int, int)[playerCount];

            for (int i = 0; i < playerCount; i++)
            {
                players[i] = array[idx];
                idx++;
            }

            // Create random special celd position
            var celds = new (int, int)[specialCeldCount];
            for (int i = 0; i < specialCeldCount; i++)
            {
                celds[i] = array[idx];
                idx++;
            }



            solve.Add(players);
            solve.Add(celds);
            solve.Add(new (int, int)[] { array[idx++] });


            return solve;


        }


        public static GameCenter CreateGame(IVisual visual, int n)
        {


            Player[] players = {
                new PlayerIntelligent(),
                new PlayerFast(),
                new PlayerExplorer(),
                new PlayerAstute(),
                new PlayerObserver(),
            };


            Board board = new Board(n);

            var generatePos = Generate(5, n, (n * n) / 10);

            //Create  PlayerPositions

            var playerPos = generatePos[0];

            for (int i = 0; i < players.Length; i++)
            {
                var item = playerPos[i];
                board.setPlayer(item.Item1, item.Item2, players[i]);
            }

            // Create Positions of SpecialCelds
            var celdPos = generatePos[1];
            for (int i = 0; i < celdPos.Length; i++)
            {
                Cell celd;
                var item = celdPos[i];
                var rd = Random.Shared.Next(0, 3);
                if (rd == 0) celd = new CellSpeed1(item.Item1, item.Item2);
                else if (rd == 1) celd = new CellSpeed2(item.Item1, item.Item2);
                else if (rd == 2) celd = new CellVision(item.Item1, item.Item2);
                else celd = new CellBridge(item.Item1, item.Item2);
                board.matrix[item.Item1, item.Item2] = celd;
            }

            // Create Position of WinnCeld
            var winPos = generatePos[2];
            for (int i = 0; i < winPos.Length; i++)
            {
                var item = winPos[i];
                board.matrix[item.Item1, item.Item2] = new CellWinn(item.Item1, item.Item2);
            }

            GameCenter gc = new GameCenter(board, new List<Player>(players), visual);

            return gc;

        }
        public static GameCenter TestGame(IVisual visual)
        {


            Player player1 = new PlayerIntelligent();
            Player player2 = new PlayerFast();
            Player player3 = new PlayerExplorer();
            Player player4 = new PlayerAstute();
            Player player5 = new PlayerObserver();


            Board board = new Board(10);

            board.setPlayer(0, 4, player1);

            board.setPlayer(4, 3, player2);
            board.setPlayer(2, 5, player3);
            board.setPlayer(4, 8, player4);
            board.setPlayer(5, 9, player5);

            board.matrix[0, 0] = new CellObs(0, 0);
            board.matrix[9, 9] = new CellWinn(9, 9);
            board.matrix[3, 3] = new CellSpeed1(3, 3);
            board.matrix[2, 2] = new CellSpeed2(2, 2);
            board.matrix[1, 3] = new CellVision(1, 3);
            board.matrix[1, 1] = new CellBridge(1, 1);


            GameCenter gc = new GameCenter(board, new List<Player> { player1, player2, player3, player4, player5 }, visual);

            return gc;

        }
    }

}