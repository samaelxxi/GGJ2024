using System;
using System.Collections.Generic;
using UnityEngine;

public class Registry<Key, Value> : ScriptableObject
{
    [Serializable]
    class Entry<KeyEntry, ValueEntry>
    {
        public KeyEntry Key;
        public ValueEntry Value;
    }

    Dictionary<Key, Value> _registry;

    [field: SerializeField]
    private List<Entry<Key, Value>> RegistryList;

    [field: SerializeField]
    private Value Default;


    public Value Get(Key key) { 

        if(!RegistryDictionary.ContainsKey(key))
        {
            //Debug.LogWarning($"No entry found for {key}");
            return Default;
        }
        return RegistryDictionary[key];
    }


    public Dictionary<Key, Value> RegistryDictionary
    {
        get
        {
            if (_registry == null)
            {
                _registry = new Dictionary<Key, Value>();
                foreach (var entry in RegistryList)
                {
                    _registry.Add(entry.Key, entry.Value);
                }
            }
            return _registry;
        }
    }
}
