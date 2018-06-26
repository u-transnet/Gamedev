using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Data;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using UnityEngine;

namespace uTrans
{
    public static class GeoUtils
    {
        public static double[] LocationToLongLatArray(Vector2d location)
        {
            double[] array = {
                location.y,
                location.x
            };
            return array;
        }

        public static Vector2d LongLatArrayToLocation(double[] array)
        {
            return new Vector2d(array[1], array[0]);
        }

        public static double UnitsToMeters(this AbstractMap map, Vector2d latlong, double units)
        {
            UnityTile tile;
            bool foundTile = map.MapVisualizer.ActiveTiles.TryGetValue(Conversions.LatitudeLongitudeToTileId(latlong.x, latlong.y, (int)map.Zoom), out tile);
            if (foundTile)
            {
                return (units / tile.TileScale);
            }
            else
            {
                return units;
            }
        }

        /*public static double MetersInPixelsFromMapZoom(this AbstractMap map, GameObject gameObject)
        {

        }*/
    }
}