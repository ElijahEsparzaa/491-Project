using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class QualitySettings_VSyncOrTargetFrameTest
{
    [UnityTest]
    public IEnumerator QualitySettings_TimingSanity()
    {
        yield return null;

        //vSyncCount is an int; assert it's present and not negative (API guarantee)
        Assert.GreaterOrEqual(QualitySettings.vSyncCount, 0, "QualitySettings.vSyncCount should be zero or positive.");

        //Application.targetFrameRate can be -1 for unlimited; check it's in sensible range when set
        int tf = Application.targetFrameRate;
        Assert.IsTrue(tf == -1 || (tf > 15 && tf <= 1000), "Application.targetFrameRate should be -1 (unlimited) or a sensible value (>15).");
    }
}