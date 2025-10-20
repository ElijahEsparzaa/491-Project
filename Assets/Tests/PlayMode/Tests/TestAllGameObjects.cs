using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MissingScriptsCheck
{
    [UnityTest]
    public IEnumerator Scene_Has_No_Missing_Scripts()
    {
        yield return null; // wait one frame for objects to initialize

        foreach (var go in Object.FindObjectsOfType<GameObject>())
        {
            var components = go.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                Assert.IsNotNull(components[i],
                    $"Missing script on GameObject '{go.name}' (index {i}) in scene '{go.scene.name}'.");
            }
        }
    }
}
