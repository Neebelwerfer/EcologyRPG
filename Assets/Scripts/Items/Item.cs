using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]
public class Item : ScriptableObject
{
    [Header("Item Properties")]
    public string Name;
    public string Description;
    public Sprite Icon;
    public float Weight;
}
