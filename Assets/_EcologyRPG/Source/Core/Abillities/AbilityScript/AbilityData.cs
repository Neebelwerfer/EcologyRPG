using EcologyRPG.Core.Abilities;
using MoonSharp.Interpreter;
using UnityEngine;

namespace EcologyRPG.AbilityScripting
{
    [System.Serializable]
    public enum AbilityCategory
    {
        Extra,
        Player,
        NPC,
    }

    [System.Serializable]
    public class AbilityData
    {
        public string abilityName;
        public AbilityCategory Category;
        public uint ID;
        public float Cooldown;
        public string ScriptPath;

        public AbilityData(string name, AbilityCategory abilityCategory, uint ID, float cooldown)
        {
            abilityName = name;
            Category = abilityCategory;
            this.ID = ID;
            Cooldown = cooldown;
        }

        public Script LoadBehaviour()
        {
            var script = System.IO.File.ReadAllText(ScriptPath);
            var context = AbilityManager.CreateContext();
            context.DoString(script);
            return context;
        }

        public static AbilityData[] LoadAll()
        {
            var data = SerializedDataArray.Load();
            Debug.Log("Loaded data: " + data.data.Length); 
            return data.data;
        }
    }

    [System.Serializable]
    public class SerializedDataArray
    {
        public uint IDCounter;
        public AbilityData[] data;

        public AbilityData this[int index]
        {
            get => data[index];
            set => data[index] = value;
        }

        public SerializedDataArray()
        {
            IDCounter = 0;
            data = new AbilityData[0];
        }

        public static SerializedDataArray Load()
        {
            return JsonUtility.FromJson<SerializedDataArray>(System.IO.File.ReadAllText(AbilityManager.AbilityDataPath));
        }
    }
}