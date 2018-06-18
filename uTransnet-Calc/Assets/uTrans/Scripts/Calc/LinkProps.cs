using System;
using Mapbox.CheapRulerCs;
using Mapbox.Unity.Location;
using Mapbox.Utils;
using UnityEngine;
using uTrans.Components;

public enum LinkType
{
    Unset,
    Soft,
    Rigid
}

public class LinkProps : MonoBehaviour
{

    private BaseLink baseObject;

    [SerializeField]
    public LinkType linkType;

    public float Altitude
    {
        get
        {
            return (baseObject.FirstPoint.onMapObject.Altitude + baseObject.SecondPoint.onMapObject.Altitude) / 2;
        }
    }

    public float Elevation
    {
        get
        {
            return Math.Abs(baseObject.FirstPoint.onMapObject.Altitude - baseObject.SecondPoint.onMapObject.Altitude);
        }
    }

    public Vector2d Position
    {
        get
        {
            return (baseObject.FirstPoint.onMapObject.Location + baseObject.SecondPoint.onMapObject.Location) / 2;
        }
    }

    public double Slope
    {
        get
        {
            return Math.Tan(Elevation/Length) * 100;
        }
    }

    public double Length
    {
        get
        {
            var ruler = new CheapRuler(Position.x, CheapRulerUnits.Meters);

            // Distance accepts [longitude, latitude], so coordinates should be in reverse order

            double[] array1 =
            {
                baseObject.FirstPoint.onMapObject.Location.y,
                baseObject.FirstPoint.onMapObject.Location.x
            };
            double[] array2 =
            {
                baseObject.SecondPoint.onMapObject.Location.y,
                baseObject.SecondPoint.onMapObject.Location.x
            };
            return ruler.Distance(
                    array1,
                    array2
            );
        }
    }

    void Awake()
    {
        baseObject = GetComponent<BaseLink>();
    }
}