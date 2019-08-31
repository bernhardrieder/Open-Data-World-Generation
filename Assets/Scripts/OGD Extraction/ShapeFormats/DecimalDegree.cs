using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[System.Serializable]
public class DecimalDegree
{
    [SerializeField]
    private decimal latitude;
    [SerializeField]
    private decimal longitude;

    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }

    public DecimalDegree() { }
    public DecimalDegree(decimal lat, decimal lon)
    {
        Latitude = lat;
        Longitude = lon;
    }

    public void ParseDecimalToLatitudeOrLongitude(decimal number)
    {
        if (number > 47)
        {
            Latitude = number;
        }
        else if (number < 17)
        {
            Longitude = number;
        }
    }
}
