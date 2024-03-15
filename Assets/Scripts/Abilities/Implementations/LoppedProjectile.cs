using Character.Abilities;
using Codice.Client.Commands;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/LoppedProjectile", fileName = "New Lopped Projectile")]
public class LoppedProjectile : AttackAbilityEffect
{
    [Header("Lopped Projectile")]
    [Tooltip("The prefab of the projectile")]
    public GameObject ProjectilePrefab;
    [Tooltip("The layer mask of the colliders the projectile can travel through")]
    public LayerMask ignoreMask;
    [Tooltip("The angle of the projectile")]
    public float Angle;
    [Tooltip("The travel time of the projectile")]
    public float TravelTime;
    [Tooltip("The ability that will be cast when the projectile hits the ground")]
    public AttackAbility OnHitAbility;

    public override void Cast(CastInfo caster)
    {
        Debug.DrawRay(caster.mousePoint, Vector3.up * 10, Color.red, 5);
        ProjectileUtility.CreateCurvedProjectile(ProjectilePrefab, caster.mousePoint, TravelTime, -Angle, ignoreMask, caster.owner, (projectileObject) =>
        {
            caster.owner.StartCoroutine(OnHitAbility.HandleCast(new CastInfo { owner = caster.owner, castPos = projectileObject.transform.position, mousePoint = caster.mousePoint }));
        });
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(LoppedProjectile))]
public class LoppedProjectileEditor : AttackAbilityEffectEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LoppedProjectile ability = (LoppedProjectile)target;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("ProjectilePrefab"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("ignoreMask"));
        ability.Angle = EditorGUILayout.FloatField("Angle", ability.Angle);
        ability.TravelTime = EditorGUILayout.FloatField("Travel Time", ability.TravelTime);

        if(ability.OnHitAbility == null)
        {
            if (GUILayout.Button("Add On Hit Ability"))
            {
                ability.OnHitAbility = CreateInstance<AttackAbility>();
                ability.OnHitAbility.name = "On Hit Ability";
                AssetDatabase.AddObjectToAsset(ability.OnHitAbility, ability);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.SetDirty(ability);
            }
        } else
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("OnHitAbility"));
            if(GUILayout.Button("Remove On Hit Ability"))
            {
                DestroyImmediate(ability.OnHitAbility, true);
                ability.OnHitAbility = null;
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.SetDirty(ability);
            }
        }
       
    }
}
#endif