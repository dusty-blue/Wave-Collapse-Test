using Assets.Scripts.WFC;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts.EditorTests;

public class TileUnitTest
{
    // A Test behaves as an ordinary method
    public class TestTile: WFCTile
    {
        public TestTile(State[] states, State starting) : base(states, starting)
        {
        }
    }

    
    [Test]
    public void TileUnitTestSimplePasses()
    {
        // Use the Assert class to test conditions

        List<String> stateNames = new()
        { "Tile Unit Test Cases"};
        LoadedObjects loadedScriptableObjects = UnitTestUtility.SetupSidedStates(stateNames);

        WFCTile test01Tile = new WFCTile(loadedScriptableObjects.allStates,loadedScriptableObjects.allStates.First(s=> s.name=="T-Test01")); 

        WFCTile test02Tile = new WFCTile(new State[0], loadedScriptableObjects.allStates.First(s => s.name == "T-Test02"));
        Assert.AreEqual(float.MinValue, test02Tile.getEntropy());
        Assert.AreEqual(0f, test01Tile.getEntropy(), 0.000001f);

        Assert.AreEqual(2, test01Tile.possibleStates.Length);

        Assert.AreEqual(0, test02Tile.possibleStates.Length);
        test01Tile.SelectCurrentState();
        Assert.AreNotEqual(test02Tile, test01Tile.currentState);
            
       }

    [Test]
    public void TryUpdateTests()
    {
        List<String> stateNames = new()
            { "Tile Unit Test Cases"};
        LoadedObjects loadedScriptableObjects = UnitTestUtility.SetupSidedStates(stateNames);

        WFCTile test01Tile = new WFCTile(loadedScriptableObjects.allStates, loadedScriptableObjects.allStates.First(s => s.name == "T-Test01"));

        WFCTile test02Tile = new WFCTile(loadedScriptableObjects.allStates, loadedScriptableObjects.allStates.First(s => s.name == "T-Test02"));

        WFCSocket socket0101 = loadedScriptableObjects.allSockets.First(s => s.name == "01-01 Socket");
        WFCSocket socket0102 = loadedScriptableObjects.allSockets.First(s => s.name == "01-02 Socket");

        WFCSocket socket0202 = loadedScriptableObjects.allSockets.First(s => s.name == "02-02 Socket");
        WFCSocket[][] testSocketArray = new WFCSocket[4][];
        for (int i = 0; i < testSocketArray.Length; i++)
        {
            testSocketArray[i] = new WFCSocket[1];
            testSocketArray[i][0] = socket0102;
        }

        testSocketArray[0][0] = socket0202;
        test02Tile.updateStates(testSocketArray);
        Assert.AreEqual(1, test02Tile.possibleStates.Length);
        Assert.AreEqual(loadedScriptableObjects.allStates.First(s => s.name == "T-Test02"), test02Tile.possibleStates[0]);


    }


}
