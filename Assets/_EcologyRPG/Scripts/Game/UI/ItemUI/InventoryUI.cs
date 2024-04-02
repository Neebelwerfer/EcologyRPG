using EcologyRPG.Core.Items;
using EcologyRPG.Game.Player;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EcologyRPG.Game.UI
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
            Inventory Inventory = PlayerManager.PlayerInventory;
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
            CreateInventoryItemButton(PlayerManager.PlayerInventory.GetInventoryItem(item));
            CarryWeight.text = PlayerManager.PlayerInventory.CurrentWeight + "/" + PlayerManager.PlayerInventory.MaxCarryWeight;
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
            CarryWeight.text = PlayerManager.PlayerInventory.CurrentWeight + "/" + PlayerManager.PlayerInventory.MaxCarryWeight;
        }

        void GetEquipmentInfo()
        {
            var mask = PlayerManager.PlayerInventory.equipment.GetEquipment(EquipmentType.Mask);
            MaskButton.Setup(mask);
            MaskButton.Unequip = () =>
            {
                PlayerManager.PlayerInventory.UnequipItem(mask);
                MaskButton.Setup(null);
            };

            var armor = PlayerManager.PlayerInventory.equipment.GetEquipment(EquipmentType.Armour);
            ArmorButton.Setup(armor);
            ArmorButton.Unequip = () =>
            {
                PlayerManager.PlayerInventory.UnequipItem(armor);
                ArmorButton.Setup(null);
            };

            var waterTank = PlayerManager.PlayerInventory.equipment.GetEquipment(EquipmentType.WaterTank);
            WaterTankButton.Setup(waterTank);
            WaterTankButton.Unequip = () =>
            {
                PlayerManager.PlayerInventory.UnequipItem(waterTank);
                WaterTankButton.Setup(null);
            };

            var weapon = PlayerManager.PlayerInventory.equipment.GetEquipment(EquipmentType.Weapon);
            WeaponButton.Setup(weapon);
            WeaponButton.Unequip = () =>
            {
                PlayerManager.PlayerInventory.UnequipItem(weapon);
                WeaponButton.Setup(null);
            };
            CarryWeight.text = PlayerManager.PlayerInventory.CurrentWeight + "/" + PlayerManager.PlayerInventory.MaxCarryWeight;
        }

        private void OnEnable()
        {
            if (player != null)
                CarryWeight.text = PlayerManager.PlayerInventory.CurrentWeight + "/" + PlayerManager.PlayerInventory.MaxCarryWeight;
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
            PlayerManager.PlayerInventory.DropItem(selectedItem.item);
        }
    }
}