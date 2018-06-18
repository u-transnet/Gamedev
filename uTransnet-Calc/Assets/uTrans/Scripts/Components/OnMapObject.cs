using System.Collections.Generic;

namespace uTrans.Components
{
    using System;
    using Mapbox.Unity.Map;
    using Mapbox.Utils;
    using UnityEngine;

    public class OnMapObject : MonoBehaviour
    {

        [SerializeField]
        public float spawnScale = 10f;

        [SerializeField]
        public bool fixedScale = false;

        [SerializeField]
        private float heightShift = 0;


        [SerializeField]
        private float _altitude;

        public float Altitude
        {
            get
            {
                return _altitude;
            }
            private set
            {
                _altitude = value;
            }
        }

        [SerializeField]
        private AbstractMap map;

        public event Action<Vector2d> OnLocationUpdate = vectorD =>
        {
        };

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
                OnLocationUpdate(_location);
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

        void Start()
        {
            OnLocationUpdate += newLocation =>
            {
                Altitude = map.QueryElevationInMetersAt(newLocation);
            };
        }

        void OnWillRenderObject()
        {
            // Trigger event
            Location = Location;
        }


        public void NewPos(Vector3 pos)
        {
            Location = map.WorldToGeoPosition(pos);
        }

        void Update()
        {
            transform.localPosition = map.GeoToWorldPosition(Location, true);
            if(heightShift > 0)
            {
                transform.localPosition += new Vector3(0, heightShift, 0);
            }
            if(!fixedScale)
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
