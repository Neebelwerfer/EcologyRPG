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

    public GameObject InventoryMenuView;
    public Button UseButton;
    public TextMeshProUGUI ItemName;

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
            CreateInventoryItemButton(player.Inventory.items[i], i + 1);
        }
        player.Inventory.InventoryChanged.AddListener(UpdateReferences);
        player.Inventory.ItemAdded.AddListener(OnItemAdded);
        player.Inventory.ItemRemoved.AddListener(OnItemRemoved);
        player.Inventory.equipment.EquipmentUpdated.AddListener((int type) => GetEquipmentInfo());
    }

    private void OnDisable()
    {
        HideMenuView();
    }

    void OnItemAdded(Item item)
    {
        CreateInventoryItemButton(player.Inventory.GetInventoryItem(item), BindedButtons.Count + 1);
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
    }

    void GetEquipmentInfo()
    {
        var mask = player.Inventory.equipment.GetEquipment(EquipmentType.Mask);
        MaskButton.Setup(mask);

        var armor = player.Inventory.equipment.GetEquipment(EquipmentType.Armour);
        ArmorButton.Setup(armor);

        var waterTank = player.Inventory.equipment.GetEquipment(EquipmentType.WaterTank);
        WaterTankButton.Setup(waterTank);

        var weapon = player.Inventory.equipment.GetEquipment(EquipmentType.Weapon);
        WeaponButton.Setup(weapon);

    }

    void CreateInventoryItemButton(InventoryItem item, int order)
    {
        GameObject button = Instantiate(InventoryButtonPrefab, InventoryView.transform);
        button.transform.position = InventoryView.transform.position;
        var buttonComponent = button.GetComponent<InventoryButton>();
        buttonComponent.Setup(item);
        buttonComponent.onClick.AddListener(() => OnInventoryButtonClicked(item));
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
                if (BindedButtons[i].item == selectedItem)
                {
                    HideMenuView();
                }
                BindedButtons.RemoveAt(i);

                continue;
            }
            TextMeshProUGUI text = BindedButtons[i].button.GetComponentInChildren<TextMeshProUGUI>();
            text.text = BindedButtons[i].item.amount + "x " + BindedButtons[i].item.item.Name;
        }
    }

    private void OnInventoryButtonClicked(InventoryItem item)
    {
        selectedItem = item;
        ItemName.text = item.item.Name;
        InventoryMenuView.SetActive(true);
        UseButton.onClick.RemoveAllListeners();

        if (item.item is ConsumableItem ci)
        {
            UseButton.gameObject.SetActive(true);
            UseButton.GetComponentInChildren<TextMeshProUGUI>().text = "Use";
            UseButton.onClick.AddListener(() => player.Inventory.ConsumeItem(ci));
        }
        else if (item.item is EquipableItem ei)
        {
            UseButton.gameObject.SetActive(true);
            UseButton.GetComponentInChildren<TextMeshProUGUI>().text = "Equip";
            UseButton.onClick.AddListener(() => player.Inventory.EquipItem(ei));
        }
        else
        {
            UseButton.gameObject.SetActive(false);
        }
    }

    public void HideMenuView()
    {
        selectedItem = null;
        InventoryMenuView.SetActive(false);
        UseButton.gameObject.SetActive(false);
    }

    public void DropOneSelectedItem()
    {
        player.Inventory.DropItem(selectedItem.item);
    }
}
