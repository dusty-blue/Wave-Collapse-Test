using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assets.Scripts.WFC;

public class TileUnitTest
{
    // A Test behaves as an ordinary method
    public class TestTile: WFCTile
    {
        public TestTile(NeighbourState[] states, State starting) : base(states, starting)
        {
        }
    }
    [Test]
    public void TileUnitTestSimplePasses()
    {
        // Use the Assert class to test conditions

        State grass = new State("Grass", 0.5f);

        State shrubs = new State("Shrubs", 0.4f);

        State trees = new State("Trees", 0.1f);

        State abyss = new State("Void", 0.9f);

        State unknown = new State("", 1f);

        //abyss.m_allowedNeighbours = new[] { abyss };
        //grass.m_allowedNeighbours = new[] { grass, shrubs };
        //shrubs.m_allowedNeighbours = new[] { grass, shrubs, trees };
        //trees.m_allowedNeighbours = new[] { shrubs };

        WFCTile fluxTile = new WFCTile(new[] { new NeighbourState(grass,1f)}, unknown); //[CreateAssetMenu(fileName = "Data", menuName = "WFC/State", order = 1)]

        WFCTile abyssTile = new WFCTile(abyss);
        Assert.AreEqual(-0.9f, abyssTile.getEntropy());
        Assert.AreEqual(1.497766832f, fluxTile.getEntropy(), 0.000001f);

        //fluxTile.updateStates(new[] { shrubs, trees });
        Assert.Contains(shrubs, fluxTile.possibleStates);
        Assert.Contains(trees, fluxTile.possibleStates);
        Assert.AreEqual(2, fluxTile.possibleStates.Length);

        abyssTile.updateStates(new[] { new NeighbourState(shrubs, 1f) });
        Assert.AreEqual(0, abyssTile.possibleStates.Length);
        fluxTile.SelectCurrentState();
        Assert.AreNotEqual(unknown, fluxTile.currentState);
            
       }
    

}
