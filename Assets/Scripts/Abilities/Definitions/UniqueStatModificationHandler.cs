using Character;
using System.Collections.Generic;
using UnityEngine;

public class UniqueStatModificationHandler
{
    class Value
    {
        public CharacterEffect effect;
        public float value;
    }

    class UniqueStatModification
    {
        public List<Value> values;
        public StatModification statModification;

    }

    StatModType modType;
    string statName;

    Dictionary<BaseCharacter, UniqueStatModification> UniqueStatMap;

    public UniqueStatModificationHandler(string statName, StatModType modType)
    {
        this.modType = modType;
        this.statName = statName;
    }

    public void AddValue(BaseCharacter target, CharacterEffect effect, float value)
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
        RecalculateMax(target);
    }

    public void RemoveValue(BaseCharacter target, CharacterEffect effect)
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

            RecalculateMax(target);
        }

    }

    public void UpdateValue(BaseCharacter target, CharacterEffect effect, float value)
    {
        if (!UniqueStatMap.ContainsKey(target)) return;

        var uniqueStat = UniqueStatMap[target];
        var valueObj = uniqueStat.values.Find(x => x.effect == effect);
        if (valueObj != null)
        {
            valueObj.value = value;
            RecalculateMax(target);
        }
    }

    void RecalculateMax(BaseCharacter target)
    {
        var uniqueStat = UniqueStatMap[target];
        var max = 0f;
        foreach (var value in uniqueStat.values)
        {
            max = Mathf.Max(max, value.value);
        }
        uniqueStat.statModification.Value = max;
    }
}