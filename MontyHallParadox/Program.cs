// See https://aka.ms/new-console-template for more information


using Akka.Actor;

namespace MontyHallParadox;

public static class Program
{
    public static async Task<int> Main(params string[] args)
    {
        Console.WriteLine("Hello, Monty!");
        
        var argCount = args.Length;
        
        var playerType = Game.PlayerType.Stubborn;
        if (argCount > 0)
        {
            if (args[0].ToLower().Contains("clever"))
            {
                playerType = Game.PlayerType.Clever;
            }
        }


        var numberOfGames = 100;
        if (argCount > 1)
        {
            int.TryParse(args[1], out numberOfGames);
        }

        var actorSystem = ActorSystem.Create("MontyHall");

        var godProps = Props.Create<GameGod>(() => new GameGod(Console.Out));
        var gameGod = actorSystem.ActorOf(godProps);
        var result = await gameGod.Ask<GameGod.GamesEnded>( new GameGod.StartGames(numberOfGames, playerType) );

        var gamesTotal = result.GamesWon + result.GamesLost;
        Console.WriteLine($"  Total games: {gamesTotal}");
        Console.WriteLine($"  Games won: {result.GamesWon} ({100.0f*result.GamesWon/gamesTotal}%)");
        Console.WriteLine($"  Games lost: {result.GamesLost} ({100.0f*result.GamesLost/gamesTotal}%)");

        return 0;
    }
}