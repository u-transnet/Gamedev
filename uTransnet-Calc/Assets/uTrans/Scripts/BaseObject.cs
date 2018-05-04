using UnityEngine;
using System.Collections;

public class BaseObject : MonoBehaviour
{
    [SerializeField]
    OnMapObject onMapObject;
    [SerializeField]
    CollidingObject collidingObject;
    [SerializeField]
    DraggableObject draggableObject;
    [SerializeField]
    Renderer objectRenderer;

    [SerializeField]
    float alfa = 0.5f;

    [SerializeField]
    bool canBeSelected = true;

    public Project project;

    private bool _editable;
    [SerializeField]
    public bool Editable
    {
        get
        {
            return _editable;
        }

        set
        {
            _editable = value;
            Color oldColor = objectRenderer.material.color;
            objectRenderer.material.color = new Color(oldColor.r, oldColor.g, oldColor.b, _editable ? alfa : 1);
        }
    }

    private bool _active;
    [SerializeField]
    public bool Active
    {
        get
        {
            return _active;
        }
        set
        {
            _active = value;
            if (draggableObject != null)
            {
                draggableObject.enabled = value;
            }
            if (collidingObject != null)
            {
                if (!value)
                {
                    collidingObject.Disable();
                }
                else
                {
                    collidingObject.enabled = value;
                }
            }
        }
    }

    private bool clicking = false;

    // Use this for initialization
    void Start()
    {
        //draggableObject = GetComponent<DraggableObject>();
        //collidingObject = GetComponent<CollidingObject>();
        //onMapObject = GetComponent<OnMapObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
