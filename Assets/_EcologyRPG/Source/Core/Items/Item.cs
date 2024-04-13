using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.Core.Items
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]
    public class Item : ScriptableObject, IEquatable<Item>
    {
        [Header("Item Properties")]
        public string Name;
        public string Description;
        public Sprite Icon;
        public float Weight;

        protected string DisplayString;

        public virtual bool Equals(Item other)
        {
            return Name == other.Name;
        }

        public virtual string GetDisplayString()
        {
            return "Decsription: " + Description + "\n" + "Weight: " + Weight + "\n";
        }
    }
}