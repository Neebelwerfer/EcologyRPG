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

    public enum ShowOptions
    {
        Always,
        Never,
        WhenNonZero,
    }

    [Serializable]
    public class StatData
    {
        [Tooltip("The name of the stat")]
        public string name;
        [Tooltip("The display name of the stat")]
        public string displayName;
        public ShowOptions ShowOptions;
        [Tooltip("The default value of this stat")]
        public float baseValue;
        [Tooltip("The maximum value this stat can have")]
        public float MaxValue = float.MaxValue;
        [Tooltip("The minimum value this stat can have")]
        public float MinValue = float.MinValue;
        [TextArea(3, 10)]
        [Tooltip("A description of what this stat does")]
        public string description;
    }

    [Serializable]
    public class StatModification
    {
        public string StatName;
        public object Source;
        public int Order;
        public float Value;
        public StatModType ModType;
        [HideInInspector]
        public UnityEvent OnStatModChanged = new();

        public void UpdateValue(float newValue)
        {
            Value = newValue;
            OnStatModChanged.Invoke();
        }

        public StatModification(string name, float value, StatModType modType, int order, object source)
        {
            StatName = name;
            Value = value;
            ModType = modType;
            this.Order = order;
            Source = source;
        }

        public StatModification(string name, float value, StatModType modType, object source) : this(name, value, modType, (int)modType, source) { }

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
            finalValue = Mathf.Clamp(finalValue, Data.MinValue, Data.MaxValue);
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
                mod.OnStatModChanged.RemoveAllListeners();
                isDirty = true;
                Debug.Log("Removed modifier: " + mod.Value);
                return true;
            }
            return false;
        }

        public void AddModifier(StatModification mod)
        {
            isDirty = true;
            Modifiers.Add(mod);
            Modifiers.Sort(CompareModifierOrder);
            mod.OnStatModChanged.AddListener(() => isDirty = true);
            Debug.Log("Added modifier: " + mod.Value);
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