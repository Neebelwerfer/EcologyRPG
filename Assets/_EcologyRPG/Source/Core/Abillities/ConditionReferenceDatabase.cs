using EcologyRPG.Core.Character;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.Core.Abilities
{
    public class ConditionReferenceDatabase : ScriptableObject
    {
        public static ConditionReferenceDatabase Instance;
        public const string DatabasePath = "Assets/_EcologyRPG/Resources/ConditionReferenceDatabase.asset";
        public const string ConditionsPath = "Assets/_EcologyRPG/Resources/Conditions/";
        public ConditionReferenceData[] conditions = new ConditionReferenceData[0];

        readonly Dictionary<int, ConditionPool> conditionDictionary = new Dictionary<int, ConditionPool>();

        public ConditionReference GetCondition(int ID)
        {
            foreach (var condition in conditions)
            {
                if (condition.ID == ID)
                {
                    if (!conditionDictionary.ContainsKey(ID))
                    {
                        conditionDictionary.Add(ID, new ConditionPool(condition));
                    }
                    return conditionDictionary[ID].Get();
                }
            }

            return null;
        }

        public static void Load()
        {
            Instance = Resources.Load<ConditionReferenceDatabase>("ConditionReferenceDatabase");
        }

        public static void ReturnCondition(ConditionReference condition)
        {
            Instance.conditionDictionary[condition.ID].Return(condition);
        }
    }


    public class ConditionPool
    {
        public ConditionReferenceData data;
        public Stack<ConditionReference> pool;

        public ConditionPool(ConditionReferenceData data)
        {
            this.data = data;
            pool = new Stack<ConditionReference>();
        }

        public ConditionReference Get()
        {
            if (pool.Count > 0)
            {
                return pool.Pop();
            }
            return new ConditionReference(data);
        }

        public void Return(ConditionReference condition)
        {
            pool.Push(condition);
        }
    }
}