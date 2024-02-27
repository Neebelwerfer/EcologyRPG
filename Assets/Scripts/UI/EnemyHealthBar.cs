using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider barSlider;
    [SerializeField] private Slider easeSlider;
    [SerializeField] private float lerpSpeed;
    [SerializeField] private string resourceName;
    [SerializeField] private NPCCharacter character;
    [SerializeField] private GameObject easingBar;
    [SerializeField] private GameObject healthBar;

    private bool initialized = false;

    public float maxHealth = 100;
    public float currentHealth;

    private void Start()
    {
        InitializeBar(character, resourceName);
        barSlider.maxValue = maxHealth;
        barSlider.value = maxHealth;
        currentHealth = maxHealth;
    }
    private void Update()
    {
        if (!initialized) InitializeBar(character, resourceName);

        UpdateBar(character, resourceName);
        VisibleBar();
    }

    public void InitializeBar(NPCCharacter nPC, string resourceName)
    {
        maxHealth = nPC.stats.GetResource(resourceName).MaxValue;
        barSlider.maxValue = maxHealth;
        barSlider.value = barSlider.maxValue;
        easeSlider.maxValue = barSlider.maxValue;
        easeSlider.value = easeSlider.maxValue;
        if (maxHealth != 0) initialized = true;
    }
    public void UpdateBar(NPCCharacter nPC, string resourceName)
    {
        currentHealth = nPC.stats.GetResource(resourceName).CurrentValue;
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
        if(character.stats.GetResource(resourceName).MaxValue == character.stats.GetResource(resourceName).CurrentValue)
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