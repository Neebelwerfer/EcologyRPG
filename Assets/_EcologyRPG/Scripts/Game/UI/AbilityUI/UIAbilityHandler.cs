using EcologyRPG.Core.Abilities;
using EcologyRPG.Game.Player;
using UnityEngine;

namespace EcologyRPG.Game.UI
{
    public class UIAbilityHandler : MonoBehaviour
    {
        private PlayerCharacter player;
        private AbilityDefintion weaponAttack;
        private AbilityDefintion dodgeAbility;
        private AbilityDefintion ability1;
        private AbilityDefintion ability2;
        private AbilityDefintion ability3;
        private AbilityDefintion ability4;

        [SerializeField] private AbilityUI weaponAttackUI;
        [SerializeField] private AbilityUI dodgeAbilityUI;
        [SerializeField] private AbilityUI ability1UI;
        [SerializeField] private AbilityUI ability2UI;
        [SerializeField] private AbilityUI ability3UI;
        [SerializeField] private AbilityUI ability4UI;
        [SerializeField] private GameObject abilitySelectionUI;

        private void Start()
        {

            player = PlayerManager.PlayerCharacter;
            AbilitySelectionUI.Instance.Setup(abilitySelectionUI);
            
        }

        public void SetAbilitySlot(AbilityUI abilitySlot, AbilityDefintion ability)
        {
            abilitySlot.SetAbility(ability);
        }
    }
}