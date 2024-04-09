using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using EcologyRPG.Core.Abilities.AbilityData;
using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.UI;
using EcologyRPG.GameSystems.PlayerSystems;
using UnityEngine.InputSystem;
using System;
using TMPro;
using System.Collections;
using EcologyRPG.GameSystems.Abilities;

namespace EcologyRPG.GameSystems.UI
{
    public class AbilityUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, ITooltip
    {
        [Header("Ability")]
        public AbilitySlots abilitySlot;
        [SerializeField] private bool blockedByUI = true;
        [SerializeField] private InputActionReference abilityAction;
        [SerializeField] private Image abilityImage;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private TextMeshProUGUI actionText;

        private string abilityName;
        private PlayerCharacter player;
        private float cooldown;
        private PlayerAbilityDefinition ability;
        private Sprite abilitySprite;   

        bool coroutineStarted = false;
        bool isPressed = false;

        Color baseColor;
        Color baseGrayColor;

        void Start()
        {
            baseColor = abilityImage.color;
            baseGrayColor = backgroundImage.color;
            abilityImage.fillAmount = 1;
            player = Player.PlayerCharacter;
            Player.PlayerAbilities.AddListener(abilitySlot, SetAbility);
            ability = (PlayerAbilityDefinition)Player.PlayerAbilities.GetAbility(abilitySlot);
            abilityAction.action.started += ActivateAbility;
            abilityAction.action.canceled += StoppedAction;
            AbilityManager.OnToxicModeChanged.AddListener(OnToxicModeChanged);
            SetUpAbilityUI();
        }

        private void OnToxicModeChanged(bool arg0)
        {
            if (ability == null) return;
            if (ability.ToxicAbility != null)
            {
                if (arg0)
                {
                    abilityImage.color = Game.Settings.ToxicAbilityReady;
                    backgroundImage.color = Game.Settings.ToxicAbilityNotReady;
                }
                else
                {
                    abilityImage.color = baseColor; 
                    backgroundImage.color = baseGrayColor;
                }
            }
        }

        private void OnEnable()
        {
            abilityAction.action.Enable();
            actionText.text = abilityAction.action.GetBindingDisplayString(0);
            InvokeRepeating(nameof(UpdateState), 0f, 0.1f);
        }

        private void OnDisable()
        {
            CancelInvoke(nameof(UpdateState));
            abilityAction.action.Disable();
        }

        private void Update()
        {
            if (isPressed)
            {
                if (ability == null) return;
                if (Game.Instance.CurrentState == Game_State.Playing
                    && (!blockedByUI || !EventSystem.current.IsPointerOverGameObject()))
                    InvokeRepeating(nameof(UpdateAction), 0f, 0.1f);
                isPressed = false;
            }
        }

        private void UpdateState()
        {
            if (ability == null) return;
            if (ability.state.Equals(AbilityStates.ready))
            {
                if (ability.CanActivate(player))
                    abilityImage.fillAmount = 1;
                else abilityImage.fillAmount = 0;
            }
            else if (ability.state.Equals(AbilityStates.cooldown))
            {
                if(!coroutineStarted)
                {
                    StartCoroutine(nameof(UpdateCooldown));
                    coroutineStarted = true;
                }
            }
        }

        IEnumerator UpdateCooldown()
        {
            while (ability.state.Equals(AbilityStates.cooldown))
            {
                abilityImage.fillAmount = 1 - (ability.remainingCooldown) / cooldown;
                yield return null;
            }
            coroutineStarted = false;
            abilityImage.fillAmount = 1;
        }

        public void SetUpAbilityUI()
        {
            if (ability == null)
            {
                abilitySprite = null;
                abilityImage.sprite = abilitySprite;
                backgroundImage.sprite = abilitySprite;
                return;
            };

            abilityName = ability.DisplayName;
            cooldown = ability.Cooldown;
            if (ability.Icon != null)
                abilitySprite = ability.Icon;
            abilityImage.sprite = abilitySprite;
            backgroundImage.sprite = abilitySprite;
        }

        private void ActivateAbility(InputAction.CallbackContext context)
        {
            if (ability == null) return;
            isPressed = true;
        }

        private void StoppedAction(InputAction.CallbackContext context)
        {
            CancelInvoke(nameof(UpdateAction));
        }

        private void UpdateAction()
        {
            if (abilityAction.action.IsPressed())
            {
                var castInfo = new CastInfo { activationInput = abilityAction.action, castPos = player.CastPos, owner = player };
                ability.Activate(castInfo);
            }
        }

        public void SetAbility(AbilityDefintion newAbility)
        {
            ability = (PlayerAbilityDefinition)newAbility;
            SetUpAbilityUI();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Tooltip.HideTooltip(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (ability == null) return;
            Tooltip.ShowTooltip(this);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            AbilitySelectionUI.Instance.Show(this);
        }

        public TooltipData GetTooltipData()
        {
            if(ability is PlayerAbilityDefinition p)
            {
                return new TooltipData() { Title = abilityName, Icon = abilitySprite, Description = p.Description };
            }
            return new TooltipData() { Title = abilityName, Icon = abilitySprite, Description = "" };
        }

        public void OnDestroy()
        {
            abilityAction.action.started -= ActivateAbility;
            abilityAction.action.canceled -= StoppedAction;
        }
    }
}
