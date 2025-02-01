using Logic;


SpectreConsoleVisual game = new SpectreConsoleVisual();

// var players = new Player[]{
//     new PlayerIntelligent(),
//     new PlayerFast(),
//     new PlayerExplorer(),
//     new PlayerAstute(),
//     new PlayerObserver(),
// };




var gc = GameBuilder.CreateGame(game, game.players.ToArray(), game.N);

game.Play(gc);

