namespace uTrans.Calc
{
    using UnityEngine;
    using uTrans.Components;

    public enum PointType
    {
        Default,
        Light,
        Heavy

    }

    public class PointProps : MonoBehaviour
    {

        private BasePoint baseObject;

        [SerializeField]
        public PointType pointType;

        public float Altitude
        {
            get
            {
                return baseObject.onMapObject.Altitude;
            }
        }

        public TerrainType Terrain
        {
            get
            {
                var types = baseObject.onMapObject.TerrainTypes;
                if (types.Contains(TerrainType.water.Tag()))
                {
                    return TerrainType.water;
                }
                else if (types.Contains(TerrainType.swamp.Tag()))
                {
                    return TerrainType.swamp;
                }
                else if (types.Contains(TerrainType.wood.Tag()))
                {
                    return TerrainType.wood;
                }
                else if (types.Contains(TerrainType.grass.Tag()))
                {
                    return TerrainType.grass;
                }
                else
                    return TerrainType.normal;
            }
        }

        void Awake()
        {
            baseObject = GetComponent<BasePoint>();
        }
    }
}