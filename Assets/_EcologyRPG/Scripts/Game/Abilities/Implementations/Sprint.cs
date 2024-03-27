using EcologyRPG.Core.Abilities;
using EcologyRPG.Game.Abilities.Conditions;
using UnityEditor;

namespace EcologyRPG.Game.Abilities.Implementations
{
    public class Sprint : BaseAbility
    {
        public Exhaustion Exhaustion;
        public float sprintSpeedMultiplier = 1f;

        static SprintCondition sprintCondition;


        public override void Cast(CastInfo castInfo)
        {
            if (sprintCondition == null)
            {
                sprintCondition = CreateInstance<SprintCondition>();
                sprintCondition.Value = sprintSpeedMultiplier;
                sprintCondition.duration = 0.25f;
            }


            if (castInfo.owner.Stats.GetResource("Stamina") < 5)
            {
                castInfo.owner.ApplyCondition(castInfo, Instantiate(Exhaustion));
            }

            castInfo.owner.ApplyCondition(castInfo, Instantiate(sprintCondition));
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Sprint))]
    public class SprintEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            Sprint ability = (Sprint)target;
            if (ability.Exhaustion == null)
            {
                ability.Exhaustion = CreateInstance<Exhaustion>();
                ability.Exhaustion.name = "Exhaustion";
                AssetDatabase.AddObjectToAsset(ability.Exhaustion, ability);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            else
                ability.Exhaustion = (Exhaustion)EditorGUILayout.ObjectField("Exhaustion Effect", ability.Exhaustion, typeof(Exhaustion), false);
        }
    }
#endif
}