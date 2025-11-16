using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Scene_CanvasExistsTest
{
    [UnityTest]
    public IEnumerator SceneContains_Canvas()
    {
        GameObject temp = null;
        if (Object.FindObjectOfType<Canvas>() == null)
        {
            temp = new GameObject("TempCanvas");
            temp.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        }

        yield return null;

        var canvases = Object.FindObjectsOfType<Canvas>();
        Assert.IsTrue(canvases.Length >= 1, "There should be at least one Canvas in the scene.");

        if (temp != null) Object.Destroy(temp);
        yield return null;
    }
}