using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.WFC;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class WFC_UnitTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void WFC_UnitTestsSimplePasses()
    {
        // Use the Assert class to test conditions
                
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator WFC_UnitTestsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
