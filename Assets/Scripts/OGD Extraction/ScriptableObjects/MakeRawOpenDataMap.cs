using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

class MakeRawOpenDataMap
{
    [MenuItem("Assets/Create/Raw Open Data Map")]
    public static void CreateMyAsset()
    {
        RawOpenDataMap asset = ScriptableObject.CreateInstance<RawOpenDataMap>();

        AssetDatabase.CreateAsset(asset, "Assets/ScriptableObjects/Input/NewOpenDataMap.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}
