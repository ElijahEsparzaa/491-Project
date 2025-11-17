using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CameraFollow_PlayerExistsTest
{
    [UnityTest]
    public IEnumerator CameraAndPlayerExist()
    {
        //Create a minimal player and camera
        var player = new GameObject("Player");
        player.AddComponent<Transform>();

        var camera = new GameObject("Main Camera");
        var cam = camera.AddComponent<Camera>();
        camera.tag = "MainCamera";

        yield return null;

        //Assert
        var mainCam = Camera.main;
        Assert.IsNotNull(mainCam, "Main Camera must exist and be tagged MainCamera.");
        Assert.IsNotNull(GameObject.Find("Player"), "Player GameObject must exist in the scene.");

        //Cleanup
        Object.Destroy(player);
        Object.Destroy(camera);
        yield return null;
    }
}
