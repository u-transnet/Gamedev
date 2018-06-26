namespace uTrans.Components
{
    using System;
    using Mapbox.Unity.Map;
    using UnityEngine;

    public class ObjectWithHeight : Component
    {

        [SerializeField]
        private float height = 10;

        public float Height
        {
            get
            {
                return height;
            }

            set
            {
                height = value;
                transform.localScale = new Vector3(
                        transform.localScale.x,
                        height / (10 * ((BasePoint)BaseObject).onMapObject.spawnScale),
                        transform.localScale.z
                );
                OnHeightChanged();
            }
        }

        public float UnityHeight
        {
            get
            {
                return transform.lossyScale.y * 2;
            }
        }

        public event Action OnHeightChanged = () =>
        {
        };

        [SerializeField]
        private AbstractMap map;


        public ObjectWithHeight()
        {
            OnHeightChanged += () =>
            {
//                height = 10 * transform.localScale.y * ((BasePoint)BaseObject).onMapObject.spawnScale;
                transform.localPosition = new Vector3(0, transform.localScale.y, 0);
            };
        }

        void Awake()
        {


            BaseObject.OnReady += () =>
            {
                //Trigger height update
                Height = Height;
            };
        }


        public void SetMap(AbstractMap map)
        {
            this.map = map;
        }

        public void Up()
        {
            Height += 5f;
        }

        public void Down()
        {

            Height -= 5f;
        }

        private double MetersOffset(int meters)
        {
            float scaledHeight = GetComponent<MeshRenderer>().bounds.extents.y * transform.localScale.y * transform.parent.localScale.y;
            return meters / Math.Pow(1 / map.transform.localScale.x, 2) - scaledHeight;
        }
    }
}