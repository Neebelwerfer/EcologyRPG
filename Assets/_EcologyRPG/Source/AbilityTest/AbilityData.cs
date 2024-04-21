using UnityEngine;

namespace EcologyRPG.AbilityTest
{
    [System.Serializable]
    public class AbilityData
    {
        public string Name;
        public string IconPath;
        public uint ID;
        public string Description;
        public float Cooldown;
        public string ScriptPath;

        public AbilityData(string name, uint ID, string description, float cooldown)
        {
            Name = name;
            this.ID = ID;
            Description = description;
            Cooldown = cooldown;
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