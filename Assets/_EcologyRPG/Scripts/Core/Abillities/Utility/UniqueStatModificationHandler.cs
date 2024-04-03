using EcologyRPG._Core.Character;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG._Core.Abilities
{
    public class UniqueStatModificationHandler
    {
        class Value
        {
            public Condition effect;
            public float value;
        }

        class UniqueStatModification
        {
            public List<Value> values;
            public StatModification statModification;

        }

        StatModType modType;
        string statName;
        bool shouldFindMax;

        Dictionary<BaseCharacter, UniqueStatModification> UniqueStatMap;

        public UniqueStatModificationHandler(string statName, StatModType modType, bool shouldFindMax)
        {
            this.modType = modType;
            this.statName = statName;
            this.shouldFindMax = shouldFindMax;
        }

        public void AddValue(BaseCharacter target, Condition effect, float value)
        {
            UniqueStatMap ??= new Dictionary<BaseCharacter, UniqueStatModification>();

            if (!UniqueStatMap.ContainsKey(target))
            {
                UniqueStatMap.Add(target, new UniqueStatModification()
                {
                    values = new List<Value>(),
                    statModification = new StatModification(statName, 0, modType, effect)
                });
                target.Stats.AddStatModifier(UniqueStatMap[target].statModification);

            }

            UniqueStatMap[target].values.Add(new Value()
            {
                effect = effect,
                value = value
            });
            Recalculate(target);
        }

        public void RemoveValue(BaseCharacter target, Condition effect)
        {
            if (!UniqueStatMap.ContainsKey(target)) return;

            var uniqueStat = UniqueStatMap[target];
            var value = uniqueStat.values.Find(x => x.effect == effect);
            if (value != null)
            {
                uniqueStat.values.Remove(value);
                if (uniqueStat.values.Count == 0)
                {
                    target.Stats.RemoveStatModifier(uniqueStat.statModification);
                    return;
                }

                Recalculate(target);
            }

        }

        public void UpdateValue(BaseCharacter target, Condition effect, float value)
        {
            if (!UniqueStatMap.ContainsKey(target)) return;

            var uniqueStat = UniqueStatMap[target];
            var valueObj = uniqueStat.values.Find(x => x.effect == effect);
            if (valueObj != null)
            {
                valueObj.value = value;
                Recalculate(target);
            }
        }

        void Recalculate(BaseCharacter target)
        {
            var uniqueStat = UniqueStatMap[target];
            var newValue = 0f;
            foreach (var value in uniqueStat.values)
            {
                if (shouldFindMax)
                    newValue = Mathf.Max(newValue, value.value);
                else
                    newValue = Mathf.Min(newValue, value.value);
            }
            uniqueStat.statModification.Value = newValue;
        }
    }
}