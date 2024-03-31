using EcologyRPG.Core.Abilities.AbilityData;
using EcologyRPG.Game.Player;
using EcologyRPG.Utility;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.Game.UI
{
    public class AbilitySelectionUI
    {
        static AbilitySelectionUI _instance;
        public static AbilitySelectionUI Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new();
                }
                return _instance;
            }
        }
        PlayerAbilities abilityLookup;
        GameObject abilitySelectionUI;
        GameObject buttonPrefab;

        List<AbilityButton> buttonList;
        AbilityUI selectedAbilityUI;
        GameObjectPool ButtonPool;
        PlayerCharacter player;

        private void AssignAbility(PlayerAbilityDefinition ability)
        {
            var abilityHandler = player.playerAbilitiesHandler;
            if (abilityHandler.GotAbility(ability, out var slot))
            {
                if (slot.HasValue)
                {
                    if (slot.Value == selectedAbilityUI.abilitySlot)
                    {
                        Hide();
                        return;
                    }
                    abilityHandler.SetAbility(slot.Value, null);
                }
            }
            abilityHandler.SetAbility(selectedAbilityUI.abilitySlot, ability);
            Hide();
        }

        public void Setup(GameObject abilitySelectionUI)
        {
            this.abilitySelectionUI = abilitySelectionUI;
            abilitySelectionUI.SetActive(false);
            player = Player.PlayerManager.GetPlayer();
            abilityLookup = Resources.Load<PlayerAbilities>("PlayerAbilities");
            buttonPrefab = Resources.Load<GameObject>("UI/AbilityRef");

            ButtonPool = new GameObjectPool(buttonPrefab);
            ButtonPool.Preload(abilityLookup.Count);
            buttonList = new();
        }

        public void Show(AbilityUI abilityUI)
        {
            if (abilityUI.abilitySlot == AbilitySlots.WeaponAttack || abilityUI.abilitySlot == AbilitySlots.Dodge)
            {
                return;
            }

            if (abilityUI == selectedAbilityUI)
            {
                Hide();
                return;
            }

            if (selectedAbilityUI == null)
            {
                foreach (var ability in abilityLookup.GetPlayerAbilities((uint)player.Level))
                {
                    var button = ButtonPool.GetObject(abilitySelectionUI.transform);
                    var buttonComponent = button.GetComponent<AbilityButton>();
                    buttonComponent.Setup(ability);
                    buttonComponent.onClick.AddListener(() => AssignAbility(ability));
                    buttonList.Add(buttonComponent);
                }
            }

            var pos = abilityUI.transform.position;
            var offset = abilitySelectionUI.GetComponent<RectTransform>().rect.height / 1.5f;
            pos.y += offset;
            abilitySelectionUI.transform.position = pos;
            selectedAbilityUI = abilityUI;
            abilitySelectionUI.SetActive(true);
        }

        public void Hide()
        {
            abilitySelectionUI.SetActive(false);
            selectedAbilityUI = null;
            foreach (var button in buttonList)
            {
                ButtonPool.ReturnObject(button.gameObject, true);
                button.onClick.RemoveAllListeners();
            }
            buttonList.Clear();
        }
    }
}