using MoonSharp.Interpreter;
using System;
using UnityEditor;

namespace EcologyRPG.Core.Abilities
{
    [System.Serializable]
    public enum GlobalVariableType
    {
        String,
        Int,
        Float,
        Bool,
        DamageType,
    }

    [Serializable]
    public class GlobalVariable
    {
        public string Name;

        public GlobalVariable(string name)
        {
            Name = name;
        }

        public virtual DynValue GetDynValue()
        {
            return DynValue.NewNil();
        }

        public virtual void DrawEditableValue()
        {

        }

        public virtual void DrawValueLabel()
        {

        }

        public virtual GlobalVariable Clone()
        {
            return null;
        }
    }

    [Serializable]
    public class StringGlobalVariable : GlobalVariable
    {
        public string Value;

        public StringGlobalVariable(string name, string value) : base(name)
        {
            Value = value;
        }

        public override void DrawEditableValue()
        {
            Value = EditorGUILayout.TextField(Value);
        }

        public override void DrawValueLabel()
        {
            EditorGUILayout.LabelField(Value);
        }

        public override DynValue GetDynValue()
        {
            return DynValue.NewString(Value);
        }

        public override GlobalVariable Clone()
        {
            return new StringGlobalVariable(Name, Value);
        }
    }

    [Serializable]
    public class IntGlobalVariable : GlobalVariable
    {
        public int Value;
        public IntGlobalVariable(string name, int value) : base(name)
        {
            Value = value;
        }

        public override void DrawEditableValue()
        {
             Value = EditorGUILayout.IntField(Value);
        }

        public override DynValue GetDynValue()
        {
            return DynValue.NewNumber(Value);
        }

        public override void DrawValueLabel()
        {
            EditorGUILayout.LabelField(Value.ToString());
        }

        public override GlobalVariable Clone()
        {
            return new IntGlobalVariable(Name, Value);
        }
    }

    [Serializable]
    public class FloatGlobalVariable : GlobalVariable
    {
        public float Value;
        public FloatGlobalVariable(string name, float value) : base(name)
        {
            Value = value;
        }

        public override void DrawEditableValue()
        {
            Value = EditorGUILayout.FloatField(Value);
        }

        public override DynValue GetDynValue()
        {
            return DynValue.NewNumber(Value);
        }

        public override void DrawValueLabel()
        {
            EditorGUILayout.LabelField(Value.ToString());
        }

        public override GlobalVariable Clone()
        {
            return new FloatGlobalVariable(Name, Value);
        }
    }

    [Serializable]
    public class BoolGlobalVariable : GlobalVariable
    {
        public bool Value;
        public BoolGlobalVariable(string name, bool value) : base(name)
        {
            Value = value;
        }

        public override void DrawEditableValue()
        {
            Value = EditorGUILayout.Toggle(Value);
        }

        public override DynValue GetDynValue()
        {
            return DynValue.NewBoolean(Value);
        }

        public override void DrawValueLabel()
        {
            EditorGUILayout.LabelField(Value.ToString());
        }

        public override GlobalVariable Clone()
        {
            return new BoolGlobalVariable(Name, Value);
        }
    }

    [Serializable]
    public class DamageTypeGlobalVariable : GlobalVariable
    {
        public DamageType Value;
        public DamageTypeGlobalVariable(string name, DamageType value) : base(name)
        {
            Value = value;
        }

        public override void DrawEditableValue()
        {
            Value = (DamageType)EditorGUILayout.EnumPopup(Value);
        }

        public override DynValue GetDynValue()
        {
            return DynValue.NewNumber((int)Value);
        }

        public override void DrawValueLabel()
        {
            EditorGUILayout.LabelField(Value.ToString());
        }

        public override GlobalVariable Clone()
        {
            return new DamageTypeGlobalVariable(Name, Value);
        }
    }
}