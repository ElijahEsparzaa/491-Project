using NUnit.Framework;
using UnityEngine;

public class WorkflowVerificationTest
{
    [Test]
    public void CI_Pipeline_Is_Running_Correctly()
    {
        //Dummy test to confirm the GitHub Actions CI/CD workflow runs Unity tests.
        Debug.Log("✅ Unity CI/CD Test Workflow Triggered Successfully!");
        Assert.IsTrue(true, "The test pipeline executed correctly.");
    }
}