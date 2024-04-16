using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EcologyRPG.Core.Items
{
    [System.Serializable]
    public struct ItemReference
    {
        public string GUID;
        [SerializeField] string name;

        public ItemReference(string GUID, string name)
        {
            this.GUID = GUID;
            this.name = name;
        }

        public readonly Item Get()
        {
            return ItemDatabase.Instance.GetItemByGUID(GUID);
        }

        public readonly string GetName()
        {
            return name;
        }
    }

    public class ItemDatabase : ScriptableObject
    {
        public static ItemDatabase Instance { get; private set; }
        public const string ItemDatabasePath = "Assets/_EcologyRPG/Resources/Config/ItemDatabase.asset";
        public const string ItemDatabaseResourcePath = "Config/ItemDatabase";
        [SerializeField] Item[] items = new Item[0];

        public Item GetItemByGUID(string GUID)
        {
            foreach (var item in items)
            {
                if (item.GUID == GUID)
                {
                    return item;
                }
            }
            return null;
        }

        public Item GetItemByName(string name)
        {
            foreach (var item in items)
            {
                if (item.Name == name)
                {
                    return item;
                }
            }
            return null;
        }

        public Item[] GetItems() => items;

        void Init()
        {
            foreach (var item in items)
            {
                item.Initialize();
            }
            Debug.Log($"Item Database Initialized: {items.Length} items found");
        }

        public List<Item> GetItemsWithGenerationRules()
        {
            List<Item> itemsWithRules = new List<Item>();
            foreach (var item in items)
            {
                if (item.CanGenerate)
                {
                    itemsWithRules.Add(item);
                }
            }
            return itemsWithRules;
        }

        public static ItemDatabase Load()
        {
            var itemDatabase = Resources.Load<ItemDatabase>(ItemDatabaseResourcePath);
            itemDatabase.Init();
            Instance = itemDatabase;
            return itemDatabase;
        }
    }

#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(ItemReference))]
    public class ItemReferenceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var guid = property.FindPropertyRelative("GUID");
            var name = property.FindPropertyRelative("name");
            if(guid.stringValue == "")
            {
                GUI.Label(new Rect(position.x, position.y, position.width * 0.5f, 20), label, EditorStyles.boldLabel);
                if (GUI.Button(new Rect(position.x + position.width * 0.5f, position.y, position.width * 0.5f, 20), "Select Item"))
                {
                    var selector = ItemSelectorEditor.OpenWindow();
                    selector.SetProperty(property);
                }
            }
            else
            {
                GUI.Label(new Rect(position.x, position.y, position.width * 0.5f, 20), name.stringValue, EditorStyles.boldLabel);
                if (GUI.Button(new Rect(position.x + position.width * 0.5f, position.y, position.width * 0.5f, 20), "Change Item"))
                {
                    var selector = ItemSelectorEditor.OpenWindow();
                    selector.SetProperty(property);
                }
            }

        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 20;
        }
    }


    public class ItemSelectorEditor : EditorWindow
    {
        public static ItemSelectorEditor OpenWindow()
        {
            return GetWindow<ItemSelectorEditor>();
        }

        Item.ItemTypes itemSearchType = Item.ItemTypes.Item;
        bool EnableSearchFilter = false;
        string nameSearch = "";

        SerializedProperty serializedField;
        ItemDatabase itemDatabase;
        Item[] items;

        public void SetProperty(SerializedProperty field)
        {
            serializedField = field;
        }

        private void OnEnable()
        {
            itemDatabase = AssetDatabase.LoadAssetAtPath<ItemDatabase>(ItemDatabase.ItemDatabasePath);
            items = itemDatabase.GetItems();
        }


        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            itemSearchType = (Item.ItemTypes)EditorGUILayout.EnumPopup("Item Type", itemSearchType);
            EnableSearchFilter = GUILayout.Toggle(EnableSearchFilter, "Enable search Filter");
            GUILayout.EndHorizontal();
            nameSearch = EditorGUILayout.TextField("Search", nameSearch);

            for (int i = 0; i < items.Length; i++)
            {
                if (!string.IsNullOrEmpty(nameSearch) && !items[i].Name.ToLower().Contains(nameSearch.ToLower()))
                {
                    continue;
                }

                if (EnableSearchFilter && items[i].ItemType != itemSearchType)
                {
                    continue;
                }

                if (GUILayout.Button(items[i].Name))
                {
                    serializedField.FindPropertyRelative("GUID").stringValue = items[i].GUID;
                    serializedField.FindPropertyRelative("name").stringValue = items[i].Name;
                    serializedField.serializedObject.ApplyModifiedProperties();
                    Close();
                }
            }
        }


    }

    public class ItemDatabaseEditor : EditorWindow
    {
        bool[] showItem = new bool[0];
        Vector2 scrollPos;
        Item.ItemTypes newItemType;
        Item.ItemTypes searchType;
        bool searchTypeSet = false;
        string search = "";
        SerializedObject serializedObject;
        LootDatabase lootDatabase;
        bool showLootDatabase = false;

        [MenuItem("Game/Item Database")]
        public static void OpenWindow()
        {
            GetWindow<ItemDatabaseEditor>();
        }

        private void OnEnable()
        {
            titleContent = new GUIContent("Item Database");
            lootDatabase = AssetDatabase.LoadAssetAtPath<LootDatabase>(LootDatabase.Path);
            var itemDatabase = AssetDatabase.LoadAssetAtPath<ItemDatabase>(ItemDatabase.ItemDatabasePath);
            if(itemDatabase == null)
            {
                itemDatabase = CreateInstance<ItemDatabase>();
                AssetDatabase.CreateAsset(itemDatabase, ItemDatabase.ItemDatabasePath);
                AssetDatabase.SaveAssets();
            }
            serializedObject = new SerializedObject(itemDatabase);
        }

        public void OnGUI()
        {
            var items = serializedObject.FindProperty("items");
            var itemCount = items.arraySize;
            int deleteIndex = -1;

            if(itemCount == 0)
            {
                EditorGUILayout.HelpBox("No items in database", MessageType.Info);
            }

            if (showItem.Length != itemCount)
            {
                showItem = new bool[itemCount];
            }

            EditorGUILayout.Space(10);
            GUILayout.BeginHorizontal();
            searchType = (Item.ItemTypes)EditorGUILayout.EnumPopup("Search Type", searchType);
            searchTypeSet = GUILayout.Toggle(searchTypeSet, "Enable search Filter");
            GUILayout.EndHorizontal();
            EditorGUILayout.Space(10);
            GUILayout.BeginHorizontal();
            newItemType = (Item.ItemTypes)EditorGUILayout.EnumPopup("Item Type", newItemType);
            if (GUILayout.Button("Add Item"))
            {
                var newItem = CreateItem(newItemType);
                items.InsertArrayElementAtIndex(itemCount);
                newItem.GUID = System.Guid.NewGuid().ToString();
                items.GetArrayElementAtIndex(itemCount).objectReferenceValue = newItem;
            }
            GUILayout.EndHorizontal();
            search = EditorGUILayout.TextField("Search", search);
            EditorGUILayout.Space(10);

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            for (int i = 0; i < itemCount; i++)
            {
                var item = items.GetArrayElementAtIndex(i);
                Item itemObject = item.objectReferenceValue as Item;

                if (!string.IsNullOrEmpty(search) && !itemObject.Name.ToLower().Contains(search.ToLower()))
                {
                    continue;
                }
                if(searchTypeSet && itemObject.ItemType != searchType)
                {
                    continue;
                }

                if(itemObject == null)
                {
                    items.DeleteArrayElementAtIndex(i);
                    continue;
                }

                EditorGUILayout.BeginHorizontal();
                showItem[i] = EditorGUILayout.Foldout(showItem[i], $"{itemObject.ItemType}/{itemObject.Name}");
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    deleteIndex = i;
                }
                EditorGUILayout.EndHorizontal();
                if (showItem[i])
                {
                    EditorGUI.indentLevel++;
                    var editor = Editor.CreateEditor(item.objectReferenceValue);
                    editor.OnInspectorGUI();
                    EditorGUI.indentLevel--;
                }
            }
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            showLootDatabase = EditorGUILayout.Foldout(showLootDatabase, "Loot Database");
            if(showLootDatabase)
            {
                EditorGUILayout.Space(10);
                EditorGUILayout.LabelField("Loot Database settings", EditorStyles.boldLabel);
                var editorLootDatabase = Editor.CreateEditor(lootDatabase);
                editorLootDatabase.OnInspectorGUI();
            }
            EditorGUILayout.EndScrollView();
            if (deleteIndex != -1)
            {
                var item = items.GetArrayElementAtIndex(deleteIndex).objectReferenceValue as Item;
                items.DeleteArrayElementAtIndex(deleteIndex);
                AssetDatabase.DeleteAsset(Item.ItemPath + "/" + item.ItemType + "/" + item.name + ".asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            
            serializedObject.ApplyModifiedProperties();
        }

        Item CreateItem(Item.ItemTypes type)
        {
            Item item;
            switch (type)
            {
                case Item.ItemTypes.ReplenishingItem:
                    item = CreateInstance<ReplenishingItem>();
                    break;
                case Item.ItemTypes.Weapon:
                    item = CreateInstance<Weapon>();
                    break;
                case Item.ItemTypes.Armor:
                    item = CreateInstance<Armour>();
                    break;
                case Item.ItemTypes.Mask:
                    item = CreateInstance<Mask>();
                    break;
                case Item.ItemTypes.WaterTank:
                    item = CreateInstance<WaterTank>();
                    break;
                default:
                    item = CreateInstance<Item>();
                    break;
            }
            var typeString = type.ToString();
            var path = Item.ItemPath + "/" + typeString + "/" ;
            if(!AssetDatabase.IsValidFolder(path))
            {
                AssetDatabase.CreateFolder(Item.ItemPath, typeString);
            }
            AssetDatabase.CreateAsset(item, AssetDatabase.GenerateUniqueAssetPath(path + typeString + ".asset"));
            AssetDatabase.SaveAssets();
            item.ItemType = type;
            return item;
        }
    }
 #endif
}