using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;
using UnityEditor;
using UnityEngine;

namespace EcologyRPG.GameSystems.Abilities.Conditions
{

    public class KnockCondition : DebuffCondition, IUpdateCondition
    {
        public enum KnockType
        {
            Away,
            Towards
        }

        public float KnockBackDistance = 4;
        public KnockType knockType;

        Vector3 dir;
        float KnockbackSpeed;

        public override void OnApply(CastInfo caster, BaseCharacter target)
        {
            if (knockType == KnockType.Away)
                dir = (target.Transform.Position - caster.castPos).normalized;
            else
                dir = (caster.castPos - target.Transform.Position).normalized;

            target.state = CharacterStates.disabled;
            target.Rigidbody.isKinematic = false;
            KnockbackSpeed = KnockBackDistance / duration;
        }
        public override void OnReapply(BaseCharacter target)
        {

        }

        public override void OnRemoved(BaseCharacter target)
        {
            target.state = CharacterStates.active;
            target.Rigidbody.velocity = Vector3.zero;
        }

        public override void OnUpdate(BaseCharacter target, float deltaTime)
        {
            if (BaseCharacter.IsLegalMove(target, dir, KnockbackSpeed * deltaTime))
            {
                target.Rigidbody.velocity = dir * KnockbackSpeed;
            } 
            else target.Rigidbody.velocity = Vector3.zero;
        }
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(KnockCondition))]
    public class KnockConditionEditor : ConditionEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("KnockBackDistance"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("knockType"));
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}