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

    public float maxValue;
    public float statValue;

    
}