using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeModifier
{
    public object Source;
    public int Value;
}

public class Attribute
{
    public string Name;
    public string DisplayName;
    public string Description;
    public int DefaultValue = 10;

    public List<AttributeModifier> modifiers;

    public int currentValue;

    public bool isDirty = true;

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

    public void AddModifier(AttributeModifier modifier)
    {
        modifiers.Add(modifier);
        isDirty = true;
    }

    public void RemoveModifier(AttributeModifier modifier)
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
        var v = DefaultValue;
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


