using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class CanvasScaler_ExistsTest
{
    [UnityTest]
    public IEnumerator CanvasScaler_IsPresent()
    {
        GameObject created = null;
        if (Object.FindObjectOfType<CanvasScaler>() == null)
        {
            created = new GameObject("TempCanvas");
            var canvas = created.AddComponent<Canvas>();
            created.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        }

        yield return null;

        var cs = Object.FindObjectOfType<CanvasScaler>();
        Assert.IsNotNull(cs, "A CanvasScaler should exist in the scene (for UI scaling).");

        if (created != null) Object.Destroy(created);
        yield return null;
    }
}