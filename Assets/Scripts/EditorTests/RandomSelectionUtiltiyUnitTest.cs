using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Assets.Scripts.WFC;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
public class RandomSelectionUtiltiyUnitTest
{
    public class TSingleAdd : RejectionSamplingAddSingle
    {
        public TSingleAdd(Dictionary<int,float> distr): base(distr)
        {

        }
        public void testTable(int expected)
        {
            Assert.AreEqual(expected, table.Count);
        }

        public void testTablecontains(int a)
        {
            Assert.Contains(a, table);
        }
    }
    public class TSubTable : RejectionSamplingAddSingle
    {
        public TSubTable(Dictionary<int, float> distr) : base(distr)
        {

        }
        public void testTable(int expected)
        {
            Assert.AreEqual(expected, table.Count);
        }

        public void testTablecontains(int a)
        {
            Assert.Contains(a, table);
        }
    }
    // A Test behaves as an ordinary method
    [Test]
    public void RandomSelectionUtiltiyUnitTestSimpleSingleAdd()
    {
        // Use the Assert class to test conditions
        Dictionary<int, float> simpleDistr = new Dictionary<int, float>();
        simpleDistr.Add(0, 0.5f);
        simpleDistr.Add(1, 0.5f);
        TSingleAdd test01 = new TSingleAdd(simpleDistr);
        test01.testTable(2);

        Dictionary<int, float> oddDistr = new Dictionary<int, float>();
        oddDistr.Add(0, 1f / 3f);
        oddDistr.Add(1, 2f / 3f);

        TSingleAdd test02 = new TSingleAdd(oddDistr);
        test02.testTable(3);
        Assert.IsInstanceOf(typeof(int), test02.DrawSample());

    }

    [Test]
    public void RandomSelectionUtiltiyUnitTestSubTableLarge()
    {
        Dictionary<int, float> simpleDistr = new Dictionary<int, float>();
        simpleDistr.Add(0, 0.000001f);
        simpleDistr.Add(1, 1f - 0.000001f);
        TSubTable testLarge = new TSubTable(simpleDistr);

        testLarge.testTablecontains(0);
    }

    [Test]
    public void RandomSelectionUtiltiyUnitTestSingleAddLarge()
    {
        
        Dictionary<int, float> simpleDistr = new Dictionary<int, float>();
        simpleDistr.Add(0, 0.000001f);
        simpleDistr.Add(1, 1f- 0.000001f);
        TSingleAdd testLarge = new TSingleAdd(simpleDistr);


        testLarge.testTablecontains(0);
    }

    [Test]
    public void RandomSelectionUtiltiyUnitTestSimpleSubTable()
    {
        // Use the Assert class to test conditions
        Dictionary<int, float> simpleDistr = new Dictionary<int, float>();
        simpleDistr.Add(0, 0.5f);
        simpleDistr.Add(1, 0.5f);
        TSubTable test01 = new TSubTable(simpleDistr);
        test01.testTable(2);

        Dictionary<int, float> oddDistr = new Dictionary<int, float>();
        oddDistr.Add(0, 1f/3f);
        oddDistr.Add(1, 2f/3f);

        TSubTable test02 = new TSubTable(oddDistr);

        test02.testTable(3);
        Assert.IsInstanceOf(typeof(int), test02.DrawSample());

    }

    [Test]
    public void RandomSelectionUtilityUnitTestAlias()
    {
        
        List<float> simpleDistr = new List<float>();
        simpleDistr.Add(0.5f);
        simpleDistr.Add(0.25f);
        simpleDistr.Add(0.25f);
        AliasSampling tAlias = new AliasSampling(simpleDistr);

        Assert.IsInstanceOf<int>(tAlias.DrawSample());

        List<float> improperDistr = new List<float>();
        improperDistr.Add(50f);
        improperDistr.Add(20f);

        AliasSampling tAlias2 = new AliasSampling(improperDistr);
        int s = tAlias2.DrawSample();

        Assert.That(s >= 0 && s < improperDistr.Count);
    }

    [Test]
    public void anotherTest()
    {
        Stopwatch sw = Stopwatch.StartNew();
        List<float> simpleDistr = new List<float>();
        simpleDistr.Add( 0.000001f);
        simpleDistr.Add(1f-0.000001f);
        AliasSampling tAlias = new AliasSampling(simpleDistr);

        sw.Stop();
        //UnityEngine.Debug.Log($"alias took {sw.ElapsedMilliseconds}ms to run.");

        Assert.IsInstanceOf<int>(tAlias.DrawSample());
    }


    [Test]
    public void statisticalTest()
    {
        List<float> simpleDistr = new List<float>();
        simpleDistr.Add(0.5f);
        simpleDistr.Add(0.5f);
        AliasSampling tAlias = new AliasSampling(simpleDistr);

        int results = 0;
        for(int i =0; i<100; i++)
        {
            results += tAlias.DrawSample();
        }
        //UnityEngine.Debug.Log(results);

    }

}
