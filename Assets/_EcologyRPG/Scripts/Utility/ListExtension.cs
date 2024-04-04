using System;
using System.Collections.Generic;

namespace EcologyRPG.Utility
{
    [Serializable]
    public class SerializableList<T>
    {
        public List<T> list;

        public SerializableList()
        {
            list = new List<T>();
        }
    }

    public static class ListExtention
    {
        public static SerializableList<T> ToSerializable<T>(this List<T> list) { return new SerializableList<T>() { list = list }; }

    }

    [Serializable]
    public class SerializableDictionary<TKey, TValue>
    {

        public List<TKey> keys;
        public List<TValue> values;

        public SerializableDictionary()
        {
            keys = new List<TKey>();
            values = new List<TValue>();
        }
    }

    public static class DictionaryExtention
    {
        public static SerializableDictionary<TKey, TValue> ToSerializable<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        {
            var keys = new List<TKey>();
            var values = new List<TValue>();

            foreach (var kvp in dict)
            {
                keys.Add(kvp.Key);
                values.Add(kvp.Value);
            }

            return new SerializableDictionary<TKey, TValue>() { keys = keys, values = values };
        }

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this SerializableDictionary<TKey, TValue> dict)
        {
            var result = new Dictionary<TKey, TValue>();

            for (int i = 0; i < dict.keys.Count; i++)
            {
                result.Add(dict.keys[i], dict.values[i]);
            }

            return result;
        }
    }
}
