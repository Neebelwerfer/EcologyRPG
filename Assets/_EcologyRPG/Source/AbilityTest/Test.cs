using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using EcologyRPG.Core.Character;
using EcologyRPG.AbilityTest;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update

    static Script instance;
    static BaseCharacter GetPlayer()
    {
        return (Characters.Instance.GetCharactersByTag("Player")[0]);
    }

    static void Log(string message)
    {
        Debug.Log(message);
    }

    static DynValue Delay(float seconds)
    {
        return DynValue.NewYieldReq(new DynValue[] { DynValue.NewNumber(seconds) });
    }

    static string script = @"    
		        -- defines a factorial function
		        function fact (n)
                    Log('Getting player')
                    Delay(2)
                   local player = GetPlayer()
                    Log('Got player')
                    Delay(3)
                   local health = player:GetResource('Health')
                   Log('Health: ' .. health:GetCurrent())
                   local movementSpeed = player:GetStat('MovementSpeed')
                     Log('Movement Speed: ' .. movementSpeed:GetValue())
		        end

		        return fact(5)";

    void Start()
    {
       

        UserData.RegisterProxyType<ResourceContext, Resource>(r => new ResourceContext(r));
        UserData.RegisterProxyType<CharacterContext, BaseCharacter>(c => new CharacterContext(c));
        UserData.RegisterProxyType<StatContext, Stat>(s => new StatContext(s));

        instance = new Script();
        instance.Globals["GetPlayer"] = (System.Func<BaseCharacter>)GetPlayer;
        instance.Globals["Log"] = (System.Action<string>)Log;
        instance.Globals["Delay"] = (System.Func<float, DynValue>)Delay;

        StartCoroutine(TestCoroutine());
        
        
        //var res = instance.Call(fun);

        //DynValue res = Script.RunString(script);
    }

    IEnumerator TestCoroutine()
    {
        var fun = instance.LoadString(script);
        var cor = instance.CreateCoroutine(fun);

        foreach (var res in cor.Coroutine.AsTypedEnumerable())
        {
            var num = res.Number;
            yield return new WaitForSeconds((float)num);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
