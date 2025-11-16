using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.EventSystems;

public class EventSystem_PresenceTest
{
    [UnityTest]
    public IEnumerator EventSystem_IsPresent()
    {
        GameObject esGO = null;
        if (Object.FindObjectOfType<EventSystem>() == null)
        {
            esGO = new GameObject("TempEventSystem");
            esGO.AddComponent<EventSystem>();
            esGO.AddComponent<StandaloneInputModule>();
        }

        yield return null;

        var es = Object.FindObjectOfType<EventSystem>();
        Assert.IsNotNull(es, "An EventSystem must be present for UI events to work.");

        if (esGO != null) Object.Destroy(esGO);
        yield return null;
    }
}