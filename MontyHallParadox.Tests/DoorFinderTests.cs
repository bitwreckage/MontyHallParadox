using NUnit.Framework;
using Shouldly;

namespace MontyHallParadox.Tests;

public class DoorFinderTests
{
    [SetUp]
    public void Setup()
    {
    }

    [TestCase( 0, 0, 1)]
    [TestCase( 1, 1, 0)]
    [TestCase( 0, 1, 2)]
    [TestCase( 2, 2, 0)]
    [TestCase( 0, 2, 1)]
    [TestCase( 1, 2, 0)]
    public void FindFree_ReturnsBitFlagForFreeDoor(int taken1, int taken2, int expectedResult)
    {
        DoorFinder.FindFree(taken1, taken2).ShouldBe(expectedResult);
    }
}