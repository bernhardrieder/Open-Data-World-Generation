using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[System.Serializable]
public abstract class ShapeFormat
{
    public List<DecimalDegree> coordinates = new List<DecimalDegree>();

    public ShapeFormat() { }

    public ShapeFormat(DecimalDegree coordinate)
    {
        coordinates.Add(coordinate);
    }

    public List<Vector3> ReturnRealCoordsAsUnityCoords()
    {
        List<Vector3> unityCoordinates = new List<Vector3>();
        foreach (var obj in coordinates)
        {
            Vector3 newUnityCoordinate = new Vector3((float)obj.Longitude, 0f, (float)obj.Latitude);
            unityCoordinates.Add(newUnityCoordinate);
        }
        return unityCoordinates;
    }
}

