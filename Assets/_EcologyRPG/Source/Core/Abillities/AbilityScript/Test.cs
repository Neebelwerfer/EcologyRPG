using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using EcologyRPG.Core.Character;
using EcologyRPG.AbilityScripting;
using EcologyRPG.Core.Abilities;

[DefaultExecutionOrder(1000)]
public class Test : MonoBehaviour
{
    // Start is called before the first frame update

    static Script instance;

    public AbilityReference ability;

    static BaseCharacter GetPlayer()
    {
        return (Characters.Instance.GetCharactersByTag("Player")[0]);
    }

    private void Awake()
    {
    }
    void Start()
    {
        ability.Init(GetPlayer());
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            ability.Cast();
        }
    }
}
