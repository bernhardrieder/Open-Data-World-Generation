using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ExtractedOpenDataEntry
{
    public ExtractedOpenDataEntry(GameObject relatedPrefab, OpenDataObject openDataObject, string name)
    {
        this.relatedPrefab = relatedPrefab;
        this.openDataObject = openDataObject;
        this.name = name;
    }

    public GameObject relatedPrefab;
    public string name;
    public OpenDataObject openDataObject;
}