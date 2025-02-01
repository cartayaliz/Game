using System;
using System.Collections;
using System.Security.AccessControl;
using Spectre.Console.Rendering;

namespace Logic
{
    public class GameBuilder
    {
        public static void ExpandR(bool[,] Obs, bool[,] Mark, int i, int j)
        {
            Mark[i, j] = true;
            int[] dx = { 1, -1, 0, 0 };
            int[] dy = { 0, 0, 1, -1 };

            for (int k = 0; k < dx.Length; k++)
            {
                int ni = i + dx[k];
                int nj = j + dy[k];

                if (ni >= 0 && nj >= 0 && ni < Obs.GetLength(0) && nj < Obs.GetLength(1) && !Mark[ni, nj] && !Obs[ni, nj])
                {
                    ExpandR(Obs, Mark, ni, nj);
                }
            }

        }

        public static bool[,] Expande(bool[,] Obs, int i, int j)
        {
            bool[,] Mark = new bool[Obs.GetLength(0), Obs.GetLength(1)];

            ExpandR(Obs, Mark, i, j);

            return Mark;

        }

        public static bool CanPutObs(bool[,] Obs, int mi, int mj, List<(int, int)> players)
        {
            var mark = Expande(Obs, mi, mj);

            foreach (var p in players)
            {
                if (!mark[p.Item1, p.Item2]) return false;
            }
            return true;

        }



        public static List<(int, int)[]> Generate(int playerCount, int n, int specialCeldCount, int obsCount)
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

            var celdConected = new List<(int, int)>();

            // create random player position
            var players = new (int, int)[playerCount];

            for (int i = 0; i < playerCount; i++)
            {
                players[i] = array[idx];
                celdConected.Add(array[idx]);
                idx++;
            }

            // Create random special celd position
            var celds = new (int, int)[specialCeldCount];
            for (int i = 0; i < specialCeldCount; i++)
            {
                celds[i] = array[idx];
                celdConected.Add(array[idx]);
                idx++;
            }



            solve.Add(players);
            solve.Add(celds);
            celdConected.Add(array[idx]);
            var meta = array[idx++];
            solve.Add(new (int, int)[] { meta });


            // Create obs

            bool[,] obs = new bool[n, n];
            List<(int, int)> obsPos = new List<(int, int)>();

            while (obsCount > 0 && idx < array.Length)
            {
                var pos = array[idx++];

                obs[pos.Item1, pos.Item2] = true;

                if (CanPutObs(obs, meta.Item1, meta.Item2, celdConected))
                {
                    obsCount--;
                    obsPos.Add(pos);
                }
                else
                {
                    obs[pos.Item1, pos.Item2] = false;
                }
            }

            solve.Add(obsPos.ToArray());


            return solve;


        }




        public static GameCenter CreateGame(IVisual visual, Player[] players, int n)
        {

            Board board = new Board(n);

            var generatePos = Generate(players.Length, n, (n * n) / 10, (n * n) / 5);

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
                var rd = Random.Shared.Next(0, 4);
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

            // Create Position of Obs
            var obsPos = generatePos[3];
            for (int i = 0; i < obsPos.Length; i++)
            {
                var item = obsPos[i];
                board.matrix[item.Item1, item.Item2] = new CellObs(item.Item1, item.Item2);
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