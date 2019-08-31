using UnityEngine;
using UnityEditor;
using System.Collections;

public class OGDWorldGeneratorWindow : EditorWindow 
{
    RawOpenDataMap rawOGDMap = null;
    bool extractFromFile = false;
    string extractedFilename = "";


    ExtractedOpenDataMap extractedOGDMap = null;
    bool createGameObjectsFromFile = false;
    Color lineColor = Color.white;
    Material lineMaterial = new Material(Shader.Find("Particles/Additive"));
    float lineWidth = 5f;

    // Add menu item named "OGD World Generator" to the Window menu
    [MenuItem ("Window/OGD World Generator")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(OGDWorldGeneratorWindow));
    }

    void OnGUI()
    {
        GUILayout.Label("OGD Extraction", EditorStyles.boldLabel);
       
        extractFromFile = EditorGUILayout.BeginToggleGroup("Extract data from source file?", extractFromFile);
        rawOGDMap = EditorGUILayout.ObjectField("Source File: ", rawOGDMap, typeof(RawOpenDataMap), false) as RawOpenDataMap;
        extractedFilename = EditorGUILayout.TextField("Destination Filename:", extractedFilename);

        GUI.enabled = (rawOGDMap != null) && (extractedFilename.Length > 0) && extractFromFile;
        if(GUILayout.Button("Create"))
        {
            OpenDataExtractor.ExtractRawOpenDataMap(rawOGDMap, extractedFilename);
        }

        EditorGUILayout.EndToggleGroup();



        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();



        GUILayout.Label("World Generation", EditorStyles.boldLabel);
        createGameObjectsFromFile = EditorGUILayout.BeginToggleGroup("Create world objects from extracted file?", createGameObjectsFromFile);
        extractedOGDMap = EditorGUILayout.ObjectField("Source File: ", extractedOGDMap, typeof(ExtractedOpenDataMap), false) as ExtractedOpenDataMap;

        EditorGUILayout.Space();
        GUILayout.Label("If there are Linestring, Polygon or Multipolygon in the files,\nhow should the line between 2 points look like?", EditorStyles.boldLabel);
        lineColor = EditorGUILayout.ColorField("Color: ", lineColor);
        lineMaterial = EditorGUILayout.ObjectField("Material: ", lineMaterial, typeof(Material), false) as Material;
        lineWidth = EditorGUILayout.FloatField("Line width: ", lineWidth);

        GUI.enabled = createGameObjectsFromFile && extractedOGDMap;
        if (GUILayout.Button("Create"))
        {
            OGDWorldGenerator.GenerateContent(extractedOGDMap, lineColor, lineMaterial, lineWidth);
            //OGDWorldGenerator.TestCoordinatesFrom(extractedOGDMap);
        }

        EditorGUILayout.EndToggleGroup();
    }
}
