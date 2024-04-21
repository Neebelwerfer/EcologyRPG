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
                    local player = GetPlayer()
                    for i = 1, 20 do
                        Log('Looping')
                        Delay(1)
                        Log('player position: ' .. player.GetPosition().ToString())
                        player.SetVelocity(Vector3(1, 1, 1))
                    end
                   
		        end

		        return fact(5)";

    void Start()
    {
       
        AbilityManager.Create();

        instance = new Script();
        instance.Globals["GetPlayer"] = (System.Func<BaseCharacter>)GetPlayer;
        instance.Globals["Log"] = (System.Action<string>)Log;
        instance.Globals["Delay"] = (System.Func<float, DynValue>)Delay;
        instance.Globals["Vector3"] = (System.Func<float, float, float, Vector3Context>)Vector3Context._Vector3;

        StartCoroutine(TestCoroutine());
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
