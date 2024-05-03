using MoonSharp.Interpreter;
using System;

namespace EcologyRPG.Core.Abilities
{
    [System.Serializable]
    public enum GlobalVariableType
    {
        String,
        Int,
        AbilityID,
        ConditionID,
        Float,
        Bool,
        DamageType,
        Function,
    }

    [Serializable]
    public class GlobalVariable
    {
        public string Name;
        public GlobalVariableType Type;
        public string Value;

        public GlobalVariable(string name, GlobalVariableType type) : this(name, type, DefaultValue(type))
        {
        }

        internal GlobalVariable(string name, GlobalVariableType type, string value)
        {
            Name = name;
            Type = type;
            Value = value;
        }

        public static string DefaultValue(GlobalVariableType type)
        {
            return type switch
            {
                GlobalVariableType.String => "",
                GlobalVariableType.Int => "0",
                GlobalVariableType.AbilityID => "0",
                GlobalVariableType.ConditionID => "0",
                GlobalVariableType.Float => "0",
                GlobalVariableType.Bool => "false",
                GlobalVariableType.DamageType => "0",
                GlobalVariableType.Function => @"
function OnHit(target)
    target.ApplyDamage(10, DamageType)
end",
                _ => "",
            };
        }

        public virtual DynValue GetDynValue(Script context)
        {
            return Type switch
            {
                GlobalVariableType.String => DynValue.NewString(Value),
                GlobalVariableType.Int => DynValue.NewNumber(int.Parse(Value)),
                GlobalVariableType.AbilityID => DynValue.NewNumber(int.Parse(Value)),
                GlobalVariableType.ConditionID => DynValue.NewNumber(int.Parse(Value)),
                GlobalVariableType.Float => DynValue.NewNumber(float.Parse(Value)),
                GlobalVariableType.Bool => DynValue.NewBoolean(bool.Parse(Value)),
                GlobalVariableType.DamageType => DynValue.NewNumber((int)Enum.Parse(typeof(DamageType), Value)),
                GlobalVariableType.Function => context.DoString(Value),
                _ => DynValue.NewString(Value),
            };
        }

        public virtual GlobalVariable Clone()
        {
            return new GlobalVariable(Name, Type, Value);
        }
    }
}