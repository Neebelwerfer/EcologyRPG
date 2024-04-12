using EcologyRPG.Utility;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace EcologyRPG.Core
{
    public class Flags
    {
        public const string path = "Assets/_EcologyRPG/Resources/Flags.txt";
        const string resourcePath = "Flags";

        readonly Dictionary<string, int> flags;

        public UnityEvent<string> OnFlagChanged = new UnityEvent<string>();

        public Flags()
        {
            var json = Resources.Load<TextAsset>(resourcePath).text;
            var newList = JsonUtility.FromJson<SerializableDictionary<string, int>>(json);
            flags = newList.ToDictionary();
        }

        public void SetFlag(string flag, int value)
        {
            flags[flag] = value;
            OnFlagChanged.Invoke(flag);
        }

        public bool TryGetFlag(string flag, out int value)
        {
            return flags.TryGetValue(flag, out value);
        }
    }
}