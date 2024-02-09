using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Character
{
    [Serializable]
    public class SerializableStats
    {
        public List<AttributeData> Attributes;
        public List<StatData> Stats;

        public SerializableStats(List<StatData> stats, List<AttributeData> attributes)
        {
            Stats = stats;
            Attributes = attributes;
        }
    }

    public class Stats
    {
        List<Stat> StatList;
        List<Attribute> AttributeList;

        public Stats()
        {
            var json = Resources.Load<TextAsset>("CharacterStats").text;
            var newList = JsonUtility.FromJson<SerializableStats>(json);

            StatList = new List<Stat>();
            foreach (StatData data in newList.Stats)
            {
                StatList.Add(new Stat(data));
            }

            AttributeList = new List<Attribute>();
            foreach (AttributeData data in newList.Attributes)
            {
                AttributeList.Add(new Attribute(data));
            }
        }

        public Stat GetStat(string name)
        {
            foreach (Stat stat in StatList)
            {
                if (stat.Data.name == name)
                {
                    return stat;
                }
            }
            Debug.LogError("Couldn't find Stat " + name);
            return null;
        }
    }

}