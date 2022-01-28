internal class CleverPlayer : IPlayer
{
    private Random _randomizer = new Random((int)DateTime.Now.Ticks);
    
    public int SelectDoor()
    {
        return _randomizer.Next(3);
    }

    public bool SwitchDoor()
    {
        return true;
    }
}