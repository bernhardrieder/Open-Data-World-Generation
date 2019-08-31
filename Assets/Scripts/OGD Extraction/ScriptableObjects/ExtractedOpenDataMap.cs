using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ExtractedOpenDataMap : ScriptableObject 
{
    public List<ExtractedOpenDataEntry> extractedOpenDataEntries = new List<ExtractedOpenDataEntry>();
}
