namespace Mapbox.Examples
{
    using Mapbox.Unity.Location;
    using UnityEngine;

    public class ImmediatePositionWithLocationProvider : MonoBehaviour
    {
        //[SerializeField]
        //private UnifiedMap _map;

        bool _isInitialized;

        bool _updated = false;

        ILocationProvider _locationProvider;

        ILocationProvider LocationProvider
        {
            get
            {
                if (_locationProvider == null)
                {
                    _locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
                }

                return _locationProvider;
            }
        }

        Vector3 _targetPosition;

        void Start()
        {
            LocationProviderFactory.Instance.mapManager.OnInitialized += () => _isInitialized = true;
//            LocationProviderFactory.Instance.mapManager.OnInitialized += UpdateLocation;
            UpdateLocation();
        }

        void UpdateLocation()
        {
//            if (_isInitialized)
            {
                var map = LocationProviderFactory.Instance.mapManager;
                LocationProvider.OnLocationUpdated += location =>
                {
                    if (!_updated)
                    {
                        map.Initialize(location.LatitudeLongitude, map.AbsoluteZoom);
                        Debug.Log("loc: " + location);
                        _updated = true;
                    }
                };
            }
        }
    }
}