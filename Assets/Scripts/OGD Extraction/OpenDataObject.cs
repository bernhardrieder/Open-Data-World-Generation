using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[System.Serializable]
public class OpenDataObject
{
    [SerializeField]
    private string name, street, district;
    [SerializeField]
    private ShapeFormat shape;

    public ShapeFormat Shape { get; set; }
    public string Name { get; set; }
    public string Street { get; set; }
    public string District { get; set; }

    public OpenDataObject() { }
    public OpenDataObject (ShapeFormat shapeformat)
    {
        Shape = shapeformat;
    }
}

