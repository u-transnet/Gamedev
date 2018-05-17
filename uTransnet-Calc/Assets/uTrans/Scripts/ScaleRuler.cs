using Mapbox.CheapRulerCs;
using Mapbox.Unity.Map;
using UnityEngine;
namespace uTrans
{
    public class ScaleRuler : MonoBehaviour
    {
        [SerializeField]
        GameObject bottom;

        [SerializeField]
        GameObject top;

        [SerializeField]
        TextMesh text;

        [SerializeField]
        AbstractMap _map;

        float scaleAtBeginning;

        void Start()
        {
            scaleAtBeginning = transform.localPosition.z;
        }


        // Update is called once per frame
        void Update()
        {
            var bottomGeo = _map.WorldToGeoPosition(bottom.transform.position);
            var topGeo = _map.WorldToGeoPosition(top.transform.position);
            var cameraGeo = _map.WorldToGeoPosition(Camera.main.transform.position);
            var ruler = new CheapRuler(cameraGeo.y, CheapRulerUnits.Meters);
            var distance = ruler.Distance(bottomGeo.ToArray(), topGeo.ToArray());
            text.text = ((int)distance).ToString();
        }
    }
}