using UnityEngine;
using System.Collections;
using Mapbox.Utils;
using Mapbox.Unity.Map;

public class CollidingObject : MonoBehaviour
{
    public Collider NodeCollider { get; private set; }
    public Renderer NodeRenderer { get; private set; }

    [SerializeField]
    public bool colliding = false;

    private OnMapObject uObject;

    [SerializeField]
    Color defaultColor;
   

    // Use this for initialization
    void Start()
    {
        uObject = GetComponent<OnMapObject>();
        NodeCollider = GetComponent<Collider>();
        NodeRenderer = GetComponent<Renderer>();
        defaultColor = NodeRenderer.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (colliding)
        {
            NodeRenderer.material.color = new Color(0.8f, 0, 0, 0.3f);
        }
        else
        {
            NodeRenderer.material.color = new Color(0, 0.8f, 0, 0.3f);
        }
    }

    public void Disable()
    {
        NodeRenderer.material.color = defaultColor;
        enabled = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "building" || collision.gameObject.tag == "structure")
        {
            colliding = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "building" || collision.gameObject.tag == "structure")
        {
            colliding = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        colliding = false;
    }


}
