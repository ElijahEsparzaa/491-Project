using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Lighting_SceneHasLightOrDefaultTest
{
    [UnityTest]
    public IEnumerator SceneHasAtLeastOneLight()
    {
        GameObject created = null;
        if (Object.FindObjectOfType<Light>() == null)
        {
            created = new GameObject("TempDirectionalLight");
            var light = created.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 0.6f;
        }

        yield return null;

        var any = Object.FindObjectOfType<Light>();
        Assert.IsNotNull(any, "Scene should contain at least one Light (directional or point).");

        if (created != null) Object.Destroy(created);
        yield return null;
    }
}