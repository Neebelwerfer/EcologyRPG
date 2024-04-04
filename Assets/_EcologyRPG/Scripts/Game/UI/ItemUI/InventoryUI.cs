using EcologyRPG.Core.Items;
using EcologyRPG.GameSystems.PlayerSystems;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EcologyRPG.GameSystems.UI
{
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
            Inventory Inventory = Player.PlayerInventory;
            for (int i = 0; i < Inventory.items.Count; i++)
            {
                CreateInventoryItemButton(Inventory.items[i]);
            }
            Inventory.InventoryChanged.AddListener(UpdateReferences);
            Inventory.ItemAdded.AddListener(OnItemAdded);
            Inventory.ItemRemoved.AddListener(OnItemRemoved);
            Inventory.equipment.EquipmentUpdated.AddListener((int type) => GetEquipmentInfo());
            CarryWeight.text = Inventory.CurrentWeight + "/" + Inventory.MaxCarryWeight;
        }

        void OnItemAdded(Item item)
        {
            CreateInventoryItemButton(Player.PlayerInventory.GetInventoryItem(item));
            CarryWeight.text = Player.PlayerInventory.CurrentWeight + "/" + Player.PlayerInventory.MaxCarryWeight;
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
            CarryWeight.text = Player.PlayerInventory.CurrentWeight + "/" + Player.PlayerInventory.MaxCarryWeight;
        }

        void GetEquipmentInfo()
        {
            var mask = Player.PlayerInventory.equipment.GetEquipment(EquipmentType.Mask);
            MaskButton.Setup(mask);
            MaskButton.Unequip = () =>
            {
                Player.PlayerInventory.UnequipItem(mask);
                MaskButton.Setup(null);
            };

            var armor = Player.PlayerInventory.equipment.GetEquipment(EquipmentType.Armour);
            ArmorButton.Setup(armor);
            ArmorButton.Unequip = () =>
            {
                Player.PlayerInventory.UnequipItem(armor);
                ArmorButton.Setup(null);
            };

            var waterTank = Player.PlayerInventory.equipment.GetEquipment(EquipmentType.WaterTank);
            WaterTankButton.Setup(waterTank);
            WaterTankButton.Unequip = () =>
            {
                Player.PlayerInventory.UnequipItem(waterTank);
                WaterTankButton.Setup(null);
            };

            var weapon = Player.PlayerInventory.equipment.GetEquipment(EquipmentType.Weapon);
            WeaponButton.Setup(weapon);
            WeaponButton.Unequip = () =>
            {
                Player.PlayerInventory.UnequipItem(weapon);
                WeaponButton.Setup(null);
            };
            CarryWeight.text = Player.PlayerInventory.CurrentWeight + "/" + Player.PlayerInventory.MaxCarryWeight;
        }

        private void OnEnable()
        {
            if (player != null)
                CarryWeight.text = Player.PlayerInventory.CurrentWeight + "/" + Player.PlayerInventory.MaxCarryWeight;
        }

        void CreateInventoryItemButton(InventoryItem item)
        {
            GameObject button = Instantiate(InventoryButtonPrefab, InventoryView.transform);
            button.transform.position = InventoryView.transform.position;
            var buttonComponent = button.GetComponent<InventoryButton>();
            buttonComponent.Setup(item);
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
            Player.PlayerInventory.DropItem(selectedItem.item);
        }
    }
}