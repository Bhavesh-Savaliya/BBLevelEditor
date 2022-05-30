using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapContent : ScriptableObject
{
    [System.Serializable]
    public struct Content
    {
        public CellContentType cct;
        public GameObject[] Objects;
    }

    public Content[] contents;

}
