using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;

namespace Character
{
    public enum StatModType
    {
        Flat = 100,
        PercentAdd = 200,
        PercentMult = 300
    }

    [Serializable]
    public class StatData
    {
        public string name;
        public string displayName;
        public float baseValue;
        [TextArea(3, 10)]
        public string description;
    }

    public class StatModification
    {
        public object Source;

        public int Order;
        public float Value;
        public StatModType ModType;

        public StatModification(float value, StatModType modType, int order, object source)
        {
            Value = value;
            ModType = modType;
            this.Order = order;
            Source = source;
        }

        public StatModification(float value, StatModType modType, object source) : this(value, modType, (int)modType, source) { }

    }

    public class Stat
    {
        public StatData Data;

        public UnityEvent<float> OnStatChanged = new UnityEvent<float>();

        bool isDirty = true;

        float finalValue;

        public float Value
        {
            get
            {
                if (isDirty)
                {
                    CalculateFinalValue();
                    isDirty = false;
                    OnStatChanged.Invoke(finalValue);
                }
                return finalValue;
            }
        }

        public ReadOnlyCollection<StatModification> StatModifiers;
        List<StatModification> Modifiers;

        public Stat(string name, float baseValue, string description, string displayName)
        {
            Data = new StatData();
            Data.name = name;
            Data.baseValue = baseValue;
            Data.description = description;
            Data.displayName = displayName;
            Modifiers = new List<StatModification>();
            StatModifiers = Modifiers.AsReadOnly();
        }

        public Stat(StatData Data)
        {
            this.Data = Data;
            Modifiers = new List<StatModification>();
            StatModifiers = Modifiers.AsReadOnly();
        }

        void CalculateFinalValue()
        {
            finalValue = Data.baseValue;
            float sumPercentAdd = 0;

            for (int i = 0; i < Modifiers.Count; i++)
            {
                var mod = Modifiers[i];

                if (mod.ModType == StatModType.Flat)
                {
                    finalValue += mod.Value;
                }
                else if (mod.ModType == StatModType.PercentAdd)
                {
                    sumPercentAdd += mod.Value;


                    if (i + 1 >= Modifiers.Count || Modifiers[i + 1].ModType != StatModType.PercentAdd)
                    {
                        finalValue *= 1 + sumPercentAdd;
                        sumPercentAdd = 0;
                    }
                }
                else if (mod.ModType == StatModType.PercentMult)
                {
                    finalValue *= 1 + mod.Value;
                }
            }
        }

        public bool RemoveAllModifiersFromSource(object source)
        {
            bool didRemove = false;

            for (int i = Modifiers.Count - 1; i >= 0; i--)
            {
                if (Modifiers[i].Source == source)
                {
                    isDirty = true;
                    didRemove = true;
                    Modifiers.RemoveAt(i);
                }
            }
            return didRemove;
        }

        public bool RemoveModifier(StatModification mod)
        {
            if (Modifiers.Remove(mod))
            {
                isDirty = true;
                return true;
            }
            return false;
        }


        public void AddModifier(StatModification mod)
        {
            isDirty = true;
            Modifiers.Add(mod);
            Modifiers.Sort(CompareModifierOrder);
        }

        private int CompareModifierOrder(StatModification a, StatModification b)
        {
            if (a.Order < b.Order)
                return -1;
            else if (a.Order > b.Order)
                return 1;
            return 0;
        }
    }
}