namespace Mapbox.Examples
{
	using UnityEngine;
	using Mapbox.Utils;
	using Mapbox.Unity.Map;
	using Mapbox.Unity.MeshGeneration.Factories;
	using Mapbox.Unity.Utilities;
	using System.Collections.Generic;

	public class SpawnOnMapD : MonoBehaviour
	{
		[SerializeField]
		AbstractMap _map;
        
		List<Vector2d> _locations;

		[SerializeField]
		float _spawnScale = 100f;

		[SerializeField]
		GameObject _markerPrefab;

        [SerializeField]
        Camera cam;

		List<GameObject> _spawnedObjects;

		void Start()
		{
			_locations = new List<Vector2d>();
			_spawnedObjects = new List<GameObject>();
		}

		private void Update()
		{
            if (Input.GetButtonDown("Fire1"))
            {
                var mousePos = Input.mousePosition;
                //mousePos.z = 2.0;       // we want 2m away from the camera position
                var objectPos = cam.ScreenToWorldPoint(mousePos);
                _locations.Add(Spawn(objectPos));
            }

            int count = _spawnedObjects.Count;
			for (int i = 0; i < count; i++)
			{
				var spawnedObject = _spawnedObjects[i];
				var location = _locations[i];
				spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
				spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
			}
		}

        private Vector2d Spawn(Vector3 pos)
        {
            var _location = _map.WorldToGeoPosition(pos);
            var instance = Instantiate(_markerPrefab);
            instance.transform.localPosition = _map.GeoToWorldPosition(_location, true);
            instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
            _spawnedObjects.Add(instance);
            return _location;
        }
	}
}