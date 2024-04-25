using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EcologyRPG.Core.Abilities;
using UnityEngine.InputSystem;
using EcologyRPG.GameSystems.Interactables;

namespace EcologyRPG.GameSystems.UI

{
    public class WaterToxicToggle : MonoBehaviour
    {
        [SerializeField] private GameObject waterBar;
        [SerializeField] private GameObject toxicWaterBar;
        [SerializeField] private GameObject inactiveWaterBar;
        [SerializeField] private GameObject inactiveToxicWaterBar;
        
        public InputActionReference ToggleToxicMode;

        private void Start()
        {
            ToggleBetweenToxicAndWater(AbilityManager.UseToxic);
        }

        private void Update()
        {
            if (ToggleToxicMode.action.ReadValue<float>() == 0)
            {
                ToggleBetweenToxicAndWater(AbilityManager.UseToxic);
            }
        }

        public void ToggleBetweenToxicAndWater(bool UseToxic)
        {
            if (UseToxic)
            {
                waterBar.SetActive(false);
                inactiveToxicWaterBar.SetActive(false);
                inactiveWaterBar.SetActive(true);
                toxicWaterBar.SetActive(true);
            }
            else if (!UseToxic)
            {
                waterBar.SetActive(true);
                inactiveToxicWaterBar.SetActive(true);
                inactiveWaterBar.SetActive(false);
                toxicWaterBar.SetActive(false);

            }
        }
    }
}
