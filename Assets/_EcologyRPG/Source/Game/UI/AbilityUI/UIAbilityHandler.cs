using EcologyRPG.AbilityScripting;
using EcologyRPG.Core.Abilities;
using EcologyRPG.GameSystems.PlayerSystems;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EcologyRPG.GameSystems.UI
{
    public class UIAbilityHandler : MonoBehaviour
    {
        private PlayerCharacter player;
        [SerializeField] private GameObject abilitySelectionUI;
        [SerializeField] private InputActionReference ToxicSwitchAction;

        private void Start()
        {
            ToxicSwitchAction.action.Enable();
            ToxicSwitchAction.action.canceled += ToggleToxic;
            player = Player.PlayerCharacter;
            AbilitySelectionUI.Instance.Setup(abilitySelectionUI);
        }

        void ToggleToxic(InputAction.CallbackContext context)
        {
            AbilityManager.UseToxic = !AbilityManager.UseToxic;
            AbilityManager.OnToxicModeChanged.Invoke(AbilityManager.UseToxic);
        }

        public void SetAbilitySlot(AbilityUI abilitySlot, PlayerAbilityReference ability)
        {
            abilitySlot.SetAbility(ability);
        }

        private void OnDestroy()
        {
            ToxicSwitchAction.action.Disable();
            ToxicSwitchAction.action.canceled -= ToggleToxic;
        }
    }
}