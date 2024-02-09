using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Character
{
    [Serializable]
    public class StatData
    {
        public string name;
        public string displayName;
        public float baseValue;
        [TextArea(3, 10)]
        public string description;
    }

    public class Stats
    {
        List<Stat> StatList;
        public Stats()
        {
            var json = Resources.Load<TextAsset>("Stats").text;
            var newList = JsonUtility.FromJson<SerializableList<StatData>>(json);

            StatList = new List<Stat>();
            foreach (StatData stat in newList.list)
            {
                StatList.Add(new Stat(stat.name, stat.baseValue, stat.description, stat.displayName));
            }
        }

        public Stat GetStat(string name)
        {
            foreach (Stat stat in StatList)
            {
                if (stat.Name == name)
                {
                    return stat;
                }
            }
            Debug.LogError("Couldn't find Stat " + name);
            return null;
        }
    }

}