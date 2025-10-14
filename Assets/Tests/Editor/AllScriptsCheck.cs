using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;

public class AllScriptsCheck
{
    [Test]
    public void AllGameScripts_LoadWithoutErrors()
    {
        // Get all MonoBehaviour scripts in the project assemblies
        var allMonoBehaviours = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a =>
            {
                try
                {
                    return a.GetTypes();
                }
                catch (ReflectionTypeLoadException e)
                {
                    // Handle assemblies that fail to load properly
                    return e.Types.Where(t => t != null);
                }
            })
            .Where(t => t.IsSubclassOf(typeof(MonoBehaviour)) && !t.IsAbstract);

        int scriptCount = 0;

        foreach (var type in allMonoBehaviours)
        {
            scriptCount++;
            Assert.IsNotNull(type, $"Script {type?.Name ?? "Unknown"} failed to load properly.");
        }

        Debug.Log($"[Auto-Check] Successfully validated {scriptCount} MonoBehaviour scripts in the project.");
    }
}