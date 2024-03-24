using Items;
using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

struct BindedButton
{
    public InventoryButton button;
    public InventoryItem item;
}

public class InventoryUI : MonoBehaviour
{
    public GameObject InventoryButtonPrefab;
    public GameObject InventoryView;

    public Button UseButton;
    public TextMeshProUGUI ItemName;
    public TextMeshProUGUI CarryWeight;

    [Header("Equipment View")]
    public EquipmentButton MaskButton;
    public EquipmentButton ArmorButton;
    public EquipmentButton WaterTankButton;
    public EquipmentButton WeaponButton;

    List<BindedButton> BindedButtons;

    InventoryItem selectedItem;
    PlayerCharacter player;

    private void Awake()
    {
        BindedButtons ??= new List<BindedButton>();
        player = PlayerManager.Instance.GetPlayerCharacter();
        for (int i = 0; i < player.Inventory.items.Count; i++)
        {
            CreateInventoryItemButton(player.Inventory.items[i]);
        }
        player.Inventory.InventoryChanged.AddListener(UpdateReferences);
        player.Inventory.ItemAdded.AddListener(OnItemAdded);
        player.Inventory.ItemRemoved.AddListener(OnItemRemoved);
        player.Inventory.equipment.EquipmentUpdated.AddListener((int type) => GetEquipmentInfo());
        CarryWeight.text = player.Inventory.CurrentWeight + "/" + player.Inventory.MaxCarryWeight;
    }

    void OnItemAdded(Item item)
    {
        CreateInventoryItemButton(player.Inventory.GetInventoryItem(item));
        CarryWeight.text = player.Inventory.CurrentWeight + "/" + player.Inventory.MaxCarryWeight;
    }

    void OnItemRemoved(Item item)
    {
        for (int i = BindedButtons.Count - 1; i >= 0; i--)
        {
            if (BindedButtons[i].item.item == item)
            {
                Destroy(BindedButtons[i].button.gameObject);
                BindedButtons.RemoveAt(i);
                break;
            }
        }
        CarryWeight.text = player.Inventory.CurrentWeight + "/" + player.Inventory.MaxCarryWeight;
    }

    void GetEquipmentInfo()
    {
        var mask = player.Inventory.equipment.GetEquipment(EquipmentType.Mask);
        MaskButton.Setup(mask);
        MaskButton.Unequip = () =>
        {
            player.Inventory.UnequipItem(mask);
            MaskButton.Setup(null);
        };

        var armor = player.Inventory.equipment.GetEquipment(EquipmentType.Armour);
        ArmorButton.Setup(armor);
        ArmorButton.Unequip = () =>
        {
            player.Inventory.UnequipItem(armor);
            ArmorButton.Setup(null);
        };

        var waterTank = player.Inventory.equipment.GetEquipment(EquipmentType.WaterTank);
        WaterTankButton.Setup(waterTank);
        WaterTankButton.Unequip = () =>
        {
            player.Inventory.UnequipItem(waterTank);
            WaterTankButton.Setup(null);
        };

        var weapon = player.Inventory.equipment.GetEquipment(EquipmentType.Weapon);
        WeaponButton.Setup(weapon);
        WeaponButton.Unequip = () =>
        {
            player.Inventory.UnequipItem(weapon);
            WeaponButton.Setup(null);
        };
        CarryWeight.text = player.Inventory.CurrentWeight + "/" + player.Inventory.MaxCarryWeight;
    }

    private void OnEnable()
    {
        if (player != null)
            CarryWeight.text = player.Inventory.CurrentWeight + "/" + player.Inventory.MaxCarryWeight;
    }

    void CreateInventoryItemButton(InventoryItem item)
    {
        GameObject button = Instantiate(InventoryButtonPrefab, InventoryView.transform);
        button.transform.position = InventoryView.transform.position;
        var buttonComponent = button.GetComponent<InventoryButton>();
        buttonComponent.Setup(item);
        buttonComponent.inventory = player.Inventory;
        BindedButtons.Add(new BindedButton { button = buttonComponent, item = item });
    }

    void UpdateReferences()
    {
        GetEquipmentInfo();
        for (int i = BindedButtons.Count - 1; i >= 0; i--)
        {
            if (BindedButtons[i].item.amount == 0)
            {
                Destroy(BindedButtons[i].button.gameObject);
                BindedButtons.RemoveAt(i);

                continue;
            }
            TextMeshProUGUI text = BindedButtons[i].button.GetComponentInChildren<TextMeshProUGUI>();
            text.text = BindedButtons[i].item.amount + "x";
        }
    }

    public void DropOneSelectedItem()
    {
        player.Inventory.DropItem(selectedItem.item);
    }
}
