using UnityEngine;

public class LongPress : MonoBehaviour
{
    [SerializeField]
    public float pressTime = 1;
    float timer = 0;
    float touchTime;

    private Vector3 currentMouseDown;
    private Vector2 currentTouchDown;
    private bool pressed = false;

    LongPressEventManager eventManager;

    // Use this for initialization
    void Start()
    {
        eventManager = FindObjectOfType<LongPressEventManager>();
    }

    // Update is called once per frame
    void OnMouseOver()
    {
        // Increment the timer
        if (Input.GetMouseButton(0))
        {
            if (!pressed)
            {
                currentMouseDown = Input.mousePosition;
                pressed = true;
            }
            timer += Time.deltaTime;
        }
        else if(pressed)
        {
            if (currentMouseDown != null && currentMouseDown == Input.mousePosition)
            {
                if (timer > pressTime)
                {
                    // this is a long press
                    OnLongPress();
                }
                else if (timer > 0)
                {
                    OnClick();
                }
            }
            timer = 0;
            pressed = false;
        }

        if (Input.touches.Length > 0)
        {
            Touch touch = Input.touches[0];

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
                        //do stuff as a tap​
                        OnClick();
                    }
                    else
                    {
                        // this is a long press
                        OnLongPress();
                    }
                }
            }
        }
    }

    protected void OnLongPress()
    {
        eventManager.LongPress(gameObject);
    }

    protected void OnClick()
    {
        eventManager.Click(gameObject);
    }
}
