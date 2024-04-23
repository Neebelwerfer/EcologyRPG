using MoonSharp.Interpreter;
using System;
using UnityEngine;

namespace EcologyRPG.Core.Abilities
{
    [System.Serializable]
    public enum UpdateType
    {
        Update,
        FixedUpdate
    }

    public class SerializedConditionArray
    {
        public int IDCounter;
        public ConditionData[] data;

        public SerializedConditionArray()
        {
            data = new ConditionData[0];
        }
    }

    [System.Serializable]
    public class ConditionData
    {
        public UpdateType updateType;
        public int ID;
        public string ConditionName;
        public string ConditionDescription;
        public string ScriptPath;
        [SerializeReference]
        public GlobalVariable[] _DefaultGlobalVariables;

        public ConditionData(string name, int ID, UpdateType updateType)
        {
            ConditionName = name;
            this.updateType = updateType;
            this.ID = ID;
        }

        public Script LoadBehaviour()
        {
            var script = System.IO.File.ReadAllText(ScriptPath);
            var context = AbilityManager.CreateScriptContext();
            context.DoString(script);
            return context;
        }

        public void LoadDefaultVariables(Script context)
        {
            foreach (var variable in _DefaultGlobalVariables)
            {
                context.Globals[variable.Name] = variable.GetDynValue();
            }
        }
    }
}