using UnityEngine;

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
        public bool Active
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