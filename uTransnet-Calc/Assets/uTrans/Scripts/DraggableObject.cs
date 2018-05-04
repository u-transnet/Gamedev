using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using Mapbox.Examples;

public class DraggableObject : MonoBehaviour {


    private OnMapObject uObject;
    private bool dragging = false;
    private float distance;

   

    public SpawnOnMapD BuildingManager { get; set; }



    // Use this for initialization
    void Start ()
    {
        uObject = GetComponent<OnMapObject>();
    }
	
	// Update is called once per frame
	void Update () {

        if (dragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(distance);
            uObject.NewPos(rayPoint);
        }
    }


    void OnMouseDown()
    {
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        dragging = true;
        BuildingManager.PointerUsed = true;
    }

    void OnMouseUp()
    {
        dragging = false;
        BuildingManager.PointerUsed = false;
    }

}
