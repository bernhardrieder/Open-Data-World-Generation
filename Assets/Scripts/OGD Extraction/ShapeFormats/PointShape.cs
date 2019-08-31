using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[System.Serializable]
public class PointShape : ShapeFormat
{
    public PointShape() : base() { }
    public PointShape(DecimalDegree coordinate) : base(coordinate) { }
}

