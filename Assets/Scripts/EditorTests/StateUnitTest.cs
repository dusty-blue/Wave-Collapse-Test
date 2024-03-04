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
using UnityEngine.UIElements;

public class StateUnitTest
{
     
    // A Test behaves as an ordinary method
    [Test]
    public void StateUnitTestSimplePasses()
    {
        // Use the Assert class to test conditions
        List<String> stateNames = new()
        {
            "NGrass",
            "NShrubs",
            "NTree"
        };
        StateLoader loader = new("Assets/Scripts/WFC/States/Test");
        Dictionary <String, State> stateDic = loader.LoadStates(stateNames.ToArray());
        Assert.AreEqual(stateNames.Count, stateDic.Count);

        foreach(KeyValuePair<String,State> s in stateDic)
        {
            Assert.IsNotNull(s.Value.m_allowedNeighbours, $"Neighbours were null for {s.Key}");
            Assert.IsTrue(s.Value.Contains(stateDic["NShrubs"]),$"Could not find Shrubs for state {s.Key}");        
        }

        //Assert.IsTrue(stateDic["NGrass"].m_allowedNeighbours[1].m_state.Contains(stateDic["NShrubs"]));

        Dictionary<String,State> differentdic =loader.LoadStates(new[]
        {
            "Pond",
            "Sand"
        });
        
        // TO DO?
        //grass.allowedNeighbours.Append(trees);
        //
        //Assert.AreEqual(3, grass.allowedNeighbours.Length);
        //Assert.Contains(trees, grass.allowedNeighbours);        

    }
    [Test]
    public void InitialTestSetup()
    {
        List<String> stateNames = new()
        {
            "NGrass",
            "NShrubs",
            "NTree",
            "Sand",
            "Pond"
        };
        StateLoader loader = new("Assets/Scripts/WFC/States/Test");
        Dictionary<String, State> stateDic = loader.LoadStates(stateNames.ToArray());

        List<NeighbourState> initList = new List<NeighbourState>(stateNames.Count);

        //State initial = ScriptableObject.CreateInstance<State>();

        //foreach (KeyValuePair<String,State> s in stateDic)
        //{
        //    NeighbourState ns = ScriptableObject.CreateInstance<NeighbourState>();
        //    ns.m_state = s.Value;
        //    ns.m_weight = 1f;
        //    initList.Add(ns);

        //    AssetDatabase.CreateAsset(ns, $"Assets/Scripts/WFC/States/Test/Init/{ns.m_state.m_name}Ninit.asset");
        //}
        //initial.m_allowedNeighbours = initList.ToArray();
        //AssetDatabase.CreateAsset(initial, "Assets/Scripts/WFC/States/Test/Init/Init.asset");
    }
}
