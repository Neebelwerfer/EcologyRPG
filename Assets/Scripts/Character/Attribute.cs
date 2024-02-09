using Character;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AttributeModification
{
    public object Source;
    public int Value;
}

[Serializable]
public class AttributeProgression
{
    [Tooltip("The name of the stat to modify")]
    public string statName;
    [Tooltip("The type of modification to apply")]
    public StatModType modType;
    [Tooltip("The applied stats at 0 points in this attribute")]
    public float startValue;
    [Tooltip("How much the value changes from each point in this attribute")]
    public float changePerPoint;
}

[Serializable]
public class AttributeData
{
    [Tooltip("The name of the attribute")]
    public string name;
    [Tooltip("The display name of the attribute")]
    public string displayName;
    [TextArea(3, 10)]
    [Tooltip("A description of what this attribute does")]
    public string description;
    [Tooltip("The default value of this attribute")]
    public int defaultValue = 5;

    [Tooltip("The progression of stats this attribute modifies")]
    public List<AttributeProgression> statProgressions;
}

public class Attribute
{
    readonly AttributeData data;

    public List<AttributeModification> modifiers;

    public int currentValue;

    public bool isDirty = true;

    public Attribute(AttributeData data)
    {
        this.data = data;
        modifiers = new List<AttributeModification>();
    }

    public int Value
    {
        get
        {
            if(isDirty)
            {
                CalculateValue();
                isDirty = false;
            };
            return currentValue;
        }
    }

    public void AddModifier(AttributeModification modifier)
    {
        modifiers.Add(modifier);
        isDirty = true;
    }

    public void RemoveModifier(AttributeModification modifier)
    {
        modifiers.Remove(modifier);
        isDirty = true;
    }

    public void RemoveAllModifiersFromSource(object source)
    {
        modifiers.RemoveAll(x => x.Source == source);
        isDirty = true;
    }

    void CalculateValue()
    {
        var v = data.defaultValue;
        foreach (var item in modifiers)
        {
            v += item.Value;
        }
        currentValue = v;
        UpdateStatModifiers();
    }

    public void UpdateStatModifiers()
    {

    }

}


