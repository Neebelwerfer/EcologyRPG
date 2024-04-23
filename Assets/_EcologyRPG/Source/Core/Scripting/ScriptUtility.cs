using MoonSharp.Interpreter;
using UnityEngine;

namespace EcologyRPG.Core.Scripting
{
    public static class ScriptUtility
    {
        internal static void AddContext(Script script)
        {
            script.Globals["Delay"] = (System.Func<float, DynValue>)Delay;
            script.Globals["DrawLine"] = (System.Action<Vector3, Vector3, Color, float>)DrawLine;
            script.Globals["DrawRay"] = (System.Action<Vector3, Vector3, Color, float>)DrawRay;
            script.Globals["Log"] = (System.Action<string>)Log;
        }

        static DynValue Delay(float seconds)
        {
            return DynValue.NewYieldReq(new DynValue[] { DynValue.NewNumber(seconds) });
        }

        static void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
        {
            Debug.DrawLine(start, end, color, duration);
        }

        static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration = 0.2f)
        {
            Debug.DrawRay(start, dir, color, duration);
        }

        static void Log(string message)
        {
            Debug.Log(message);
        }
    }
}