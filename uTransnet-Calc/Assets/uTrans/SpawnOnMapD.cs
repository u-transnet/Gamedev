namespace Mapbox.Examples
{
	using UnityEngine;
	using Mapbox.Utils;
	using Mapbox.Unity.Map;
	using Mapbox.Unity.MeshGeneration.Factories;
	using Mapbox.Unity.Utilities;
	using System.Collections.Generic;
    using UnityEngine.UI;

    public class SpawnOnMapD : MonoBehaviour
    {
        [SerializeField]
        AbstractMap _map;

        [SerializeField]
        GameObject _markerPrefab;

        [SerializeField]
        Camera cam;

        [SerializeField]
        public Button pointButton;

        [SerializeField]
        public Button yesButton;

        [SerializeField]
        public Button noButton;

        [SerializeField]
        public Button linkButton;

        private GameObject activePoint = null;

        public bool PointerUsed { get; set; }

        void Start()
        {
            pointButton.GetComponent<Button>().onClick.AddListener(SpawnOnCenter);
            yesButton.GetComponent<Button>().onClick.AddListener(ConfirmPoint);
            noButton.GetComponent<Button>().onClick.AddListener(RemoveActivePoint);

            PointerUsed = false;
        }

        private void Update() {
		
            if (activePoint != null)
            {
                yesButton.interactable = !activePoint.GetComponent<CollidingObject>().colliding;
            }
            else
            {
                yesButton.interactable = false;
            }
        }

        private Vector2d Spawn(Vector3 pos)
        {
            var _location = _map.WorldToGeoPosition(pos);
            activePoint = Instantiate(_markerPrefab);
            var node = activePoint.GetComponent<OnMapObject>();
            node.Map = _map;
            node.NewPos(pos);
            node.GetComponent<DraggableObject>().BuildingManager = this;
            return _location;
        }

        public void SpawnOnCenter()
        {
            pointButton.interactable = false;
            noButton.interactable = true;
            var mousePos = Input.mousePosition;
            var objectPos = cam.ScreenToWorldPoint(mousePos);
            Spawn(objectPos);
        }

        public void ConfirmPoint()
        {
            pointButton.interactable = true;
            noButton.interactable = false;

            activePoint.GetComponent<DraggableObject>().enabled = false;
            activePoint.GetComponent<CollidingObject>().Disable();

            activePoint = null;

        }

        public void RemoveActivePoint()
        {
            Destroy(activePoint);
            pointButton.interactable = true;
            noButton.interactable = false;
        }
	}
}