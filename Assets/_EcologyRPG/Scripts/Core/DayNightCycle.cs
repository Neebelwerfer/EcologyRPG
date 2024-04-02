using EcologyRPG._Core.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG._Core
{
    public class DayNightCycle : SystemBehavior, IUpdateSystem
    {
        private Vector3 rot = Vector3.zero;
        [SerializeField] private float cycleDuration;
        [SerializeField] private GameObject day;
        [SerializeField] private GameObject night;
        private float changePerSec;

        public bool Enabled => true;

        public DayNightCycle(GameObject day, GameObject night, float cycleDuration)
        {
            this.day = day;
            this.night = night;
            this.cycleDuration = cycleDuration;
            changePerSec = 360 / cycleDuration;
        }

        public void OnUpdate()
        {
            rot.x = changePerSec * Time.deltaTime;
            day.transform.Rotate(rot, Space.World);
            night.transform.Rotate(rot, Space.World);
        }
    }
}

