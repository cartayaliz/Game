using Logic;

Player player1 = new Player('b', 1, 1, 1);
Player player2 = new Player('a', 2, 2, 1);


Board board = new Board(5);

board.setPlayer(0, 4, player1);

board.setPlayer(4, 3, player2);

board.matrix[0, 0] = new CellObs(0, 0);
board.matrix[4, 4] = new CellWinn(4, 4);
board.matrix[3, 3] = new CellSpeed1(3, 3);
board.matrix[2, 2] = new CellSpeed2(2, 2);
board.matrix[1, 3] = new CellVision1(1, 3);
board.matrix[1, 1] = new CellBridge(1, 1);


BasicConsoleVisual game = new BasicConsoleVisual();

GameCenter gc = new GameCenter(board, new List<Player> { player1, player2 }, game);

game.Play(gc);