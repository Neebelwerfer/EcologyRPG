using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attribute
{
    public string Name;
    public int defaultValue = 10;

    public Dictionary<string, int> modifiers;

    public int currentValue;

    public bool isDirty = true;

    public int Value
    {
        get
        {
            CalculateValue();
            return currentValue;
        }
    }

    public void AddModifier(string name, int value)
    {
        modifiers.Add(name, value);
    }

    public void RemoveModifier(string name)
    {
        modifiers.Remove(name);
    }

    void CalculateValue()
    {
        var v = defaultValue;
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


