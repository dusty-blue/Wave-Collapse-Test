using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assets.Scripts.WFC;
using System.Linq;
using System;

public class StateUnitTest
{
     
    // A Test behaves as an ordinary method
    [Test]
    public void StateUnitTestSimplePasses()
    {
        // Use the Assert class to test conditions
        State grass = new State("Grass", 0.5f);

        Assert.AreEqual("Grass", grass.m_name);

        State shrubs = new State("Shrubs", 0.4f );
        
        State trees = new State("Trees", 0.1f);

        grass.m_allowedNeighbours = new[] { grass, shrubs };
        shrubs.m_allowedNeighbours = new[] { grass, shrubs ,trees};
        trees.m_allowedNeighbours = new []{ shrubs};

        Assert.IsNotNull(shrubs.m_allowedNeighbours);
        Assert.IsNotNull(trees.m_allowedNeighbours);
        Assert.IsNotNull(grass.m_allowedNeighbours);
        Assert.AreEqual(2, grass.m_allowedNeighbours.Length);
        Assert.AreEqual(3, shrubs.m_allowedNeighbours.Length);
        Assert.AreEqual(1, trees.m_allowedNeighbours.Length);
        Assert.Contains(grass, grass.m_allowedNeighbours);

        Assert.Contains(grass, grass.m_allowedNeighbours[1].m_allowedNeighbours);

        
        // TO DO?
        //grass.allowedNeighbours.Append(trees);
        //
        //Assert.AreEqual(3, grass.allowedNeighbours.Length);
        //Assert.Contains(trees, grass.allowedNeighbours);        

    }
}
