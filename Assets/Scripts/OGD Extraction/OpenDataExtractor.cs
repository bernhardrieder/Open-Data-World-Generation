using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public static class OpenDataExtractor 
{
    private const float lowestLongitude = 16.177f;
    private const float lowestLatitude = 48.115f;

    public static void ExtractRawOpenDataMap(RawOpenDataMap rawMap, string extractedFilename)
    {
        ExtractedOpenDataMap finishedMap = ScriptableObject.CreateInstance<ExtractedOpenDataMap>();
        int fileCount = 0;
        foreach(var entry in rawMap.dataEntries)
        {
            List<OpenDataObject> objectsFromCsv = CsvReader.GetEveryLineAsObjectFromCSV(entry.openDataFile);

            for (int i = 0; i < objectsFromCsv.Count; i ++ )
            {
                if(objectsFromCsv[i].Shape is PointShape)
                {
                    objectsFromCsv[i] = ApplyAllRules(objectsFromCsv[i]);
                }
                else if (objectsFromCsv[i].Shape is LinestringShape)
                {
                    objectsFromCsv[i] = ApplyAllRules(objectsFromCsv[i]);
                }
                else if (objectsFromCsv[i].Shape is PolygonShape)
                {
                    objectsFromCsv[i] = ApplyAllRules(objectsFromCsv[i]);
                }
                else if (objectsFromCsv[i].Shape is MultipolygonShape)
                {
                    MultipolygonShape multipolygon = (MultipolygonShape) objectsFromCsv[i].Shape;

                    for (int y = 0; y < multipolygon.polygonShapes.Count; y++)
                    {
                        for (int index = 0; index < multipolygon.polygonShapes[y].coordinates.Count; index++)
                        {
                            multipolygon.polygonShapes[y].coordinates[index] = ApplyCoordinatePreparationRules(multipolygon.polygonShapes[y].coordinates[index]);
                        }
                    }
                    objectsFromCsv[i].Shape = multipolygon;
                }
                
                SaveInDestinationObject(finishedMap, new ExtractedOpenDataEntry(entry.relatedPrefab, objectsFromCsv[i], entry.openDataFile.name));
            }
            Debug.Log("Files: " + fileCount++);
        }
        SaveFinishedDestinationFile(finishedMap, extractedFilename);
        Debug.Log("Finished Extraction");
    }
    
    private static OpenDataObject ApplyAllRules(OpenDataObject obj)
    {
        for (int index = 0; index < obj.Shape.coordinates.Count; index++)
        {
            obj.Shape.coordinates[index] = ApplyCoordinatePreparationRules(obj.Shape.coordinates[index]);
        }
        return obj;
    }

    private static void SaveInDestinationObject(ExtractedOpenDataMap mapInstance, ExtractedOpenDataEntry entry)
    {
        mapInstance.extractedOpenDataEntries.Add(entry);
    }

    private static void SaveFinishedDestinationFile(ExtractedOpenDataMap mapInstance, string assetName)
    {
        AssetDatabase.CreateAsset(mapInstance, "Assets/ScriptableObjects/Output/" + assetName + ".asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = mapInstance;
    }

    private static DecimalDegree ApplyCoordinatePreparationRules(DecimalDegree coordinate)
    {
        coordinate = SubtractLowestCoordinates(coordinate);
        coordinate = RoundCoordinatesOnto7DigitsAndConvertToUnityUnits(coordinate);

        return coordinate;
    }

    private static DecimalDegree SubtractLowestCoordinates(DecimalDegree coordinate)
    {
        if (!(coordinate.Latitude == 0) || !(coordinate.Longitude == 0))
        {
            coordinate.Longitude -= (decimal)lowestLongitude;
            coordinate.Latitude -= (decimal)lowestLatitude;
        }

        return coordinate;
    }

    private static DecimalDegree RoundCoordinatesOnto7DigitsAndConvertToUnityUnits(DecimalDegree coordinate)
    {
        coordinate.Latitude *= 10000000;
        coordinate.Longitude *= 10000000;

        coordinate.Latitude = (decimal)Mathf.Round((float)coordinate.Latitude);
        coordinate.Longitude = (decimal)Mathf.Round((float)coordinate.Longitude);

        coordinate.Latitude /= 100;
        coordinate.Longitude /= 100;

        return coordinate;
    }
}
