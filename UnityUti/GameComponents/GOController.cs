using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlugRMK.UnityUti
{
    [AddComponentMenu("Unity Utility/GO Controller")]
    public class GOController : MonoBehaviour
    {
        [Serializable]
        public class Pair
        {
            [SerializeField]
            string name;
            [SerializeField]
            GameObject go;

            public string Name { get => name; }
            public GameObject GO { get => go; }
        }

        public List<Pair> list = new();

        [TextArea(3, 15)]
        public string comment = "Hello, world!";

        public bool TryGet(string name, out GameObject foundGO)
        {
            foreach (var pair in list)
                if (pair.Name == name)
                {
                    foundGO = pair.GO;
                    return true;
                }

            foundGO = null;
            return false;
        }

        public GameObject Get(string name)
        {
            foreach (var pair in list)
                if (pair.Name == name)
                    return pair.GO;

            return null;
        }

        public void DebugLog(string text) => Debug.Log(text);
        
    }

}
