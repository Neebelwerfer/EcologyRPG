using Items;
using Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject InventoryButtonPrefab;
    public GameObject InventoryView;


    public List<GameObject> InventoryButtons;


    private void OnEnable()
    {
        InventoryButtons ??= new List<GameObject>();

        var player = PlayerManager.Instance.GetPlayerCharacter();

        for (int i = 0; i < player.Inventory.items.Count; i++)
        {
            CreateInventoryItemButton(player.Inventory.items[i], i+1);
        }
    }

    private void OnDisable()
    {
        foreach (GameObject button in InventoryButtons)
        {
            Destroy(button);
        }
        InventoryButtons.Clear();
    }

    void CreateInventoryItemButton(InventoryItem item, int order)
    {
        GameObject button = Instantiate(InventoryButtonPrefab, InventoryView.transform);
        button.transform.position = InventoryView.transform.position;
        TextMeshProUGUI text = button.GetComponentInChildren<TextMeshProUGUI>();
        text.text = item.amount + "x " + item.item.Name;
        InventoryButtons.Add(button);
    }
}
