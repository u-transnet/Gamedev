using System;

namespace uTrans.Calc
{
    public enum TerrainType
    {
        wood,
        grass,
        swamp,
        water,
        normal
    }

    public static class TerrainTypeMethods
    {

        public static String Tag(this TerrainType s1)
        {
            switch (s1)
            {
                case TerrainType.wood:
                    return "wood";
                case TerrainType.grass:
                    return "grass";
                case TerrainType.swamp:
                    return "swamp";
                case TerrainType.water:
                    return "water";
                case TerrainType.normal:
                    return "normal";
                default:
                    return "";
            }
        }
    }
}