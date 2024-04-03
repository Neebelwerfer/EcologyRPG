using EcologyRPG.Utility;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace EcologyRPG._Core
{
    public class Flags
    {
        public const string path = "Assets/_EcologyRPG/Resources/Flags.txt";

        readonly Dictionary<string, int> flags;

        public UnityEvent<string> OnFlagChanged;

        public Flags()
        {
            var reader = new StreamReader(path);
            var json = reader.ReadToEnd();
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