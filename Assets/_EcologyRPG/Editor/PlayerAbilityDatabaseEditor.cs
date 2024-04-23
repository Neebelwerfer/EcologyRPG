using EcologyRPG.Core.Abilities;
using EcologyRPG.GameSystems.PlayerSystems;
using UnityEditor;
using UnityEngine;

public class PlayerAbilityDatabaseEditor : EditorWindow
{
    const string path = "Assets/_EcologyRPG/Resources/PlayerAbilities.asset";

    [MenuItem("Game/Character/Player Ability Database")]
    public static void ShowWindow()
    {
        GetWindow<PlayerAbilityDatabaseEditor>("Player Ability Database");
    }

    PlayerAbilitiesDatabase PlayerAbilities;

    private void OnEnable()
    {
        PlayerAbilities = AssetDatabase.LoadAssetAtPath<PlayerAbilitiesDatabase>(path);
    }

    private void OnGUI()
    {
        GUILayout.Label("Player Abilities", EditorStyles.boldLabel);
        var list = PlayerAbilities.GetList();

        if(list != null && list.Count != 0)
        {
            foreach (var ability in list)
            {
                GUILayout.BeginHorizontal();
                ability.ability = (PlayerAbilityReference)EditorGUILayout.ObjectField(ability.ability, typeof(PlayerAbilityReference), false);
                if (GUILayout.Button("Remove"))
                {
                    list.Remove(ability);
                    break;
                }
                GUILayout.EndHorizontal();
                ability.LevelRequirement = (uint)EditorGUILayout.IntField("Level Requirement", (int)ability.LevelRequirement);
            }
        }

        if (GUILayout.Button("Add Ability"))
        {
            PlayerAbilities.AddAbility(null, 0);
        }

        EditorUtility.SetDirty(PlayerAbilities);  
    }
}
