using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework; 

public class FrameRateSanityTest
{
    [UnityTest]
    public IEnumerator Game_Runs_For_One_Second_Without_Errors()
    {
        float startTime = Time.time;

        //Run for 1 second
        while (Time.time - startTime < 1f)
            yield return null;

        Assert.Greater(Time.frameCount, 0, "No frames were rendered — the game may not be updating correctly.");
    }
}