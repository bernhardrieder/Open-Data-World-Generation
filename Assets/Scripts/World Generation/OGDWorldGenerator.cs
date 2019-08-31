using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class OGDWorldGenerator
{
    public static void GenerateContent (ExtractedOpenDataMap dataMap, Color lineColor, Material lineMaterial, float lineWidth)
    {
        foreach(var entry in dataMap.extractedOpenDataEntries)
        {
            if(entry.openDataObject.Shape is PointShape)
            {
                GeneratePointShape(entry);
            } 
            else if (entry.openDataObject.Shape is PolygonShape)
            {
                GeneratePolygonShape(entry, lineColor, lineMaterial, lineWidth);
            }
            else if (entry.openDataObject.Shape is LinestringShape)
            {
                GenerateLinestringShape(entry, lineColor, lineMaterial, lineWidth);
            } 
            else if (entry.openDataObject.Shape is MultipolygonShape)
            {
                GenerateMultipolygonShape(entry, lineColor, lineMaterial, lineWidth);
            }
            else
            {
                Debug.LogError("Error, couldn't generate content. No usable coordinates to instantiate object.");
            }
        }
    }


    private static void GeneratePointShape(ExtractedOpenDataEntry entry)
    {
        List<Vector3> coordinates = entry.openDataObject.Shape.ReturnRealCoordsAsUnityCoords();
        for(int i = 0 ; i < coordinates.Count; i ++)
        {
            GameObject obj = GameObject.Instantiate(entry.relatedPrefab, coordinates[i], entry.relatedPrefab.transform.rotation) as GameObject;
            obj.transform.parent = FindParentInHierarchy(entry).transform;
        }
    }

    private static void GenerateLinestringShape(ExtractedOpenDataEntry entry, Color lineColor, Material lineMaterial, float lineWidth)
    {
        List<Vector3> coordinates = entry.openDataObject.Shape.ReturnRealCoordsAsUnityCoords();
        
        for (int i = 0; i < coordinates.Count; i++)
        {
            Vector3 firstVertex = coordinates[i];
            Vector3 secondVertex = ((i + 1) == coordinates.Count) ? coordinates[i] : coordinates[i+1];

            GameObject obj = GameObject.Instantiate(entry.relatedPrefab, coordinates[i], entry.relatedPrefab.transform.rotation) as GameObject;
         
            if (firstVertex != secondVertex)
            {
                obj = AddLineFromTo(firstVertex, secondVertex, obj, lineColor, lineMaterial, lineWidth);
            }
            obj.transform.parent = FindParentInHierarchy(entry).transform;
        }
    }


    private static void GeneratePolygonShape(ExtractedOpenDataEntry entry, Color lineColor, Material lineMaterial, float lineWidth)
    {
        List<Vector3> coordinates = entry.openDataObject.Shape.ReturnRealCoordsAsUnityCoords();

        for (int i = 0; i < coordinates.Count; i++)
        {
            Vector3 firstVertex = coordinates[i];
            Vector3 secondVertex = ((i + 1) == coordinates.Count) ? coordinates[i] : coordinates[i + 1];

            GameObject obj = GameObject.Instantiate(entry.relatedPrefab, coordinates[i], entry.relatedPrefab.transform.rotation) as GameObject;

            if (firstVertex != secondVertex)
            {
                obj = AddLineFromTo(firstVertex, secondVertex, obj, lineColor, lineMaterial, lineWidth);
            } 
            else if (firstVertex == secondVertex)
            {
                obj = AddLineFromTo(secondVertex, coordinates[0], obj, lineColor, lineMaterial, lineWidth);
            }
            obj.transform.parent = FindParentInHierarchy(entry).transform;
        }
    }

    private static void GenerateMultipolygonShape(ExtractedOpenDataEntry entry, Color lineColor, Material lineMaterial, float lineWidth)
    {
        MultipolygonShape multipolgyonShape = (MultipolygonShape) entry.openDataObject.Shape;
        List<PolygonShape> polygons = multipolgyonShape.polygonShapes;

        foreach(PolygonShape polygon in polygons)
        {
            ExtractedOpenDataEntry tempEntry = new ExtractedOpenDataEntry(entry.relatedPrefab, new OpenDataObject(polygon), entry.name);
            GeneratePolygonShape(tempEntry, lineColor, lineMaterial, lineWidth);
        }
    }

    private static GameObject AddLineFromTo(Vector3 from, Vector3 to, GameObject addToObject, Color lineColor, Material lineMaterial, float lineWidth)
    {
        LineRenderer lineRenderer = addToObject.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.SetColors(lineColor, lineColor);
        lineRenderer.SetWidth(lineWidth, lineWidth);
        lineRenderer.SetPosition(0, from);
        lineRenderer.SetPosition(1, to);
        return addToObject;
    }

    private static GameObject FindParentInHierarchy(ExtractedOpenDataEntry entry)
    {
        if (GameObject.Find(entry.name) == null)
        {
            return new GameObject(entry.name);
        }
        else if (GameObject.Find(entry.name) != null)
        {
            return GameObject.Find(entry.name);
        }
        else
        {
            return new GameObject("undefind");
        }
    }

    public static void TestCoordinatesFrom(ExtractedOpenDataMap dataMap)
    {
        foreach (var entry in dataMap.extractedOpenDataEntries)
        {
            //Debug.Log(entry.openDataObject.Shape.coordinates[0].Latitude);
            for (int i = 0; i < entry.openDataObject.Shape.coordinates.Count; i++ )
            {
                Debug.Log("Decimal Koordinaten: X: " + entry.openDataObject.Shape.coordinates[i].Longitude + " \tZ: " + entry.openDataObject.Shape.coordinates[i].Latitude);
                Vector3 newCoordinate = new Vector3((float)entry.openDataObject.Shape.coordinates[i].Longitude, 0f, (float)entry.openDataObject.Shape.coordinates[i].Latitude);
                Debug.Log("Unity Koordinaten: X: " + newCoordinate.x + " \tZ: " + newCoordinate.z);
            }
                
        }
    }
	
}
