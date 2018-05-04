using UnityEngine;
using System.Collections.Generic;

public class Project : ScriptableObject
{
    List<GameObject> points = new List<GameObject>();
    List<GameObject> links = new List<GameObject>();

    private bool _active;
    public bool Active
    {
        get { return _active; }
        set
        {
            _active = value;
            foreach (GameObject point in points)
            {
                point.GetComponent<BaseObject>().Editable = _active;
            }
            foreach (GameObject link in links)
            {
                link.GetComponent<BaseObject>().Editable = _active;
            }
        }
    }

    private BaseObject _activePoint;
    public BaseObject ActivePoint
    {
        get
        {
            return _activePoint;
        }
        set
        {
            if (_activePoint != null)
            {
                _activePoint.Active = false;
            }
            _activePoint = value;
            if (_activePoint != null)
            {
                _activePoint.Active = true;
            }
        }
    }



    public void AddPoint(GameObject point)
    {
        point.GetComponent<BaseObject>().project = this;
        points.Add(point);
    }

    public void AddLink(GameObject link)
    {
        SwitchAlfa(link.transform.GetChild(0).gameObject, _active ? 0.8f : 1);
        links.Add(link);
    }

    public void Delete()
    {
        foreach (GameObject link in links)
        {
            Destroy(link);
        }
        foreach (GameObject point in points)
        {
            Destroy(point);
        }
        links = new List<GameObject>();
        points = new List<GameObject>();
    }

    public bool Contains(GameObject go)
    {
        return points.Contains(go);
    }

    private void SwitchAlfa(GameObject go, float alfa)
    {
        var NodeRenderer = go.GetComponent<Renderer>();
        Color oldColor = NodeRenderer.material.color;
        NodeRenderer.material.color = new Color(oldColor.r, oldColor.g, oldColor.b, alfa);
    }

    public void RemovePoint(GameObject point)
    {
        points.Remove(point);
        foreach(var link in FindLinks(point))
        {
            links.Remove(link);
            Destroy(link);
        }
        Destroy(point);
    }

    private List<GameObject> FindLinks(GameObject point)
    {
        var list = new List<GameObject>();
        foreach (GameObject link in links)
        {
            var tmp = link.GetComponent<StretchyTethered>();
            if(point.transform == tmp.targetObj[0] || point.transform == tmp.targetObj[1])
            {
                list.Add(link);

            }
        }

        return list;
    }
}