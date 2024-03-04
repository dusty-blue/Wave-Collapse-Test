using Assets.Scripts.WFC;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TileUnitTest
{
    // A Test behaves as an ordinary method
    public class TestTile: WFCTile
    {
        public TestTile(NeighbourState[] states, State starting) : base(states, starting)
        {
        }
    }

    private Dictionary<String, State> SetupStates(List<String> stateNames)
    {

        StateLoader loader = new("Assets/Scripts/WFC/States/Test");
        Dictionary<String, State> stateDic = loader.LoadStates(stateNames.ToArray());

        return stateDic;
    }
    [Test]
    public void TileUnitTestSimplePasses()
    {
        // Use the Assert class to test conditions

        List<String> stateNames = new()
        {
            "NGrass",
            "NShrubs",
            "NTree"
        };
        Dictionary<String, State> stateDic = SetupStates(stateNames);

        NeighbourState testNeighbour = ScriptableObject.CreateInstance<NeighbourState>();
        testNeighbour.m_state = stateDic["NTree"];

        WFCTile grassTile = new WFCTile(stateDic["NTree"].m_allowedNeighbours, stateDic["NGrass"]); //[CreateAssetMenu(fileName = "Data", menuName = "WFC/State", order = 1)]

        WFCTile testTile = new WFCTile(new[]{ testNeighbour}, stateDic["NTree"]);
        Assert.AreEqual(-0.2f, testTile.getEntropy());
        Assert.AreEqual(0.7235767f, grassTile.getEntropy(), 0.000001f);

        //fluxTile.updateStates(new[] { shrubs, trees });
        Assert.Contains(stateDic["NTree"].m_allowedNeighbours[0],grassTile.possibleStates);
        Assert.Contains(stateDic["NTree"].m_allowedNeighbours[1], grassTile.possibleStates);
        Assert.AreEqual(2, grassTile.possibleStates.Length);

        testTile.updateStates(new[] { ScriptableObject.CreateInstance<NeighbourState>() });
        Assert.AreEqual(0, testTile.possibleStates.Length);
        grassTile.SelectCurrentState();
        Assert.AreNotEqual(testTile, grassTile.currentState);
            
       }

    [Test]
    public void TryUpdateTests()
    {
        List<String> stateNames = new()
        {
            "NGrass",
            "NShrubs",
            "NTree",
            "Init"
        };
        Dictionary<String, State> stateDic = SetupStates(stateNames);

        NeighbourState testNeighbour = ScriptableObject.CreateInstance<NeighbourState>();
        testNeighbour.m_state = stateDic["NTree"];

        WFCTile initTile = new(stateDic["Init"]);
        WFCTile grassTile = new (stateDic["NGrass"]);
        WFCTile shrubTile = new(stateDic["NShrubs"]);

        WFCTile impossibleTile = new(new NeighbourState[0], stateDic["Init"]);

        Assert.IsTrue(grassTile.TryUpdateStates(grassTile.possibleStates));
        Assert.IsFalse(initTile.TryUpdateStates(grassTile.possibleStates));
        Assert.IsTrue(initTile.TryUpdateStates(grassTile.possibleStates));
        Assert.AreEqual(float.MinValue, impossibleTile.getEntropy());
        Assert.IsTrue(impossibleTile.isImpossible);

        shrubTile.TryUpdateStates(new NeighbourState[0]);
        Assert.IsTrue(shrubTile.isImpossible);
        Assert.AreEqual(float.MinValue, shrubTile.getEntropy());

        WFCTile startTile = new(stateDic["Init"]);

    }


}
