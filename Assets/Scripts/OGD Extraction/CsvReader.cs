using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

public static class CsvReader 
{
    public static List<OpenDataObject> GetEveryLineAsObjectFromCSV(TextAsset csvFile)
    {
        List<OpenDataObject> csvObjects = new List<OpenDataObject>();

        string[] allLines = csvFile.text.Split("\n"[0]);

        Regex csvRegex = new Regex(@"""[^""]*""|'[^']*'|[^,]*");

        foreach (var line in allLines)
        {
            MatchCollection matches = csvRegex.Matches(line);
            OpenDataObject listEntry = new OpenDataObject();
            ShapeFormat newShape = GetShapeFromLine(matches);

            if (newShape != null)
            {
                listEntry.Shape = newShape;
                //listEntry.Name = ...
                //listEntry.Street = ...
                //listEntry....
                csvObjects.Add(listEntry);
            }
        }
        return csvObjects;
    }


    private static ShapeFormat GetShapeFromLine(MatchCollection matchedElementsOfALine)
    {
        ShapeFormat returnShape = null;
        
        foreach (var match in matchedElementsOfALine)
        {
            string lineElement = match.ToString();

            if (lineElement.Contains("POINT"))
            {
                returnShape = GetPointShape(lineElement);
                return returnShape;
            }
            else if (lineElement.Contains("LINESTRING"))
            {
                returnShape = GetLinestringShape(lineElement);
                return returnShape;
            }
            else if (lineElement.Contains("MULTIPOLYGON"))
            {
                returnShape = GetMultipolygonShape(lineElement);
                return returnShape;
            }
            else if (lineElement.Contains("POLYGON"))
            {
                returnShape = GetPolygonShape(lineElement);
                return returnShape;
            }
        }
        return returnShape;
    }

    private static PointShape GetPointShape(string lineElement)
    {
        // e.g. POINT (16.0000 48.0000)
        return new PointShape(ParseIntoDecimalDegreeFrom(lineElement));
    }

    private static LinestringShape GetLinestringShape(string lineElement)
    {
        // LINESTRING (16.000000 48.00000, 16.0002 48.261, 16.003 48.262)

        /*
         * regex returns group of coordinate pairs like:
         * 16.000000 48.00000
         * 16.0002 48.261
         * 16.003 48.262
         * 
         */
        Regex regex = new Regex(@"(?:\d{1,3}\.\d{1,20})\s(?:\d{1,3}\.\d{1,20})");
        MatchCollection matches = regex.Matches(lineElement);

        LinestringShape linestringShape = new LinestringShape();

        foreach(var match in matches)
        {
            linestringShape.coordinates.Add(ParseIntoDecimalDegreeFrom(match.ToString()));
        }

        return linestringShape;
    }

    private static PolygonShape GetPolygonShape(string lineElement)
    {
        // POLYGON ((16.000000 48.00000, 16.0002 48.261, 16.003 48.262))

        //regular expression is the same as the linestring regex
        Regex regex = new Regex(@"(?:\d{1,3}\.\d{1,20})\s(?:\d{1,3}\.\d{1,20})");
        MatchCollection matches = regex.Matches(lineElement);

        PolygonShape polygonShape = new PolygonShape();

        foreach(var match in matches)
        {
            polygonShape.coordinates.Add(ParseIntoDecimalDegreeFrom(match.ToString()));
        }
        
        return polygonShape;
    }

    private static MultipolygonShape GetMultipolygonShape (string lineElement)
    {
        // MULTIPOLYGON (((16.000000 48.00000, 16.0002 48.261)), ((16.003 48.262, 16.0002 48.261)))
        Regex regex = new Regex(@"\(([^)]*)\)");
        MatchCollection matches = regex.Matches(lineElement);

        MultipolygonShape multipolygonShape = new MultipolygonShape();

        foreach(var match in matches)
        {
            multipolygonShape.polygonShapes.Add(GetPolygonShape(match.ToString()));
        }

        return multipolygonShape;
    }

    private static DecimalDegree ParseIntoDecimalDegreeFrom(string lineElementWithCoordinate)
    {
        //regex returns individual decimal digits in a format of 000.00000000000..
        Regex regex = new Regex(@"(?:\d{1,3}\.\d{1,20})");
        MatchCollection matches = regex.Matches(lineElementWithCoordinate);

        DecimalDegree coordinate = new DecimalDegree();

        foreach (var match in matches)
        {
            coordinate.ParseDecimalToLatitudeOrLongitude(Decimal.Parse(match.ToString()));
        }
        return coordinate;
    }
}