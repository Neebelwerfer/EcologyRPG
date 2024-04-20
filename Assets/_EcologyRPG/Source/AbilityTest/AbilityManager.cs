using MoonSharp.Interpreter;
using System;

namespace EcologyRPG.AbilityTest
{
    public class AbilityManager
    {
        public static AbilityManager Current;
        Script scriptInstance;
        AbilityManager() 
        { 
            scriptInstance = new Script();
            UserData.RegisterAssembly();

        }

        public static void Create()
        {
            Current ??= new AbilityManager();
        }

        public static void Delete()
        {
            Current = null;
        }

        public static void AddClass(string name, object obj)
        {
            Current.scriptInstance.Globals[name] = obj;
        }

        public static void AddStaticClass(string name, Type obj)
        {
            Current.scriptInstance.Globals[name] = obj;
        }
    }
}