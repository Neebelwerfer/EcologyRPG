using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EcologyRPG.Core.Character
{
    [Serializable]
    public class ResourceData
    {
        public string name;
        public string DisplayName;
        public string MaxValueStat;
    }

    public class Resource
    {
        public float CurrentValue { get { return currentValue; } set { ModifyCurrentValue(value); } }
        public float MaxValue { get { return MaxValueStat.Value; } }
        public readonly ResourceData Data;
        public UnityEvent<float> OnValueChanged;

        float currentValue;
        readonly Stat MaxValueStat;

        public Resource(ResourceData data, Stats stats)
        {
            Data = data;
            MaxValueStat = stats.GetStat(Data.MaxValueStat);
            currentValue = MaxValueStat.Value;
            OnValueChanged = new UnityEvent<float>();
        }

        public void SetCurrentValue(float value)
        {
            if(value == currentValue)
                return;
            currentValue = value;
            currentValue = Mathf.Clamp(currentValue, 0, MaxValueStat.Value);
            OnValueChanged.Invoke(currentValue);
        }

        public void ModifyCurrentValue(float value)
        {
            if(value == 0)
                return;
            currentValue += value;
            currentValue = Mathf.Clamp(currentValue, 0, MaxValueStat.Value);
            OnValueChanged.Invoke(currentValue);
        }

        public void Reset()
        {
            currentValue = MaxValueStat.Value;
            OnValueChanged.Invoke(currentValue);
        }

        public override bool Equals(object obj)
        {
            return obj is Resource resource &&
                   Data.name == resource.Data.name;
        }

        public override int GetHashCode()
        {
            return -1961139831 + EqualityComparer<string>.Default.GetHashCode(Data.name);
        }

        public static Resource operator +(Resource a, float b)
        {
            a.ModifyCurrentValue(b);
            return a;
        }

        public static Resource operator -(Resource a, float b)
        {
            a.ModifyCurrentValue(-b);
            return a;
        }

        public static Resource operator ++(Resource a)
        {
            a.ModifyCurrentValue(1);
            return a;
        }

        public static Resource operator --(Resource a)
        {
            a.ModifyCurrentValue(-1);
            return a;
        }

        public static bool operator ==(Resource a, float b)
        {
            return a.currentValue == b;
        }

        public static bool operator !=(Resource a, float b)
        {
            return a.currentValue != b;
        }

        public static bool operator >(Resource a, float b)
        {
            return a.currentValue > b;
        }

        public static bool operator <(Resource a, float b)
        {
            return a.currentValue < b;
        }

        public static bool operator >=(Resource a, float b)
        {
            return a.currentValue >= b;
        }

        public static bool operator <=(Resource a, float b)
        {
            return a.currentValue <= b;
        }

        public static float operator /(Resource a, float b)
        {
            a.SetCurrentValue(a.currentValue / b);
            return a.currentValue;
        }
    }
}