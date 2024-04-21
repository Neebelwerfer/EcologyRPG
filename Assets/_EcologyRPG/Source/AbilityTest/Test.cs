using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using EcologyRPG.Core.Character;
using EcologyRPG.AbilityTest;

[DefaultExecutionOrder(1000)]
public class Test : MonoBehaviour
{
    // Start is called before the first frame update

    static Script instance;
    static BaseCharacter GetPlayer()
    {
        return (Characters.Instance.GetCharactersByTag("Player")[0]);
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

    private void Awake()
    {
    }
    void Start()
    {
        AbilityManager.Create();
        var data = SerializedDataArray.Load();
        AbilityManager.Current.CastAbility(data[0], new CastContext(new EcologyRPG.Core.Abilities.CastInfo() { owner = GetPlayer() }));

        //instance = new Script();
        //instance.Globals["GetPlayer"] = (System.Func<BaseCharacter>)GetPlayer;
        //instance.Globals["Log"] = (System.Action<string>)Log;
        //instance.Globals["Delay"] = (System.Func<float, DynValue>)Delay;
        //instance.Globals["Vector3"] = (System.Func<float, float, float, Vector3Context>)Vector3Context._Vector3;

        //StartCoroutine(TestCoroutine());
    }

    IEnumerator TestCoroutine()
    {
        var fun = instance.LoadString(script);
        var cor = instance.CreateCoroutine(fun);

        foreach (var res in cor.Coroutine.AsTypedEnumerable())
        {
            if(res.Type == DataType.Number)
            {
                var num = res.Number;
                yield return new WaitForSeconds((float)num);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
