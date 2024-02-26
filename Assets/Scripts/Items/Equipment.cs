using Character;
using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment
{
    public Item[] EquipmentSlots;

    BaseCharacter baseCharacter;
    public Equipment(BaseCharacter baseCharacter)
    {
        this.baseCharacter = baseCharacter;
        EquipmentSlots = new Item[4];
    }

    public void EquipItem(Item item)
    {
        if(item is EquipableItem equipable)
        {
            EquipmentSlots[(int)equipable.equipmentType] = item;
            equipable.Equip(baseCharacter);
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
