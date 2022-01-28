

using Akka.Actor;

public class Game : ReceiveActor
{
    private readonly TextWriter _output;
    private readonly Random _randomizer;

    public Game(TextWriter output)
    {
        _randomizer = new Random((int)DateTime.Now.Ticks);
        _output = output;
        Receive<Run>( r => DoRun(r.GameNumber, r.PlayerType) );
    }

    private void DoRun(int gameNumber, PlayerType playerType)
    {
//        _output.WriteLine($"Game {gameNumber} starting...");
        
        // Setup doors and place the treasure behind one of them
        var doorWithTreasure = _randomizer.Next(3); // Point a random door [0;2] behind which we place the treasure
        
        // Add one player
        IPlayer player = (playerType == PlayerType.Stubborn) ? new StubbornPlayer() : new CleverPlayer();

        // Ask player to pick a door
        var selectedDoor = player.SelectDoor();

        var openDoor = PickFirstFreeDoor(selectedDoor, doorWithTreasure);
        var alternativeDoor = PickFirstFreeDoor(selectedDoor, openDoor);
        
        // Offer player to switch
        if (player.SwitchDoor())
        {
            // _output.WriteLine($"Switch from door {selectedDoor} to door {alternativeDoor} - treasure behind door {doorWithTreasure}");
            selectedDoor = alternativeDoor;
        }
        
        // Open the door, which the player has finally picked
        // Tell result to sender
        Sender.Tell(new Finished(selectedDoor == doorWithTreasure));
    }

    private int PickFirstFreeDoor(int taken1, int taken2)
    {
        return DoorFinder.FindFree(taken1,taken2);
    }

    public record Run(int GameNumber, PlayerType PlayerType)
    {
    }
    public record Finished(bool PlayerWonThePrice) {}

    public enum PlayerType
    {
        Stubborn,   // This player type stays on him first choice no matter what
        Clever // This player type switches choice on request
    }
}