using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;

public class PlayerPlayTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void PlayerPlayTestsSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator PlayerPlayTestsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }

    [UnityTest]
    public void PlayerMovementDiagonally()
    {

    }
}
