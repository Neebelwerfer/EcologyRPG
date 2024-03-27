using Character;
using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Equipment
{
    public Item[] EquipmentSlots;
    public UnityEvent<int> EquipmentUpdated;

    BaseCharacter baseCharacter;
    public Equipment(BaseCharacter baseCharacter)
    {
        this.baseCharacter = baseCharacter;
        EquipmentSlots = new Item[4];
        EquipmentUpdated = new UnityEvent<int>();
    }

    public void EquipItem(Item item)
    {
        if(item is EquipableItem equipable)
        {
            EquipmentSlots[(int)equipable.equipmentType] = item;
            equipable.Equip(baseCharacter);
            EquipmentUpdated.Invoke((int)equipable.equipmentType);
        } 
        else
        {
            Debug.LogError("Item is not equipable");
        }
    }

    public void UnequipItem(Item item)
    {
        if(item is EquipableItem equipable)
        {
            EquipmentSlots[(int)equipable.equipmentType] = null;
            equipable.Unequip(baseCharacter);
            EquipmentUpdated.Invoke((int)equipable.equipmentType);
        }
        else
        {
            Debug.LogError("Item is not equipable");
        }
    }

    public EquipableItem GetEquipment(EquipmentType equipmentType)
    {
        if (EquipmentSlots[(int)equipmentType] is EquipableItem equipable)
        {
            return equipable;
        }
        return null;
    }
}
