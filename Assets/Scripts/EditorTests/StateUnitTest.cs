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
        State grass = new State();
        grass.name = "Grass";
        grass.SpawnWeight =0.5f;

        Assert.AreEqual("Grass", grass.name);

        State shrubs = new State();
        shrubs.name = "Shrubs";
        shrubs.SpawnWeight = 0.4f;
        
        State trees = new State();
        trees.name = "Trees";
        trees.SpawnWeight = 0.1f;

        grass.allowedNeighbours = new[] { grass, shrubs };
        shrubs.allowedNeighbours = new[] { grass, shrubs ,trees};
        trees.allowedNeighbours = new []{ shrubs};

        Assert.IsNotNull(shrubs.allowedNeighbours);
        Assert.IsNotNull(trees.allowedNeighbours);
        Assert.IsNotNull(grass.allowedNeighbours);
        Assert.AreEqual(2, grass.allowedNeighbours.Length);
        Assert.AreEqual(3, shrubs.allowedNeighbours.Length);
        Assert.AreEqual(1, trees.allowedNeighbours.Length);
        Assert.Contains(grass, grass.allowedNeighbours);

        Assert.Contains(grass, grass.allowedNeighbours[1].allowedNeighbours);

        
        // TO DO?
        //grass.allowedNeighbours.Append(trees);
        //
        //Assert.AreEqual(3, grass.allowedNeighbours.Length);
        //Assert.Contains(trees, grass.allowedNeighbours);        

    }
}
