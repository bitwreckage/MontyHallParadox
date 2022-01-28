using Akka.Actor;

namespace MontyHallParadox;

/// Responsible for generating 
///
public class GameGod : ReceiveActor
{
    private const int ParallelGames = 100;
    
    private readonly TextWriter _output;
    private IActorRef[] _gameHosts = new IActorRef[ParallelGames];
    private int _gamesRunning = 0;
    private IActorRef _waitingForTheEnd = ActorRefs.Nobody;
    private int _gamesWon;
    private int _gamesLost;

    public GameGod(TextWriter output)
    {
        _output = output;
        
        // Setup a fixed number of actors to run games in parallel...
        var gameProps = Props.Create<Game>(() => new Game(_output));
        for (int n = 0; n < ParallelGames; n++)
        {
            _gameHosts[n] = Context.ActorOf(gameProps);
        }
        
        Receive<StartGames>( sg => DoStartGames(sg.NumberOfGames, sg.playerType) );
        Receive<Game.Finished>( gf => OnGameFinished(gf.PlayerWonThePrice) );
    }

    private void OnGameFinished(bool playerWonThePrice)
    {
        if (playerWonThePrice)
        {
            _gamesWon++;
        }
        else
        {
            _gamesLost++;
        }
        
        _gamesRunning--;
        if (_gamesRunning == 0)
        {
            _waitingForTheEnd.Tell(new GamesEnded(_gamesWon, _gamesLost));
        }
    }

    private void DoStartGames(int numberOfGames, Game.PlayerType playerType)
    {
        _output.WriteLine("GameGod starting games...");

        _waitingForTheEnd = Sender;

        // Run the requested number of games distributed on the fixed array of game actors...
        foreach(var gameNumber in Enumerable.Range(0,numberOfGames))
        {
            _gameHosts[gameNumber%ParallelGames].Tell( new Game.Run(gameNumber, playerType) );
        }
        _gamesRunning = numberOfGames;
    }
    
    public record StartGames(int NumberOfGames, Game.PlayerType playerType) {}
    public record GamesEnded(int GamesWon, int GamesLost) {}
}