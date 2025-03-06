using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.WFC;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using static UnityEngine.UI.Image;
using static Assets.Scripts.EditorTests.UnitTestUtility;

public class WFC_UnitTests
{
    public class TestMatrix : WFC_Matrix
    {
        public TestMatrix(BoundsInt bounds, WFCTile defaultTile, float EntropyT, List<WFCSocket> allSockets) : base(bounds, defaultTile, EntropyT, allSockets) { }

        public void TestSize(Vector3Int size)
        {
            Assert.AreEqual(size.x, TileMatrix.GetLength(0));
            Assert.AreEqual(size.y, TileMatrix.GetLength(1));
        }

        public void TestIsInBounds(int i, int j, bool expected)
        {
            Assert.AreEqual(expected, IsInBounds(i, j));
        }

        public void TestNeighboursRound(int i, int j, int r,int noNeighbours)
        {
            Assert.AreEqual(noNeighbours, getNeighbourIndicesRound(i, j, r).Count);
        }

        public void TestNeighbours(int i, int j, int r, int noNeighbours)
        {
            Assert.AreEqual(noNeighbours, getNeighbourIndices(i, j, r).Count);
        }

        public void TestforState(State s)
        {
            foreach(WFCTile t in TileMatrix)
            {
                Assert.AreEqual(s, t.currentState, $"expected: {s.name} actual:{t.currentState.name}");
            }
            
        }

        public void TestMissingState(State s)
        {
            foreach (WFCTile t in TileMatrix)
            {
                Assert.AreNotEqual(s, t.currentState, $"not this: {s.name} actual:{t.currentState.name}");
            }
        }
    }

    
    // A Test behaves as an ordinary method
    [Test]
    public void WFC_UnitTestsSimpleBound()
    {
        // Use the Assert class to test conditions
        Vector3Int pos = new Vector3Int(0, 0, 0);
        Vector3Int size = new Vector3Int(2, 2, 1);
        BoundsInt bounds = new BoundsInt(pos, size);
        List<String> folderList =  new()
        {
            "T-States"
        };
        LoadedObjects loadedScriptableObjects = SetupSidedStates(folderList);
        TestMatrix testM = new TestMatrix(bounds,new WFCTile(loadedScriptableObjects.allStates.First(s => s.name=="TInit")),2f, loadedScriptableObjects.allSockets.ToList());
        testM.TestSize(size);
                
    }

    [Test]
    public void WFC_UnitTestsNegativePos()
    {
        // Use the Assert class to test conditions
        Vector3Int pos = new Vector3Int(-10, -10, -10);
        Vector3Int size = new Vector3Int(2, 5, 1);
        BoundsInt bounds = new BoundsInt(pos, size);
        List<String> folderList = new()
        {
            "T-States"
        };
        LoadedObjects loadedScriptableObjects = SetupSidedStates(folderList);
        TestMatrix testM = new TestMatrix(bounds, new WFCTile(loadedScriptableObjects.allStates.First(s => s.name == "TInit")), 2f, loadedScriptableObjects.allSockets.ToList());

        testM.TestSize(size);

    }

    [Test]
    public void IsInBoundsTest()
    {
        Vector3Int pos = new Vector3Int(0, 0, 0);
        Vector3Int size = new Vector3Int(2, 2, 1);
        BoundsInt bounds = new BoundsInt(pos, size);
        List<String> folderList = new()
        {
            "T-States"
        };
        LoadedObjects loadedScriptableObjects = SetupSidedStates(folderList);
        TestMatrix testM = new TestMatrix(bounds, new WFCTile(loadedScriptableObjects.allStates.First(s => s.name == "TInit")), 2f, loadedScriptableObjects.allSockets.ToList());

        testM.TestIsInBounds(0, 0, true);
        testM.TestIsInBounds(1, 1, true);
        testM.TestIsInBounds(-1, 0, false);
        testM.TestIsInBounds(0, -1, false);
        testM.TestIsInBounds(5, 0, false);
        testM.TestIsInBounds(0, 6, false);
    }

    [Test]
    public void getNeighboursTest()
    {
        Vector3Int pos = new Vector3Int(0, 0, 0);
        Vector3Int size = new Vector3Int(2, 2, 1);
        BoundsInt bounds = new BoundsInt(pos, size);
        List<String> folderList = new()
        {
            "T-States"
        };
        LoadedObjects loadedScriptableObjects = SetupSidedStates(folderList);
        TestMatrix testM = new TestMatrix(bounds, new WFCTile(loadedScriptableObjects.allStates.First(s => s.name == "TInit")), 2f, loadedScriptableObjects.allSockets.ToList());


        testM.TestNeighbours(0, 0,1, 3);
        testM.TestNeighbours(0, 0, 2, 3);

        Vector3Int pos2 = new Vector3Int(0, 0, 0);
        Vector3Int size2 = new Vector3Int(10, 10, 1);
        BoundsInt bounds2 = new BoundsInt(pos2, size2);
        //TODO replace empty List iwth allsockets list
        TestMatrix testM2 = new TestMatrix(bounds2, new WFCTile(loadedScriptableObjects.allStates.First(s => s.name == "TGrass")), 0.3f, loadedScriptableObjects.allSockets.ToList());
        testM2.TestNeighbours(0, 0, 1, 3);
        testM2.TestNeighbours(1, 1, 1, 8);
        testM2.TestNeighbours(0, 2, 1, 5);
        testM2.TestNeighbours(9, 9, 1, 3);
        testM2.TestNeighbours(4, 4, 2, 24);

        testM2.TestNeighboursRound(0, 0, 1, 2);
        testM2.TestNeighboursRound(1, 1, 1, 4);
        testM2.TestNeighboursRound(0, 2, 1, 3);
        testM2.TestNeighboursRound(9, 9, 1, 2);
        testM2.TestNeighboursRound(4, 4, 2, 12);
    }

    [Test]
    public void SetupTest()
    {
        Vector3Int pos = new Vector3Int(0, 0, 0);
        Vector3Int size = new Vector3Int(2, 2, 1);
        BoundsInt bounds = new BoundsInt(pos, size);
        List<String> folderList = new()
        {
            "T-States"
        };
        LoadedObjects loadedScriptableObjects = SetupSidedStates(folderList);
        TestMatrix testM = new TestMatrix(bounds, new WFCTile(loadedScriptableObjects.allStates.First(s => s.name == "TInit")), 2f, loadedScriptableObjects.allSockets.ToList());

        WFCTile origin = testM.GetTile(new Vector3Int(0,0,0));

        Assert.AreEqual(loadedScriptableObjects.allStates.First(s => s.name== "TInit"), origin.currentState);
    }

    [Test]
    public void UpdateTest()
    {
        //Vector3Int pos = new Vector3Int(0, 0, 0);
        //Vector3Int size = new Vector3Int(2, 2, 1);
        //BoundsInt bounds = new BoundsInt(pos, size);
        //List<State> states = SetupStates();
        ////State updateState = new State("changeMe", 0.0f);
        ////State finalState = new State("I'm here", 1f);
        ////updateState.m_allowedNeighbours = new[] { new NeighbourState(finalState,1f), new NeighbourState(updateState,1f)};
        ////finalState.m_allowedNeighbours = new[] { new NeighbourState(finalState,1f) };

        //WFCTile changeTile = new WFCTile(updateState);

        //TestMatrix testM = new TestMatrix(bounds, changeTile,2f);

        //testM.UpdateTiles();
        ////Assert.AreEqual(finalState, testM.GetTile(new Vector3Int(1,0, 0)).currentState,$"tile was state {testM.GetTile(new Vector3Int(0, 0, 0)).currentState.m_name}");
        //testM.UpdateTiles();
        //testM.UpdateTiles();
        //testM.UpdateTiles();
        //testM.TestMissingState(updateState);
    }
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    //[UnityTest]
    public IEnumerator WFC_UnitTestsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
