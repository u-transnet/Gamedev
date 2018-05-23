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

        public AbstractMap Map { get; set; }

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
                Altitude = Map.QueryElevationInMetersAt(newLocation);
            };
        }

        void OnWillRenderObject()
        {
            Location = Location;
        }


        public void NewPos(Vector3 pos)
        {
            Location = Map.WorldToGeoPosition(pos);
            transform.localPosition = Map.GeoToWorldPosition(Location, true);
            var curScale = spawnScale * Map.transform.localScale.x;
            transform.localScale = new Vector3(curScale, curScale, curScale);
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            transform.localPosition = Map.GeoToWorldPosition(Location, true);
            var curScale = spawnScale * Map.transform.localScale.x;
            transform.localScale = new Vector3(curScale, curScale, curScale);
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
    }
}
