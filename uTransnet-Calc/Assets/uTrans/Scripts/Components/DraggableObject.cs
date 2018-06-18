namespace uTrans.Components
{
    using System;
    using Mapbox.Examples;
    using Mapbox.Utils;
    using UnityEngine;

    public class DraggableObject : MonoBehaviour
    {


        [SerializeField]
        OnMapObject uObject;
        private bool dragging = false;
        private float distance;

        Vector3 gap = Vector3.zero;

        public SpawnOnMapD BuildingManager { get; set; }

        public Action<Vector3, Vector2d> OnDragFinished = (vector3, vector2D) =>
        {
        };


        // Update is called once per frame
        void Update()
        {

            if (dragging)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Vector3 rayPoint = ray.GetPoint(distance);
                uObject.NewPos(rayPoint - gap);
            }
        }


        void OnMouseDown()
        {
            // offset between clicked point and object center
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var objPos = uObject.transform.position;
            gap = new Vector3(mousePos.x - objPos.x, 0, mousePos.z - objPos.z);

            distance = Vector3.Distance(transform.position, Camera.main.transform.position);
            dragging = true;
            BuildingManager.PointerUsed = true;
        }

        void OnMouseUp()
        {
            dragging = false;
            BuildingManager.PointerUsed = false;
            OnDragFinished(transform.position, uObject.Location);
        }

    }
}
