public static class DoorFinder
{
    public static int FindFree(int taken1, int taken2)
    {
        var doorFlags1 = 1 << taken1;
        var doorFlags2 = 1 << taken2;

        var occupied = doorFlags1 | doorFlags2;
        var free = (occupied^7) & 7;

        if ((free & 1) == 1)
        {
            return 0;
        }
        
        if ((free & 2) == 2)
        {
            return 1;
        }
        
        return 2;
    }
}