using MoonSharp.Interpreter;
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EcologyRPG.Core.Abilities
{
    [System.Serializable]
    public enum GlobalVariableType
    {
        String,
        Int,
        AbilityID,
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

#if UNITY_EDITOR
        public virtual void DrawEditableValue()
        {

        }

        public virtual void DrawValueLabel()
        {

        }
#endif

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

#if UNITY_EDITOR
        public override void DrawEditableValue()
        {
            Value = EditorGUILayout.TextField(Value);
        }

        public override void DrawValueLabel()
        {
            EditorGUILayout.LabelField(Value);
        }
#endif

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
        public override DynValue GetDynValue()
        {
            return DynValue.NewNumber(Value);
        }

#if UNITY_EDITOR
        public override void DrawEditableValue()
        {
             Value = EditorGUILayout.IntField(Value);
        }

        public override void DrawValueLabel()
        {
            EditorGUILayout.LabelField(Value.ToString());
        }
#endif

        public override GlobalVariable Clone()
        {
            return new IntGlobalVariable(Name, Value);
        }
    }

    public class AbilityIDGlobalVariable : IntGlobalVariable
    {
        public AbilityIDGlobalVariable(string name, int value) : base(name, value)
        {
        }

        public override void DrawEditableValue()
        {
            EditorGUILayout.BeginHorizontal();

            if(AbilityEditor.abilityData == null)
            {
                AbilityEditor.Load();
            }
            var data = AbilityEditor.abilityData.data;
            if(data == null)
            {
                EditorGUILayout.LabelField("No abilities found");
            }
            else
            {
                var ability = Array.Find(data, x => x.ID == Value);
                if (ability == null)
                {
                    EditorGUILayout.LabelField("Ability not found");
                }
                else EditorGUILayout.LabelField(ability.abilityName);

                if (GUILayout.Button("Select"))
                {
                    var field = GetType().GetField("Value");
                    AbilitySelectorEditor.Open(field, this);
                }
            }
            EditorGUILayout.EndHorizontal();
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
        public override DynValue GetDynValue()
        {
            return DynValue.NewNumber(Value);
        }

#if UNITY_EDITOR
        public override void DrawEditableValue()
        {
            Value = EditorGUILayout.FloatField(Value);
        }

        public override void DrawValueLabel()
        {
            EditorGUILayout.LabelField(Value.ToString());
        }
#endif

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
        public override DynValue GetDynValue()
        {
            return DynValue.NewBoolean(Value);
        }

#if UNITY_EDITOR
        public override void DrawEditableValue()
        {
            Value = EditorGUILayout.Toggle(Value);
        }

        public override void DrawValueLabel()
        {
            EditorGUILayout.LabelField(Value.ToString());
        }
#endif

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
        public override DynValue GetDynValue()
        {
            return DynValue.NewNumber((int)Value);
        }

#if UNITY_EDITOR
        public override void DrawEditableValue()
        {
            Value = (DamageType)EditorGUILayout.EnumPopup(Value);
        }

        public override void DrawValueLabel()
        {
            EditorGUILayout.LabelField(Value.ToString());
        }
#endif

        public override GlobalVariable Clone()
        {
            return new DamageTypeGlobalVariable(Name, Value);
        }
    }
}