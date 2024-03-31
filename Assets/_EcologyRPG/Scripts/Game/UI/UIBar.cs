using EcologyRPG.Core.Character;
using EcologyRPG.Core.UI;
using EcologyRPG.Game.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EcologyRPG.Game.UI
{
    public class UIBar : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ITooltip
    {
        [SerializeField] private Slider barSlider;
        [SerializeField] private Slider easeSlider;
        [SerializeField] private float lerpSpeed;
        [SerializeField] private string resourceName;

        [SerializeField] private Sprite fillSprite;
        [SerializeField] private Image fillImage;
        [SerializeField] private Image easeImage;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image icon;
        [SerializeField] private Image shell;
        [SerializeField] private Sprite statIcon;
        [SerializeField] private Sprite statShell;

        private PlayerCharacter character;
        private Resource resource;
        private bool initialized = false;

        public float maxValue;
        public float resourceValue;

        // Start is called before the first frame update
        void Start()
        {
            character = PlayerManager.Player;
            InitializeBar(character, resourceName);

        }

        // Update is called once per frame
        void Update()
        {
            if (!initialized)
            {
                InitializeBar(character, resourceName);
            }
            else
            {
                UpdateBar();
            }
        }

        public void InitializeBar(PlayerCharacter player, string resourceName)
        {
            resource = player.Stats.GetResource(resourceName);
            icon.sprite = statIcon;
            shell.sprite = statShell;
            fillImage.sprite = fillSprite;
            easeImage.sprite = fillSprite;
            backgroundImage.sprite = fillSprite;
            maxValue = resource.MaxValue;
            barSlider.maxValue = maxValue;
            barSlider.value = barSlider.maxValue;
            easeSlider.maxValue = barSlider.maxValue;
            easeSlider.value = easeSlider.maxValue;
            if (maxValue != 0) initialized = true;
        }

        public void UpdateMaxValue()
        {
            maxValue = resource.MaxValue;
            barSlider.maxValue = maxValue;
            easeSlider.maxValue = barSlider.maxValue;
        }
        public void UpdateBar()
        {
            UpdateMaxValue();
            resourceValue = resource.CurrentValue;
            if (barSlider.value != resourceValue)
            {
                barSlider.value = resourceValue;
            }

            if (barSlider.value != easeSlider.value)
            {
                easeSlider.value = Mathf.Lerp(easeSlider.value, resourceValue, lerpSpeed);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Tooltip.ShowTooltip(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Tooltip.HideTooltip(this);
        }

        public TooltipData GetTooltipData()
        {
            return new TooltipData { Title = resourceName, Description = $"{(int)resourceValue}/{(int)maxValue}" };
        }
    }
}

