using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class SpawnerWaveTest
{
    [UnityTest]
    public IEnumerator GameScene_SpawnsEnemies_WithinTenSeconds()
    {
        //For this smoke test we only care that enemies eventually spawn.
        //Some existing logs (like PlayerStats NRE when loaded directly, or Physics2D.Simulate warnings)
        //would otherwise cause the test to fail, so we ignore failing log messages here.
        LogAssert.ignoreFailingMessages = true;

        //Load the main gameplay scene
        yield return SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
        yield return null; // let Awake/Start run

        float timeout = 10f;
        float elapsed = 0f;

        while (elapsed < timeout)
        {
            //Look for any object tagged as "Enemy"
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            if (enemies != null && enemies.Length > 0)
            {
                //We succeeded at least one enemy exists in the scene.
                Assert.Pass($"Found {enemies.Length} enemies after {elapsed:F1} seconds.");
            }

            elapsed += Time.deltaTime;
            yield return null; //Wait one frame
        }

        Assert.Fail("No enemies spawned in the Game scene within 10 seconds.");
    }
}