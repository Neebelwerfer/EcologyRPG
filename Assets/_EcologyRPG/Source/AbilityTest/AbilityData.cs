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
        public string Name;
        public string IconPath;
        public AbilityCategory Category;
        public uint ID;
        public string Description;
        public float Cooldown;
        public string ScriptPath;

        public AbilityData(string name, AbilityCategory abilityCategory, uint ID, string description, float cooldown)
        {
            Name = name;
            Category = abilityCategory;
            this.ID = ID;
            Description = description;
            Cooldown = cooldown;
        }

        public Script LoadBehaviour()
        {
            var script = System.IO.File.ReadAllText(ScriptPath);
            var context = AbilityManager.CreateContext();
            context.DoString(script);
            return context;
        }

        public string GetDescription()
        {
            return $"{Name}\nCooldown: {Cooldown}\n{Description}";
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
        public AbilityData[] data;

        public AbilityData this[int index]
        {
            get => data[index];
            set => data[index] = value;
        }

        public SerializedDataArray()
        {
            data = new AbilityData[0];
        }

        public static SerializedDataArray Load()
        {
            return JsonUtility.FromJson<SerializedDataArray>(System.IO.File.ReadAllText(AbilityManager.AbilityDataPath));
        }
    }
}