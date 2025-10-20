using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class AllScenesSmokeTests
{
    [UnityTest]
    public IEnumerator EachScene_Loads_WithoutErrors()
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        Assert.Greater(sceneCount, 0, "No scenes in Build Settings.");

        for (int i = 0; i < sceneCount; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            yield return SceneManager.LoadSceneAsync(i, LoadSceneMode.Single);
            Assert.IsTrue(SceneManager.GetActiveScene().isLoaded, $"Scene failed to load: {path}");
            yield return null; // give one frame for initialization
        }
    }
}
