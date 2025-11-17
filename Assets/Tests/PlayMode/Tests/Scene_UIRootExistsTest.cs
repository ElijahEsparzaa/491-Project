using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class Scene_UIRootExistsTest
{
    [UnityTest]
    public IEnumerator Scene_ContainsAtLeastOneCanvas()
    {
        //Create a canvas temporarily
        var canvasGO = new GameObject("TestCanvas");
        canvasGO.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

        yield return null;

        var canvases = Object.FindObjectsOfType<Canvas>();
        Assert.IsTrue(canvases.Length >= 1, "There should be at least one Canvas in the scene.");

        Object.Destroy(canvasGO);
        yield return null;
    }
}