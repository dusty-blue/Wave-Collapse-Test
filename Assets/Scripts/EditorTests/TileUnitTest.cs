using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assets.Scripts.WFC;
using System;

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

        //abyss.m_allowedNeighbours = new[] { abyss };
        //grass.m_allowedNeighbours = new[] { grass, shrubs };
        //shrubs.m_allowedNeighbours = new[] { grass, shrubs, trees };
        //trees.m_allowedNeighbours = new[] { shrubs };
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
    

}
