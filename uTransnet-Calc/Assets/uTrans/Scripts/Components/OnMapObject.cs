namespace uTrans.Components
{
    using System;
    using System.Collections.Generic;
    using Mapbox.Unity.Map;
    using Mapbox.Utils;
    using UnityEngine;

    public class OnMapObject : Component
    {

        [SerializeField]
        public float spawnScale = 10f;

        [SerializeField]
        public bool fixedScale = false;

        [SerializeField]
        private float heightShift = 0;


        [SerializeField]
        private float altitude;

        public float Altitude
        {
            get
            {
                return altitude;
            }
            private set
            {
                altitude = value;
            }
        }

        [SerializeField]
        private AbstractMap map;

        public event Action OnLocationUpdate = () =>
        {
        };

        public event Action OnHeightChanged = () => {};

        [SerializeField]
        private Vector2d _location;

        public Vector2d Location
        {
            get
            {
                return _location;
            }
            private set
            {
                _location = value;
                OnLocationUpdate();
            }
        }

        private List<string> _terrainTypes = new List<string>();

        public List<string> TerrainTypes
        {
            get
            {
                return _terrainTypes;
            }
        }


        public OnMapObject()
        {
            OnLocationUpdate += () =>
            {
                Altitude = map.QueryElevationInMetersAt(Location);
            };
        }

        void Awake()
        {

            if(BaseObject != null)
            {
                BaseObject.OnReady += () =>
                {
                    OnLocationUpdate();
                };
            }
        }


        public void NewPos(Vector3 pos)
        {
            Location = map.WorldToGeoPosition(pos);
        }

        void Update()
        {
            transform.localPosition = map.GeoToWorldPosition(Location, true);
            if (heightShift > 0)
            {
                transform.localPosition += new Vector3(0, heightShift, 0);
            }
            if (!fixedScale)
            {
                var curScale = spawnScale * map.transform.localScale.x;
                transform.localScale = new Vector3(curScale, curScale, curScale);
            }
        }

        void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.layer == 8)
            {
                _terrainTypes.Add(collider.gameObject.tag);
            }
        }

        void OnTriggerExit(Collider collider)
        {
            if (collider.gameObject.layer == 8)
            {
                _terrainTypes.Remove(collider.gameObject.tag);
            }
        }

        public void SetMap(AbstractMap map)
        {
            this.map = map;
        }
    }
}
