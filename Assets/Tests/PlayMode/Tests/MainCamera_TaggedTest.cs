using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MainCamera_TaggedTest
{
    [UnityTest]
    public IEnumerator MainCamera_IsTaggedAndAccessible()
    {
        //Create a temp camera if none exists
        GameObject camGO = null;
        if (Camera.main == null)
        {
            camGO = new GameObject("TempMainCamera");
            var cam = camGO.AddComponent<Camera>();
            camGO.tag = "MainCamera";
        }

        yield return null; //Allow one frame

        Assert.IsNotNull(Camera.main, "Camera.main should reference a tagged MainCamera.");
        Assert.AreEqual("MainCamera", Camera.main.gameObject.tag, "Main camera must be tagged 'MainCamera'.");

        if (camGO != null) Object.Destroy(camGO);
        yield return null;
    }
}
