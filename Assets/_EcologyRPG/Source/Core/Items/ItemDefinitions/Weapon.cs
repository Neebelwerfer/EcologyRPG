using EcologyRPG.AbilityScripting;
using EcologyRPG.Core.Character;
using UnityEngine;

namespace EcologyRPG.Core.Items
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Items/Weapon")]
    public class Weapon : EquipableItem
    {

        public PlayerAbilityReference WeaponAbility;
        public GameObject WeaponModel;
        public Vector3 WeaponRotation;
        public Vector3 WeaponLocalOffset;

        GameObject weaponInstance;
        public Weapon() : base()
        {
            equipmentType = EquipmentType.Weapon;
            modifications.Add(new EquipmentModification
            {
                StatName = "rawWeaponDamage",
                type = ModType.Stat,
                modType = StatModType.Flat,
                Value = 1
            });
        }

        public override string GetDisplayString()
        {
            if (string.IsNullOrEmpty(DisplayString))
            {
                var desc = Description + "\n";
                var weaponDamage = (int)statModifiers.Find(x => x.StatName == "rawWeaponDamage").Value;
                desc += "Damage: " + weaponDamage + "\n";
                var mods = GetModifiers();
                DisplayString = desc + mods;
                return DisplayString;
            }
            else
            {
                return DisplayString;
            }
        }

        public override void Equip(BaseCharacter character)
        {
            base.Equip(character);
            if(WeaponModel != null)
            {
                Debug.Log("Equipping weapon model");
                weaponInstance = Instantiate(WeaponModel, character.CastPos, Quaternion.identity, character.CastTransform);
                weaponInstance.transform.localPosition = WeaponLocalOffset;
                weaponInstance.transform.localEulerAngles = WeaponRotation;
            }
        }

        public override void Unequip(BaseCharacter character)
        {
            base.Unequip(character);
            if(weaponInstance != null)
                Destroy(weaponInstance);
        }
    }
}