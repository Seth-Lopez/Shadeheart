using UnityEngine;
using UnityEngine.UI;

public class CombatMenu : MonoBehaviour
{
    // Buttons on the UI
    public Button attackButton;
    public Button defendButton;
    public Button useItemButton;
    public Button fleeButton;
    
    void Start()
    {
        // Register the buttons with their respective methods
        attackButton.onClick.AddListener(Attack);
        defendButton.onClick.AddListener(Defend);
        useItemButton.onClick.AddListener(UseItem);
        fleeButton.onClick.AddListener(Flee);
    }

    void Attack()
    {
        Debug.Log("Attack selected.");
        // Add your attack logic here
    }

    void Defend()
    {
        Debug.Log("Defend selected.");
        // Add your defend logic here
    }

    void UseItem()
    {
        Debug.Log("Use Item selected.");
        // Add your item use logic here
    }

    void Flee()
    {
        Debug.Log("Flee selected.");
        // Add your flee logic here
    }
}
