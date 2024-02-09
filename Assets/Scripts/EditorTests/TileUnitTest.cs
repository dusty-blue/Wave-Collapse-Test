using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assets.Scripts.WFC;

public class TileUnitTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void TileUnitTestSimplePasses()
    {
        // Use the Assert class to test conditions
        
        State grass = new State();
        grass.name = "Grass";
        grass.spawnChance = 0.5f;

        Assert.AreEqual("Grass", grass.name);

        State shrubs = new State();
        shrubs.name = "Shrubs";
        shrubs.spawnChance = 0.4f;

        State trees = new State();
        trees.name = "Trees";
        trees.spawnChance = 0.1f;

        State abyss = new State();
        abyss.name = "Void";
        abyss.spawnChance =0.9f;

        State unknown = new State();
        unknown.name = "";
        unknown.spawnChance = 1f;

        abyss.allowedNeighbours = new[] { abyss };
        grass.allowedNeighbours = new[] { grass, shrubs };
        shrubs.allowedNeighbours = new[] { grass, shrubs, trees };
        trees.allowedNeighbours = new[] { shrubs };

        Tile fluxTile = new Tile(new[] { abyss, grass, shrubs, trees }, unknown);

        Tile abyssTile = new Tile(abyss);
        Assert.AreEqual(0.9f, abyssTile.getEntropy());
        Assert.AreEqual(1.9f, fluxTile.getEntropy());

        fluxTile.updateStates(new[] { shrubs, trees });
        Assert.Contains(shrubs, fluxTile.possibleStates);
        Assert.Contains(trees, fluxTile.possibleStates);
        Assert.AreEqual(2, fluxTile.possibleStates.Length);

        abyssTile.updateStates(new[] { shrubs });
        Assert.AreEqual(0, abyssTile.possibleStates.Length);
        fluxTile.SelectCurrentState();
        Assert.AreNotEqual(unknown, fluxTile.currentState);
            
       }
    [Test]
    public void TileUnitStatisticalPasses()
    {
        //TO DO
    }

}
