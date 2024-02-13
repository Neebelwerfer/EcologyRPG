using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBar : MonoBehaviour
{
    [SerializeField] private Slider barSlider;
    [SerializeField] private Slider easeSlider;
    [SerializeField] private float lerpSpeed;
    [SerializeField] private string resourceName;
    private PlayerCharacter character;
    private bool initialized = false;

    
    public float maxValue;
    public float statValue; 
    
 
    // Start is called before the first frame update
    void Start()
    {
        character = FindObjectOfType<PlayerCharacter>();
        InitializeBar(character, resourceName);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!initialized)
        {
            InitializeBar(character, resourceName);
            initialized = true;
        }
        UpdateBar(character, resourceName);
    }
    public void LowerValue(float deplete)
    {
        statValue -= deplete;
    }
    public void InitializeBar(PlayerCharacter player, string resourceName)
    {
        maxValue = player.stats.GetResource(resourceName).MaxValue;
        barSlider.maxValue = maxValue;
        barSlider.value = barSlider.maxValue;
        easeSlider.maxValue = barSlider.maxValue;
        easeSlider.value = easeSlider.maxValue;
    }
    public void UpdateBar(PlayerCharacter player, string resourceName)
    {
        statValue = player.stats.GetResource(resourceName).CurrentValue;
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
