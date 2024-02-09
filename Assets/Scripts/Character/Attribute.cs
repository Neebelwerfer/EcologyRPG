using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeModification
{
    public object Source;
    public int Value;
}

public class AttributeData
{
    public string name;
    public string displayName;
    public string description;
    public int defaultValue = 10;

    public List<StatModification> statMods;
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


