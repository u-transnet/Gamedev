using Mapbox.Unity.Map;
using UnityEngine;
using uTrans.Calc;

namespace uTrans.Components
{
    public class BasePoint : BaseObject
    {
        [SerializeField]
        public OnMapObject onMapObject;
        [SerializeField]
        public GameObject pin;
        [SerializeField]
        public PointProps pointProps;
        [SerializeField]
        public DraggableObject draggableObject;
        [SerializeField]
        public ObjectWithHeight objectWithHeight;


        [SerializeField]
        public Material lightMaterial;
        [SerializeField]
        public Material heavyMaterial;

        public PointType PointType
        {
            get
            {
                return pointProps.pointType;
            }
            set
            {
                pointProps.pointType = value;
                switch (value)
                {
                    case uTrans.Calc.PointType.Terminal:
                        objectRenderer.material = heavyMaterial;
                        break;
                    case uTrans.Calc.PointType.AnchorPylon:
                        objectRenderer.material = lightMaterial;
                        break;
                }
            }
        }


        [SerializeField]
        override public bool Active
        {
            get
            {
                return base.Active;
            }
            set
            {
                base.Active = value;
                if (draggableObject != null)
                {
                    draggableObject.enabled = value;
                }
                if (pin != null)
                {
                    pin.SetActive(value);
                }
            }
        }

        void Awake()
        {
            onMapObject.OnLocationUpdate += DrawDebug;
            objectWithHeight.OnHeightChanged += DrawDebug;
        }

        private void DrawDebug()
        {
            debugText.text = "Point altitude: " + pointProps.Altitude + "m";
            debugText.text += "\nPoint height: " + pointProps.Height + "m";
            debugText.text += "\nTerrain type: " + pointProps.Terrain.ToString("g");
        }

        public void Up()
        {
            objectWithHeight.Up();
        }

        public void Down()
        {
            objectWithHeight.Down();
        }

        public void SetMap(AbstractMap map)
        {
            onMapObject.SetMap(map);
            objectWithHeight.SetMap(map);
        }

        public void SetBuildingManager(SpawnOnMapD buildingManager)
        {
            draggableObject.buildingManager = buildingManager;
            pin.GetComponent<DraggableObject>().buildingManager = buildingManager;
        }

    }
}