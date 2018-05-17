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

        public AbstractMap Map { get; set; }

        public Action<Vector2d> OnLocationUpdate = vectorD =>
        {
        };

        private Vector2d _location;

        public Vector2d Location
        {
            get
            {
                return _location;
            }
        }


        public void NewPos(Vector3 pos)
        {
            _location = Map.WorldToGeoPosition(pos);
            transform.localPosition = Map.GeoToWorldPosition(_location, true);
            var curScale = spawnScale * Map.transform.localScale.x;
            transform.localScale = new Vector3(curScale, curScale, curScale);
            OnLocationUpdate(Location);
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            transform.localPosition = Map.GeoToWorldPosition(_location, true);
            var curScale = spawnScale * Map.transform.localScale.x;
            transform.localScale = new Vector3(curScale, curScale, curScale);
        }


    }
}
