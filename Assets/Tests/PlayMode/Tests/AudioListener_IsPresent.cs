using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class AudioListener_PresentTest
{
    [UnityTest]
    public IEnumerator AudioListener_IsPresent()
    {
        //Try find any AudioListener
        var anyListener = Object.FindObjectOfType<AudioListener>();
        GameObject created = null;

        if (anyListener == null)
        {
            //Create a temp camera + audio listener
            created = new GameObject("TempCamWithAudio");
            created.AddComponent<Camera>();
            created.AddComponent<AudioListener>();
        }

        yield return null;

        Assert.IsNotNull(Object.FindObjectOfType<AudioListener>(), "There should be at least one AudioListener in the scene.");

        if (created != null) Object.Destroy(created);
        yield return null;
    }
}