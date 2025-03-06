using Assets.Scripts.EditorTests;
using Assets.Scripts.WFC;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


internal class TStateUnitTest
{
    
    [Test]
    public void TStateLoadingTest()
    {
        List<String> folderList = new()
        {

            "T-State Unit Test Cases"
        };
        LoadedObjects objects = UnitTestUtility.SetupSidedStates(folderList);

        Assert.IsNotNull(objects.allStates.First(s => s.name == "T-State"));
        Assert.AreEqual(2, objects.allStates.Length);
        
    }

    [Test]
    public void TStateIsPlaceableSimpleTest()
    {
        List<String> folderList = new()
        {
            "T-State Unit Test Cases"
        };
        LoadedObjects objects = UnitTestUtility.SetupSidedStates(folderList);

        State test = objects.allStates.First(s => s.name == "T-State");
        WFCSocket borderSocket = objects.allSockets.First(s => s.name == "Border-Socket");
        WFCSocket singleSocket = objects.allSockets.First(s => s.name == "Grass-Border-Socket");

        const int DIRECTIONS = 4;
        WFCSocket[][] testCase00 = new WFCSocket[DIRECTIONS][];
        WFCSocket[][] testCase01 = new WFCSocket[DIRECTIONS][];
        WFCSocket[][] testCase02 = new WFCSocket[DIRECTIONS][];
        WFCSocket[][] testCase03 = new WFCSocket[DIRECTIONS][];
        WFCSocket[][] testCase04 = new WFCSocket[DIRECTIONS][];
        for (int i = 0; i< DIRECTIONS; i++)
        {
            testCase00[i] = new WFCSocket[1];
            testCase01[i] = new WFCSocket[1];
            testCase02[i] = new WFCSocket[1];
            testCase03[i] = new WFCSocket[1];
            testCase04[i] = new WFCSocket[1];

            testCase01[i][0] = borderSocket;
            testCase02[i][0] = borderSocket; 
            testCase03[i][0] = borderSocket;
            testCase04[i][0] = borderSocket;


        }

        // All four orientations possible
        testCase01[0][0] = singleSocket;
        testCase02[1][0] = singleSocket;
        testCase03[2][0] = singleSocket;
        testCase04[3][0] = singleSocket;

        if (test is TState state)
        {
            Assert.AreEqual(state.ThreeSocket , borderSocket);
            Assert.IsFalse(state.isPlaceable(testCase00));
            Assert.IsTrue(state.isPlaceable(testCase01));
            Assert.IsTrue(state.isPlaceable(testCase02));
            Assert.IsTrue(state.isPlaceable(testCase03));
            Assert.IsTrue(state.isPlaceable(testCase04));

        }
    }

    [Test]
    public void TStateIsPlaceableComplexTest()
    {
        List<String> folderList = new()
        {
            "T-State Unit Test Cases"
        };
        LoadedObjects objects = UnitTestUtility.SetupSidedStates(folderList);

        State test = objects.allStates.First(s => s.name == "T-State");
        WFCSocket borderSocket = objects.allSockets.First(s => s.name == "Border-Socket");
        WFCSocket singleSocket = objects.allSockets.First(s => s.name == "Grass-Border-Socket");
        WFCSocket rockSocket = objects.allSockets.First(s => s.name == "Grass-Rock-Socket");

        const int DIRECTIONS = 4;
        WFCSocket[][] testCase00 = new WFCSocket[DIRECTIONS][];
        WFCSocket[][] testCase01 = new WFCSocket[DIRECTIONS][];
        WFCSocket[][] testCase02 = new WFCSocket[DIRECTIONS][];
        WFCSocket[][] testCase03 = new WFCSocket[DIRECTIONS][];
        WFCSocket[][] testCase04 = new WFCSocket[DIRECTIONS][];
        List<WFCSocket[][]> testCases = new();

        testCases.Add(testCase00);
        testCases.Add(testCase01);
        testCases.Add(testCase02);
        testCases.Add(testCase03);
        testCases.Add(testCase04);
        
        for (int i = 0; i < DIRECTIONS; i++)
        {
            testCase00[i] = new WFCSocket[2];
            testCase01[i] = new WFCSocket[i+1];
            testCase02[i] = new WFCSocket[5-i];
            testCase03[i] = new WFCSocket[i+2];
            testCase04[i] = new WFCSocket[3];

            testCase01[i][0] = borderSocket;
            testCase02[i][4-i] = borderSocket;
            testCase03[i][1] = borderSocket;
            testCase04[i][0] = borderSocket;
        }

        foreach (var testCase in testCases)
        {
            for (int i = 0; i < DIRECTIONS; i++)
            {
                for (int j = 0; j < testCase[i].Length; j++)
                {
                    if (borderSocket != testCase[i][j])
                    {
                        testCase[i][j] = rockSocket;
                    }
                    
                }
            }
        }
        // All four orientations possible
        testCase01[0][0] = singleSocket;
        testCase02[1][0] = singleSocket;
        testCase03[2][0] = singleSocket;
        testCase04[3][0] = singleSocket;

        if (test is TState state)
        {
            Assert.AreEqual(state.ThreeSocket, borderSocket);
            Assert.IsFalse(state.isPlaceable(testCase00));
            Assert.IsTrue(state.isPlaceable(testCase01));
            Assert.IsTrue(state.isPlaceable(testCase02));
            Assert.IsTrue(state.isPlaceable(testCase03));
            Assert.IsTrue(state.isPlaceable(testCase04));
            Assert.IsTrue(state.isPlaceable(testCase02));

        }
    }

    [Test]
    public void TStateReturnRotatedSimpleTest()
    {
        List<String> folderList = new()
        {
            "T-State Unit Test Cases"
        };
        LoadedObjects objects = UnitTestUtility.SetupSidedStates(folderList);

        State test = objects.allStates.First(s => s.name == "T-State");
        WFCSocket borderSocket = objects.allSockets.First(s => s.name == "Border-Socket");
        WFCSocket singleSocket = objects.allSockets.First(s => s.name == "Grass-Border-Socket");

        const int DIRECTIONS = 4;
        WFCSocket[][] testCase00 = new WFCSocket[DIRECTIONS][];
        WFCSocket[][] testCase01 = new WFCSocket[DIRECTIONS][];
        WFCSocket[][] testCase02 = new WFCSocket[DIRECTIONS][];
        WFCSocket[][] testCase03 = new WFCSocket[DIRECTIONS][];
        WFCSocket[][] testCase04 = new WFCSocket[DIRECTIONS][];
        for (int i = 0; i < DIRECTIONS; i++)
        {
            testCase00[i] = new WFCSocket[1];
            testCase01[i] = new WFCSocket[1];
            testCase02[i] = new WFCSocket[1];
            testCase03[i] = new WFCSocket[1];
            testCase04[i] = new WFCSocket[1];

            testCase01[i][0] = borderSocket;
            testCase02[i][0] = borderSocket;
            testCase03[i][0] = borderSocket;
            testCase04[i][0] = borderSocket;
        }

        // All four orientations possible
        testCase01[0][0] = singleSocket;
        testCase02[1][0] = singleSocket;
        testCase03[2][0] = singleSocket;
        testCase04[3][0] = singleSocket;

        if (test is TState state)
        {
            Assert.AreEqual(state.ReturnRotatedPlacement(testCase00), testCase00);
            Assert.AreEqual(state.ReturnRotatedPlacement(testCase01), testCase01);
            Assert.AreEqual(state.ReturnRotatedPlacement(testCase02), testCase02); 
            Assert.AreEqual(state.ReturnRotatedPlacement(testCase03), testCase03);
            Assert.AreEqual(state.ReturnRotatedPlacement(testCase04), testCase04);
        }
    }

    [Test]
    public void TStateReturnRotatedComplexTest()
    {
        List<String> stateNames = new()
        {
            "T-State Unit Test Cases"
        };
        LoadedObjects objects = UnitTestUtility.SetupSidedStates(stateNames);

        State test = objects.allStates.First(s => s.name == "T-State");
        WFCSocket borderSocket = objects.allSockets.First(s => s.name == "Border-Socket");
        WFCSocket singleSocket = objects.allSockets.First(s => s.name == "Grass-Border-Socket");
        WFCSocket rockSocket = objects.allSockets.First(s => s.name == "Grass-Rock-Socket");

        const int DIRECTIONS = 4;
        WFCSocket[][] testCase00 = new WFCSocket[DIRECTIONS][];
        WFCSocket[][] tStateDown = new WFCSocket[DIRECTIONS][];
        WFCSocket[][] tStateLeft = new WFCSocket[DIRECTIONS][];
        WFCSocket[][] tStateUp = new WFCSocket[DIRECTIONS][];
        WFCSocket[][] tStateRight = new WFCSocket[DIRECTIONS][];
        for (int i = 0; i < DIRECTIONS; i++)
        {
            testCase00[i] = new WFCSocket[4];
            tStateDown[i] = new WFCSocket[1];
            tStateLeft[i] = new WFCSocket[1];
            tStateUp[i] = new WFCSocket[1];
            tStateRight[i] = new WFCSocket[1];

            tStateDown[i][0] = borderSocket;
            tStateLeft[i][0] = borderSocket;
            tStateUp[i][0] = borderSocket;
            tStateRight[i][0] = borderSocket;
        }

        // All four orientations possible
        tStateDown[0][0] = singleSocket;
        tStateLeft[1][0] = singleSocket;
        tStateUp[2][0] = singleSocket;
        tStateRight[3][0] = singleSocket;
        // Test Case
        foreach (var direction in testCase00)
        {
            for (int i = 0; i < direction.Length; i++)
            {
                direction[i] = rockSocket;
            }
        }

        testCase00[0][2] = singleSocket;
        testCase00[1][3] = borderSocket;
        testCase00[2][1] = borderSocket;
        testCase00[3][0] = borderSocket;
        if (test is TState state)
        {
            Assert.AreEqual(state.ReturnRotatedPlacement(testCase00), tStateDown);
        }
    }


}


