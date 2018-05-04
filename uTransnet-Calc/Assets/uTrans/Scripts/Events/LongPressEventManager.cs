using System;
using UnityEngine;
using UnityEngine.Events;

public class LongPressEventManager : MonoBehaviour
{

    [Serializable]
    public class LongPressEvent : UnityEvent<GameObject> { }

    public LongPressEvent longPress;

    public void AddLongPressListener(UnityAction<GameObject> method)
    {
        longPress.AddListener(method);
    }

    public void RemoveLongPressListener(UnityAction<GameObject> method)
    {
        longPress.RemoveListener(method);
    }

    public void LongPress(GameObject go)
    {
        longPress.Invoke(go);
    }


    [Serializable]
    public class ClickEvent : UnityEvent<GameObject> { }

    public ClickEvent click;

    public void AddClickListener(UnityAction<GameObject> method)
    {
        click.AddListener(method);
    }

    public void RemoveClickListener(UnityAction<GameObject> method)
    {
        click.RemoveListener(method);
    }

    public void Click(GameObject go)
    {
        click.Invoke(go);
    }
}
