using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EcologyRPG._Core.Character
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
        public float CurrentValue { get { return currentValuePercent * MaxValue; } set { ModifyCurrentValue(value); } }
        public float MaxValue { get { return MaxValueStat.Value; } }
        public readonly ResourceData Data;
        public UnityEvent<float> OnValueChanged;

        float currentValuePercent;
        readonly Stat MaxValueStat;

        public Resource(ResourceData data, Stats stats)
        {
            Data = data;
            MaxValueStat = stats.GetStat(Data.MaxValueStat);
            currentValuePercent = 1;
            OnValueChanged = new UnityEvent<float>();
        }

        public void SetCurrentValue(float value)
        {
            if(value == CurrentValue)
                return;
            var percent = value / MaxValue;
            currentValuePercent = percent;
            currentValuePercent = Mathf.Clamp(currentValuePercent, 0, 1);
            OnValueChanged.Invoke(CurrentValue);
        }

        public void ModifyCurrentValue(float value)
        {
            if(value == 0)
                return;
            var percent = value / MaxValue;
            currentValuePercent += percent;
            currentValuePercent = Mathf.Clamp(currentValuePercent, 0, 1);
            OnValueChanged.Invoke(CurrentValue);
        }

        public void Reset()
        {
            CurrentValue = MaxValueStat.Value;
            OnValueChanged.Invoke(CurrentValue);
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
            return a.CurrentValue == b;
        }

        public static bool operator !=(Resource a, float b)
        {
            return a.CurrentValue != b;
        }

        public static bool operator >(Resource a, float b)
        {
            return a.CurrentValue > b;
        }

        public static bool operator <(Resource a, float b)
        {
            return a.CurrentValue < b;
        }

        public static bool operator >=(Resource a, float b)
        {
            return a.CurrentValue >= b;
        }

        public static bool operator <=(Resource a, float b)
        {
            return a.CurrentValue <= b;
        }

        public static float operator /(Resource a, float b)
        {
            a.SetCurrentValue(a.CurrentValue / b);
            return a.CurrentValue;
        }
    }
}