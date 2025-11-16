using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TimeScale_DefaultTest
{
    [UnityTest]
    public IEnumerator TimeScale_IsDefaultOne()
    {
        yield return null; //One frame to settle
        Assert.AreEqual(1f, Time.timeScale, 0.001f, "Time.timeScale should be 1.0 by default (not paused).");
    }
}