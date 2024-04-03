using EcologyRPG.Core.Character;
using EcologyRPG.Game.NPC;
using UnityEngine;
using UnityEngine.UI;

namespace EcologyRPG.Game.UI
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField] private Slider barSlider;
        [SerializeField] private Slider easeSlider;
        [SerializeField] private float lerpSpeed;
        [SerializeField] private string resourceName;
        [SerializeField] private GameObject easingBar;
        [SerializeField] private GameObject healthBar;

        private bool initialized = false;

        public float maxHealth = 100;
        public float currentHealth;

        CharacterBinding characterBinding;

        private void Start()
        {
            characterBinding = GetComponentInParent<CharacterBinding>();
            characterBinding.CharacterUpdated.AddListener(() => InitializeBar(characterBinding.Character, resourceName));
            InitializeBar(characterBinding.Character, resourceName);
            barSlider.maxValue = maxHealth;
            barSlider.value = maxHealth;
            currentHealth = maxHealth;
        }
        private void Update()
        {
            if (!initialized)
            {
                characterBinding = GetComponentInParent<CharacterBinding>();
                InitializeBar(characterBinding.Character, resourceName);
            }

            UpdateBar(characterBinding.Character, resourceName);
            VisibleBar();
        }

        public void InitializeBar(BaseCharacter nPC, string resourceName)
        {
            if(nPC == null) return;
            maxHealth = nPC.Stats.GetResource(resourceName).MaxValue;
            barSlider.maxValue = maxHealth;
            barSlider.value = barSlider.maxValue;
            easeSlider.maxValue = barSlider.maxValue;
            easeSlider.value = easeSlider.maxValue;
            if (maxHealth != 0) initialized = true;
        }
        public void UpdateBar(BaseCharacter nPC, string resourceName)
        {
            currentHealth = nPC.Stats.GetResource(resourceName).CurrentValue;
            if (barSlider.value != currentHealth)
            {
                barSlider.value = currentHealth;
            }

            if (barSlider.value != easeSlider.value)
            {
                easeSlider.value = Mathf.Lerp(easeSlider.value, currentHealth, lerpSpeed);
            }
        }
        public void VisibleBar()
        {
            if (characterBinding.Character.Stats.GetResource(resourceName).MaxValue == characterBinding.Character.Stats.GetResource(resourceName).CurrentValue)
            {
                easingBar.SetActive(false);
                healthBar.SetActive(false);
            }
            else
            {
                easingBar.SetActive(true);
                healthBar.SetActive(true);
            }
        }
    }
}