using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameLight : MonoBehaviour
{
    // Start is called before the first frame update
    private Light pointLight;
    [SerializeField] float multiplier = 1f;
    [SerializeField] float fadeSpeed = 0.25f;
    private void Start()
    {
        pointLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        pointLight.intensity = Random.Range(1,5) * multiplier;
        multiplier -= fadeSpeed * Time.deltaTime;
    }
}
