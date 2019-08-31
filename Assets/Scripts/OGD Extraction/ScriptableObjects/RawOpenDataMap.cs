using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class RawOpenDataMap : ScriptableObject 
{
    [System.Serializable]
    public class OpenDataEntry 
    {
        public TextAsset openDataFile;
        public GameObject relatedPrefab;

    }
    public List<OpenDataEntry> dataEntries;
}
