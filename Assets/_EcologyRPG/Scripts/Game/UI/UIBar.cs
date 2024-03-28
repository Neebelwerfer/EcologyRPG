using EcologyRPG.Game.Player;
using UnityEngine;
using UnityEngine.UI;

namespace EcologyRPG.Game.UI
{
    public class UIBar : MonoBehaviour
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
        private bool initialized = false;

        public float maxValue;
        public float statValue;

        // Start is called before the first frame update
        void Start()
        {
            character = PlayerManager.Instance.GetPlayerCharacter();
            InitializeBar(character, resourceName);

        }

        // Update is called once per frame
        void Update()
        {
            if (!initialized)
            {
                InitializeBar(character, resourceName);
            }
            UpdateBar(character, resourceName);
        }

        public void InitializeBar(PlayerCharacter player, string resourceName)
        {
            icon.sprite = statIcon;
            shell.sprite = statShell;
            fillImage.sprite = fillSprite;
            easeImage.sprite = fillSprite;
            backgroundImage.sprite = fillSprite;
            maxValue = player.Stats.GetResource(resourceName).MaxValue;
            barSlider.maxValue = maxValue;
            barSlider.value = barSlider.maxValue;
            easeSlider.maxValue = barSlider.maxValue;
            easeSlider.value = easeSlider.maxValue;
            if (maxValue != 0) initialized = true;
        }
        public void UpdateBar(PlayerCharacter player, string resourceName)
        {
            statValue = player.Stats.GetResource(resourceName).CurrentValue;
            if (barSlider.value != statValue)
            {
                barSlider.value = statValue;
            }

            if (barSlider.value != easeSlider.value)
            {
                easeSlider.value = Mathf.Lerp(easeSlider.value, statValue, lerpSpeed);
            }
        }
    }
}

