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
        public List<ResourceData> Resources;

        public SerializableStats(List<StatData> stats, List<AttributeData> attributes, List<ResourceData> resources)
        {
            Stats = stats;
            Attributes = attributes;
            Resources = resources;
        }
    }

    public class Stats
    {
        public ReadOnlyCollection<Stat> _stats;
        public ReadOnlyCollection<Attribute> _attributes;
        public ReadOnlyCollection<Resource> _resources;

        List<Stat> StatList;
        List<Attribute> AttributeList;
        List<Resource> ResourceList;

        public Stats()
        {
            StatList = new List<Stat>();
            _stats = StatList.AsReadOnly();

            AttributeList = new List<Attribute>();
            _attributes = AttributeList.AsReadOnly();

            ResourceList = new List<Resource>();
            _resources = ResourceList.AsReadOnly();

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

            foreach (ResourceData data in newList.Resources)
            {
                ResourceList.Add(new Resource(data, this));
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

        public Attribute GetAttribute(string name)
        {
            foreach (Attribute attribute in AttributeList)
            {
                if (attribute.data.name == name)
                {
                    return attribute;
                }
            }
            Debug.LogError("Couldn't find Attribute " + name);
            return null;
        }

        public Resource GetResource(string name)
        {
            foreach (Resource resource in ResourceList)
            {
                if (resource.Data.name == name)
                {
                    return resource;
                }
            }
            Debug.LogError("Couldn't find Resource " + name);
            return null;
        }
    }

}