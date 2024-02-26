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
    public Button button;
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
    public Button MaskButton;
    public TextMeshProUGUI MaskText;
    public Button ArmorButton;
    public TextMeshProUGUI ArmorText;
    public Button WaterTankButton;
    public TextMeshProUGUI WaterTankText;
    public Button WeaponButton;
    public TextMeshProUGUI WeaponText;

    List<BindedButton> BindedButtons;

    InventoryItem selectedItem;

    private void OnEnable()
    {
        BindedButtons ??= new List<BindedButton>();

        var player = PlayerManager.Instance.GetPlayerCharacter();

        for (int i = 0; i < player.Inventory.items.Count; i++)
        {
            CreateInventoryItemButton(player.Inventory.items[i], i+1);
        }
        InvokeRepeating(nameof(UpdateReferences), 0.3f, 0.3f);
    }

    private void OnDisable()
    {
        foreach (BindedButton button in BindedButtons)
        {
            Destroy(button.button.gameObject);
        }
        HideMenuView();
        BindedButtons.Clear();
        CancelInvoke(nameof(UpdateReferences));
    }

    void GetEquipmentInfo()
    {
        var player = PlayerManager.Instance.GetPlayerCharacter();
        var mask = player.Inventory.equipment.GetEquipment(EquipmentType.Mask);
        MaskText.text = mask == null ? "No Mask" : mask.Name;

        var armor = player.Inventory.equipment.GetEquipment(EquipmentType.Armour);
        ArmorText.text = armor == null ? "No Armor" : armor.Name;

        var waterTank = player.Inventory.equipment.GetEquipment(EquipmentType.WaterTank);
        WaterTankText.text = waterTank == null ? "No Water Tank" : waterTank.Name;

        var weapon = player.Inventory.equipment.GetEquipment(EquipmentType.Weapon);
        WeaponText.text = weapon == null ? "No Weapon" : weapon.Name;

    }

    void CreateInventoryItemButton(InventoryItem item, int order)
    {
        GameObject button = Instantiate(InventoryButtonPrefab, InventoryView.transform);
        button.transform.position = InventoryView.transform.position;
        TextMeshProUGUI text = button.GetComponentInChildren<TextMeshProUGUI>();
        text.text = item.amount + "x " + item.item.Name;
        var buttonComponent = button.GetComponent<Button>();
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

        var player = PlayerManager.Instance.GetPlayerCharacter();

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
        PlayerManager.Instance.GetPlayerCharacter().Inventory.RemoveItem(selectedItem.item);
    }
}
