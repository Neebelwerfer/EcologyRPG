using EcologyRPG.AbilityScripting;
using EcologyRPG.Core.Abilities;
using System.IO;
using UnityEditor;
using UnityEngine;



public class AbilityEditor : EditorWindow
{
    [MenuItem("Game/Ability Editor")]
    public static void ShowWindow()
    {
        GetWindow<AbilityEditor>("Ability Editor");
    }

    public static SerializedDataArray abilityData;
    static uint idCounter = 0;
    static bool[] foldouts;
    Vector2 scrollPos;
    AbilityCategory selectedCategory;

    AbilityCategory searchCategory;
    bool enableCategorySearch;
    string searchName = "";

    private void OnEnable()
    {
        Load();
    }

    public static void Load()
    {
        var jsonObj = AssetDatabase.LoadAssetAtPath<TextAsset>(AbilityManager.AbilityDataPath);
        if (jsonObj != null)
        {
            abilityData = JsonUtility.FromJson<SerializedDataArray>(jsonObj.text);
            abilityData ??= new SerializedDataArray();
        }
        else
        {
            if (!AssetDatabase.IsValidFolder(AbilityManager.AbilityFullpath))
            {
                AssetDatabase.CreateFolder("Assets/_EcologyRPG/Resources", "Abilities");
            }
                
            File.WriteAllText(AbilityManager.AbilityDataPath, "[]");
            abilityData = new SerializedDataArray();
        }
        idCounter = abilityData.IDCounter;
        foldouts = new bool[abilityData.data.Length];
    }

    public void Save()
    {
        abilityData.IDCounter = idCounter;
        var json = JsonUtility.ToJson(abilityData);
        StreamWriter writer = new StreamWriter(AbilityManager.AbilityDataPath);
        writer.Write(json);
        writer.Close();
        AssetDatabase.Refresh();
    }

    public override void SaveChanges()
    {
        base.SaveChanges();
        Save();
    }

    public void CreateScript(int i)
    {
        var path = AbilityManager.AbilityFullpath + "Ability_" + abilityData[i].ID + AbilityManager.AbilityScriptExtension;
        File.WriteAllText(path, GenerateScript(abilityData[i].Category));
        abilityData[i].ScriptPath = path;
        Save();
    }

    private void OnGUI()
    {
        int DeleteIndex = -1;
        GUILayout.Label("Ability Editor", EditorStyles.boldLabel);

        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        searchCategory = (AbilityCategory)EditorGUILayout.EnumPopup("Category", searchCategory);
        enableCategorySearch = GUILayout.Toggle(enableCategorySearch, "Enable Category Search");
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        searchName = EditorGUILayout.TextField("Search ", searchName).ToLower();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        GUILayout.BeginHorizontal();
        selectedCategory = (AbilityCategory)EditorGUILayout.EnumPopup("Category", selectedCategory);
        if (GUILayout.Button("Create Ability"))
        {
            idCounter += 1;
            var newAbility = new AbilityData("New Ability", selectedCategory, idCounter);
            var newAbilityData = new AbilityData[abilityData.data.Length + 1];
            for (int i = 0; i < abilityData.data.Length; i++)
            {
                newAbilityData[i] = abilityData[i];
            }
            newAbilityData[abilityData.data.Length] = newAbility;
            foldouts = new bool[newAbilityData.Length];
            abilityData.data = newAbilityData;
            Save();
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(10);
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        for (int i = 0; i < abilityData.data.Length; i++)
        {
            if (enableCategorySearch && searchCategory != abilityData[i].Category) continue;
            if (!string.IsNullOrEmpty(searchName) && !abilityData[i].abilityName.ToLower().Contains(searchName)) continue;
            GUILayout.BeginHorizontal();
            foldouts[i] = EditorGUILayout.Foldout(foldouts[i], abilityData[i].abilityName);
            if(GUILayout.Button("Edit"))
            {
                SingleAbilityEditor.ShowWindow(i, this);
            }
            GUILayout.EndHorizontal();
            if (foldouts[i])
            {
                GUILayout.Label("ID: " + abilityData[i].ID);

                GUILayout.BeginHorizontal();
                GUILayout.Label("Name");
                abilityData[i].abilityName = GUILayout.TextField(abilityData[i].abilityName);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("Category");
                abilityData[i].Category = (AbilityCategory)EditorGUILayout.EnumPopup(abilityData[i].Category);
                GUILayout.EndHorizontal();

                if (abilityData[i]._DefaultGlobalVariables == null)
                {
                    abilityData[i]._DefaultGlobalVariables = new GlobalVariable[0];
                }

                foreach (var variable in abilityData[i]._DefaultGlobalVariables)
                {
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(variable.Name);
                    GUILayout.Label(variable.Value);
                    GUILayout.EndHorizontal();
                }

                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                if (GUILayout.Button("Delete Ability"))
                {
                    DeleteIndex = i;

                }
            }
                
        }
        if (GUILayout.Button("Save"))
        {
            Save();
        }
        GUILayout.EndScrollView();

        if(DeleteIndex > -1)
        {
            var ability = abilityData[DeleteIndex];
            var newAbilityData = new AbilityData[abilityData.data.Length - 1];
            for (int i = 0; i < abilityData.data.Length; i++)
            {
                if (i < DeleteIndex)
                {
                    newAbilityData[i] = abilityData[i];
                }
                else if (i > DeleteIndex)
                {
                    newAbilityData[i - 1] = abilityData[i];
                }
            }
            abilityData.data = newAbilityData;
            if(!string.IsNullOrEmpty(ability.ScriptPath)) File.Delete(ability.ScriptPath);
            Save();
        }
    }

    string PlayerCost()
    {
        return @"
function PayCost()
    local resource = Context.GetOwner().GetResource(""stamina"")
    resource:Consume(10)
    end
";
    }

    string NotSubAbility()
    {
        return @"
function CanActivate()
    local resource = Context.GetOwner().GetResource(""""stamina"""")
    return resource:HaveEnough(10)
end

function OnCancelledCast()
    Log(""Ability Cast Cancelled"")
end";
    }

    string GenerateScript(AbilityCategory abilityCategory)
    {
        return (abilityCategory > AbilityCategory.SubAbility ? NotSubAbility() : "") 
            + (abilityCategory == AbilityCategory.Player ? PlayerCost() : "") +
@"
function OnCast()
    Log(""Casting Ability"")
    Delay(1)
    Log(""Ability Casted"")
end
";
    }
}
