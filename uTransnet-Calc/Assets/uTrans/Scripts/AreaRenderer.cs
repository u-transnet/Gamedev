using uTrans.Components;

namespace uTrans
{
    using UnityEngine;

    public class AreaRenderer : MonoBehaviour
    {

        [SerializeField]
        float distance = 1f;

        GameObject parent;
        OnMapObject onMapObject;

        // Use this for initialization
        void Start()
        {
            var oldColor = GetComponent<Renderer>().material.color;
            GetComponent<Renderer>().material.color = new Color(oldColor.r, oldColor.g, oldColor.b, 0.2f);
            parent = transform.parent.gameObject;
            onMapObject = parent.GetComponent<OnMapObject>();
            transform.localScale = new Vector3(distance, distance / 10, distance);
        }

        public float GetDistance()
        {
            return parent.transform.localScale.x * distance / 2;
        }


    }
}