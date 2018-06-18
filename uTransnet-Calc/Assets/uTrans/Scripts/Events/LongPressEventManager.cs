using System;
using UnityEngine;
using UnityEngine.Events;

public class LongPressEventManager : MonoBehaviour
{

    public bool OverObject { get { return HoveredObjects > 0; } }

    public int HoveredObjects { get; set; }

    [Serializable]
    public class LongPressEvent : UnityEvent<GameObject, Vector3> { }

    public LongPressEvent longPress;

    public void AddLongPressListener(UnityAction<GameObject, Vector3> method)
    {
        longPress.AddListener(method);
    }

    public void RemoveLongPressListener(UnityAction<GameObject, Vector3> method)
    {
        longPress.RemoveListener(method);
    }

    public void LongPress(GameObject go, Vector3 position)
    {
        longPress.Invoke(go, position);
    }


    [Serializable]
    public class ClickEvent : UnityEvent<GameObject, Vector3> { }

    public ClickEvent click;

    public void AddClickListener(UnityAction<GameObject, Vector3> method)
    {
        click.AddListener(method);
    }

    public void RemoveClickListener(UnityAction<GameObject, Vector3> method)
    {
        click.RemoveListener(method);
    }

    public void Click(GameObject go, Vector3 position)
    {
        click.Invoke(go, position);
    }
}
