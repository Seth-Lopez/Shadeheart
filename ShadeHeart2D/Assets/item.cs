using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour
{
    private bool isPlayerInRange = false;
    private UIMenuMngr UIClass;
    private InventoryMngr InvMngr;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
    void Start()
    {
        UIClass = GameObject.FindGameObjectWithTag("UIMngr").GetComponent<UIMenuMngr>();
        InvMngr = GameObject.FindGameObjectWithTag("InventoryMngr").GetComponent<InventoryMngr>();
    }
    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !UIClass.getIsPauseMenuOpen())
        {
            List<string> lines = new List<string>{"First Item Achievement"};
            InvMngr.interactions("Achiv_1", lines);
            this.gameObject.SetActive(false);
        }
    }
}
