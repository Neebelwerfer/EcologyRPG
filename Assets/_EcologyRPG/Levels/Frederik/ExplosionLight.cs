using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionLight : MonoBehaviour
{
    private Light pointLight;
    [SerializeField] private float initialIntensity = 100f;
    [SerializeField] private float multiplier = 1f;
    [SerializeField] private float fadeSpeed = 100f;
 
    // Start is called before the first frame update
    void Start()
    {
        pointLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        pointLight.intensity = initialIntensity - multiplier;
        multiplier += (Time.deltaTime*fadeSpeed);
    }
}
