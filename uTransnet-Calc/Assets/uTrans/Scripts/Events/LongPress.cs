﻿using UnityEngine;
using UnityEngine.EventSystems;

public class LongPress : MonoBehaviour
{
    [SerializeField]
    public float pressTime = 1;
    float timer = 0;
    float touchTime;


    [SerializeField]
    private bool worldWide = false;

    private Vector3 currentMouseDown;
    private Vector2 currentTouchDown;
    private bool pressed = false;

    LongPressEventManager eventManager;

    // Use this for initialization
    void Start()
    {
        eventManager = FindObjectOfType<LongPressEventManager>();
    }


    void OnMouseOver()
    {
        if (!worldWide)
        {
            CheckInput();
        }
    }

    void OnMouseEnter()
    {
        if (!worldWide)
        {
            eventManager.HoveredObjects++;
        }
    }

    void OnMouseExit()
    {
        if (!worldWide)
        {
            eventManager.HoveredObjects--;
        }
    }

    void Update()
    {
        if (worldWide && !eventManager.OverObject)
        {
            CheckInput();
        }
    }

    void CheckInput()
    {
        // Check mouse click on UI
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            // Increment the timer
            if (Input.GetMouseButton(0))
            {
                // Save position of click down
                if (!pressed)
                {
                    currentMouseDown = Input.mousePosition;
                    pressed = true;
                }
                timer += Time.deltaTime;
            }
            else if (pressed)
            {
                if (currentMouseDown == Input.mousePosition)
                {
                    if (timer > pressTime)
                    {
                        // Don't know why coords changed
                        OnLongPress(Input.mousePosition);
                    }
                    else if (timer > 0)
                    {
                        OnClick(Input.mousePosition);
                    }
                }
                timer = 0;
                pressed = false;
            }
        }

        if (Input.touches.Length > 0)
        {
            Touch touch = Input.touches[0];
            // CHeck touch on UI
            if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                if (touch.phase == TouchPhase.Began)
                {
                    touchTime = Time.time;
                    currentTouchDown = touch.position;
                }

                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    if (Vector2.Distance(currentTouchDown, touch.position) <= 10)
                    {
                        if (Time.time - touchTime <= pressTime)
                        {
                            OnClick(new Vector3(touch.position.x, touch.position.y, 0));
                        }
                        else
                        {
                            OnLongPress(new Vector3(touch.position.x, touch.position.y, 0));
                        }
                    }
                }
            }
        }

    }

    protected void OnLongPress(Vector3 position)
    {
        if (!worldWide)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
        eventManager.LongPress(gameObject, position);
    }

    protected void OnClick(Vector3 position)
    {
        if (!worldWide)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
        eventManager.Click(gameObject, position);
    }
}
