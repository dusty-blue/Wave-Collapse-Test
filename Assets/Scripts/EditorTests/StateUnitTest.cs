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
using static Assets.Scripts.EditorTests.UnitTestUtility;
public class StateUnitTest
{
     
    // A Test behaves as an ordinary method
    [Test]
    public void StateUnitTestSimplePasses()
    {
        // Use the Assert class to test conditions
        List<String> folderList = new()
        {
            "T-States"
        };
        LoadedObjects loadedScriptableObjects =SetupSidedStates(folderList);
        Assert.AreEqual(3, loadedScriptableObjects.allStates.Length);

        foreach (var currentState in loadedScriptableObjects.allStates)
        {
            foreach (var currentSocket in currentState.wfcSockets)
            {

                Assert.Contains(currentSocket, loadedScriptableObjects.allSockets, $"{currentSocket} from {currentState} not found in allSockets");
            }
        }
        State grass = loadedScriptableObjects.allStates.First(s => s.name == "TGrass");
        Assert.AreEqual(2, grass.wfcSockets.Count);
        Assert.AreEqual(4, grass.wfcPattern.Length);
    }
}
