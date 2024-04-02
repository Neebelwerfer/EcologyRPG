using System.Collections.Generic;

namespace EcologyRPG.Core
{
    public class Flags
    {
        static Flags instance;
        readonly Dictionary<string, int> _intFlags;
        readonly Dictionary<string, string> _stringFlags;
        readonly Dictionary<string, float> _floatFlags;

        Flags()
        {
            _floatFlags = new Dictionary<string, float>();
            _intFlags = new Dictionary<string, int>();
            _stringFlags = new Dictionary<string, string>();
        }

        public static void Init()
        {
            instance ??= new Flags();
        }

        public static void SetInt(string key, int value)
        {
            instance._SetInt(key, value);
        }

        public static void SetFloat(string key, float value)
        {
            instance._SetFloat(key, value);
        }

        public static void SetString(string key, string value)
        {
            instance._SetString(key, value);
        }

        public static int GetInt(string key, int defaultValue)
        {
            return instance._GetInt(key, defaultValue);
        }

        public static float GetFloat(string key, float defaultValue)
        {
            return instance._GetFloat(key, defaultValue);
        }

        public static string GetString(string key, string defaultValue)
        {
            return instance._GetString(key, defaultValue);
        }

        protected void _SetFloat(string key, float value)
        {
            if(_floatFlags.ContainsKey(key))
                _floatFlags[key] = value;
            else
                _floatFlags.Add(key, value);
        }
        protected void _SetString(string key, string value)
        {
            if (_stringFlags.ContainsKey(key))
                _stringFlags[key] = value;
            else
                _stringFlags.Add(key, value);
        }

        protected void _SetInt(string key, int value)
        {
            if (_intFlags.ContainsKey(key))
                _intFlags[key] = value;
            else
                _intFlags.Add(key, value);
        }

        protected float _GetFloat(string key, float defaultValue)
        {
            if(_floatFlags.ContainsKey(key))
                return _floatFlags[key];
            
            _floatFlags.Add(key, defaultValue);
            return defaultValue;
        }

        protected string _GetString(string key, string defaultValue)
        {
            if(_stringFlags.ContainsKey(key))
                return _stringFlags[key];
            _stringFlags.Add(key, defaultValue);
            return defaultValue;
        }

        protected int _GetInt(string key, int defaultValue)
        {
            if(_intFlags.ContainsKey(key))
                return _intFlags[key];
            _intFlags.Add(key, defaultValue);
            return defaultValue;
        }
    }
}