using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIHandler : MonoBehaviour
{


    [SerializeField] GameObject StatBars;
    [SerializeField] GameObject Abilities;

    //functions for toggling UI elements on and off
    public void ToggleStatBars(bool toggleState)
    {
        StatBars.SetActive(toggleState);
    }
    public void ToggleAbilities(bool toggleState)
    {
        Abilities.SetActive(toggleState);
    }

    public void ToggleUI(bool toggleState)
    {
        ToggleAbilities(toggleState);
        ToggleStatBars(toggleState);
    }
}
