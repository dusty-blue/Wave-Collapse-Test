using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assets.Scripts.WFC;
using System.Linq;
using System;
using UnityEditor;
using System.IO;

public class StateUnitTest
{
     
    // A Test behaves as an ordinary method
    [Test]
    public void StateUnitTestSimplePasses()
    {
        // Use the Assert class to test conditions
        List<String> stateNames = new()
        {
            "Grass",
            "Shrubs",
            "Tree"
        };

        Dictionary<String, State> stateDic = new();

        foreach (String s in stateNames)
        {
            string[] files = Directory.GetFiles($"Assets/Scripts/WFC/States/Test/N{s}", "*.asset", SearchOption.TopDirectoryOnly);
            List<NeighbourState> nStates = new();
            foreach (var f in files)
            {
                ScriptableObject o = AssetDatabase.LoadAssetAtPath<ScriptableObject>(f);
                if ( o.GetType() == typeof(State))
                {
                    stateDic.Add(s, (State)o);
                } else if (o.GetType() == typeof(NeighbourState))
                {
                    nStates.Add((NeighbourState)o);
                }
            }
            if (stateDic.ContainsKey(s))
            {
                stateDic[s].m_allowedNeighbours = nStates.ToArray();
            }
        }
        Assert.AreEqual(stateNames.Count, stateDic.Count);

        foreach(KeyValuePair<String,State> s in stateDic)
        {
            Assert.IsNotNull(s.Value.m_allowedNeighbours, $"Neighbours were null for {s.Key}");
            Assert.IsTrue(s.Value.Contains(stateDic["Shrubs"]),$"Could not find Shrubs for state {s.Key}");        
        }

        Assert.IsTrue(stateDic["Grass"].m_allowedNeighbours[1].m_state.Contains(stateDic["Shrubs"]));

        
        // TO DO?
        //grass.allowedNeighbours.Append(trees);
        //
        //Assert.AreEqual(3, grass.allowedNeighbours.Length);
        //Assert.Contains(trees, grass.allowedNeighbours);        

    }
}
