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

    AbilityReference ability;

    static BaseCharacter GetPlayer()
    {
        return (Characters.Instance.GetCharactersByTag("Player")[0]);
    }

    private void Awake()
    {
    }
    void Start()
    {
        EcologyRPG.AbilityScripting.AbilityManager.Create();
        var data = SerializedDataArray.Load();
        ability = new AbilityReference(data[0], GetPlayer());
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
