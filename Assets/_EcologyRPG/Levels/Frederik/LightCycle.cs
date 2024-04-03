using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LightCycle : MonoBehaviour
{
    private Vector3 rot = Vector3.zero;
    [SerializeField] private float cycleDuration;
    [SerializeField] private GameObject day;
    [SerializeField] private GameObject night;
    private float changePerSec;

    private void Awake()
    {
        changePerSec = 360/cycleDuration;
    }
    private void Update()
    {
        rot.x = changePerSec* Time.deltaTime;
        day.transform.Rotate(rot, Space.World);
        night.transform.Rotate(rot, Space.World);
    }
}
