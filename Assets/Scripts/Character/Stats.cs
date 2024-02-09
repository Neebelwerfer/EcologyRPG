using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ReadOnlyCollection<Stat> _stats;
        public ReadOnlyCollection<Attribute> _attributes;

        List<Stat> StatList;
        List<Attribute> AttributeList;

        public Stats()
        {
            StatList = new List<Stat>();
            _stats = StatList.AsReadOnly();

            AttributeList = new List<Attribute>();
            _attributes = AttributeList.AsReadOnly();

            var json = Resources.Load<TextAsset>("CharacterStats").text;
            var newList = JsonUtility.FromJson<SerializableStats>(json);

            foreach (StatData data in newList.Stats)
            {
                StatList.Add(new Stat(data));
            }

            foreach (AttributeData data in newList.Attributes)
            {
                AttributeList.Add(new Attribute(data, this));
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