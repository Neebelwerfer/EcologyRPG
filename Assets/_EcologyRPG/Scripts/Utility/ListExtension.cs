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
}
