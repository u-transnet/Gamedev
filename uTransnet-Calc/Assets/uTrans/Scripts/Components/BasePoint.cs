using UnityEngine;
using uTrans.Calc;
using uTrans.Data;
using uTrans.Services;

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
                    case uTrans.Calc.PointType.Heavy:
                        objectRenderer.material = heavyMaterial;
                    break;
                    case uTrans.Calc.PointType.Light:
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

        void Start()
        {
            onMapObject.OnLocationUpdate += vectorD => {
                debugText.text = "Point altitude: " + pointProps.Altitude + "m";
                debugText.text += "\nTerrain type: " + pointProps.Terrain.ToString("g");
            };
        }

    }
}