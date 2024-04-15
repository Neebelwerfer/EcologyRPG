using EcologyRPG.Core.Character;
using EcologyRPG.GameSystems.NPC;
using EcologyRPG.Utility;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EcologyRPG.GameSystems.NPC
{
    [Serializable]
    struct StatMod
    {
        [StatAttribute(StatType.Stat)]
        public string StatName;
        public float Value;
        public StatModType ModType;

        public StatMod(string statName, float value, StatModType modType)
        {
            StatName = statName;
            Value = value;
            ModType = modType;
        }
    }
    [CreateAssetMenu(fileName = "NPCConfig", menuName = "NPC/Config")]
    public class NPCConfig : ScriptableObject
    {
        [CharacterTag]
        public List<string> tags;
        public NPCBehaviour NPCBehaviour;
        public uint Level;
        public uint xp;

        [SerializeField] StatMod[] statModifications;

        public void ApplyModifications(BaseCharacter npc)
        {
            foreach (var statMod in statModifications)
            {
                var mod = new StatModification(statMod.StatName, statMod.Value, statMod.ModType, this);
                npc.Stats.AddStatModifier(mod);
            }
        }

#if UNITY_EDITOR
        public void GenerateStatModifications()
        {
            var json = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/_EcologyRPG/Resources/CharacterStats.txt").text;
            var serializableStats = JsonUtility.FromJson<SerializableStats>(json);
            var statData = serializableStats.Stats;

            statModifications = new StatMod[statData.Count];

            for (int i = 0; i < statData.Count; i++)
            {
                statModifications[i] = new StatMod(statData[i].name, 0, StatModType.Flat);
            }
        }
#endif
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(NPCConfig))]
public class NPCConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var npcConfig = (NPCConfig)target;
        if(GUILayout.Button("Generate Stat Modifications"))
        {
            npcConfig.GenerateStatModifications();
        }   
    }
}
#endif