using UnityEngine;
using System.Collections;
using Mapbox.Utils;
using Mapbox.Unity.Map;

public class OnMapObject : MonoBehaviour
{

    [SerializeField]
    public float spawnScale = 10f;

    public AbstractMap Map { get; set; }

    private Vector2d location;
    

    public void NewPos(Vector3 pos)
    {
        location = Map.WorldToGeoPosition(pos);
        transform.localPosition = Map.GeoToWorldPosition(location, true);
        var curScale = spawnScale * Map.transform.localScale.x;
        transform.localScale = new Vector3(curScale, curScale, curScale);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        transform.localPosition = Map.GeoToWorldPosition(location, true);
        var curScale = spawnScale * Map.transform.localScale.x;
        transform.localScale = new Vector3(curScale, curScale, curScale);
    }


}
