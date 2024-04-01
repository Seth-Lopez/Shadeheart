using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class InventoryTests
{
    [Test]
    public void CorrectNumItems()
    {
        GameObject testInventory = new GameObject();
        testInventory.AddComponent<InventoryMngr>();

        Assert.AreEqual(testInventory.GetComponent<InventoryMngr>().NumItems, testInventory.GetComponent<InventoryMngr>().getItemsList().Count);
    }
}