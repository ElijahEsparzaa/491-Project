using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using System.Collections;


public class SceneLoadTests
{
    [UnityTest]
    public IEnumerator Scene_CanLoadProperly()
    {
        yield return SceneManager.LoadSceneAsync("Main Menu", LoadSceneMode.Single);
        Assert.IsTrue(SceneManager.GetActiveScene().isLoaded, "Main Menu failed to load!");
    }
}
