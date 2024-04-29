using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class NPCDialogueTests
{
    [Test]
    public void ExpectedLines()
    {
        GameObject testNPC = new GameObject();
        testNPC.AddComponent<NPCStats>();
        Assert.AreEqual(testNPC.GetComponent<NPCStats>().numLines, testNPC.GetComponent<NPCStats>().allDialogueOptions.Count);
    }
}
