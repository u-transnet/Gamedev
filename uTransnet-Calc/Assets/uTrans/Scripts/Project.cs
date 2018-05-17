using System.Collections.Generic;
using Mapbox.CheapRulerCs;
using Mapbox.Unity.Location;
using Mapbox.Utils;
using UnityEngine;

using uTrans.Components;
using uTrans.Data;
using uTrans.Services;

namespace uTrans
{

    public class Project : ScriptableObject
    {
        List<GameObject> points = new List<GameObject>();
        List<GameObject> links = new List<GameObject>();

        public ProjectDTO ProjectDTO { get; set; }

        private bool _active;

        /// <summary>
        /// If this project selected
        /// </summary>
        public bool Active
        {
            get
            {
                return _active;
            }
            set
            {
                _active = value;
                foreach (GameObject point in points)
                {
                    point.GetComponent<BaseObject>().Editable = _active;
                    if (!_active) // Deselecting object
                    {
                        var componentId = point.GetComponent<Id>();
                        var onMapObject = point.GetComponent<OnMapObject>();
                        if (componentId.Value < 0) // Not yet saved point
                        {
                            componentId.Value = DataService.instance.PointDAO.New(
                                    ProjectDTO.Id, onMapObject.Location.x, onMapObject.Location.y
                            ).Id;
                        }
                        else
                        {
                            DataService.instance.PointDAO.UpdateLocation(
                                    componentId.Value, onMapObject.Location.x, onMapObject.Location.y
                            );
                        }

                    }
                }
                foreach (GameObject link in links)
                {
                    link.GetComponent<BaseObject>().Editable = _active;
                    if (!_active) // Deselecting object
                    {
                        var componentId = link.GetComponent<Id>();
                        var stretchy = link.GetComponent<StretchyTethered>();
                        if (componentId.Value < 0) // Not yet saved link
                        {
                            componentId.Value = DataService.instance.LinkDAO.New(
                                    ProjectDTO.Id,
                                    stretchy.targetObj[0].GetComponent<Id>().Value,
                                    stretchy.targetObj[1].GetComponent<Id>().Value
                            ).Id;
                        }
                    }
                }
            }
        }

        private BaseObject _activePoint;

        /// <summary>
        /// Currently selected point for movement
        /// </summary>
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

        /// <summary>
        /// Find coordinates of active point or middle point of project if there is no active point
        /// </summary>
        /// <returns>Geographical coordinates</returns>
        public Vector2d GetCenterCoordinates()
        {
            if (ActivePoint != null)
            {
                return ActivePoint.GetComponent<OnMapObject>().Location;
            }
            else
            {
                return GetProjectCenter();
            }
        }

        /// <summary>
        /// Find coordinates of middle point of project
        /// </summary>
        /// <returns>Geographical coordinates</returns>
        public Vector2d GetProjectCenter()
        {
            double maxY = double.MinValue;
            double maxX = double.MinValue;
            double minX = double.MaxValue;
            double minY = double.MaxValue;

            foreach (var point in points)
            {
                var location = point.GetComponent<OnMapObject>().Location;
                if (location.x < minX)
                {
                    minX = location.x;
                }
                if (location.x > maxX)
                {
                    maxX = location.x;
                }
                if (location.y < minY)
                {
                    minY = location.y;
                }
                if (location.y > maxY)
                {
                    maxY = location.y;
                }
            }

            return new Vector2d((maxX + minX) / 2, (maxY + minY) / 2);
        }


        /// <summary>
        /// Add point to project and save it into db
        /// </summary>
        /// <param name="point"></param>
        /// <param name="id">-1 for new point</param>
        public void AddPoint(GameObject point, int id)
        {
            point.GetComponent<BaseObject>().project = this;
            points.Add(point);
            if (id < 0)
            {
                var onMapObject = point.GetComponent<OnMapObject>();
                var pointDTO = DataService.instance.PointDAO.New(ProjectDTO.Id, onMapObject.Location.x, onMapObject.Location.y);
                id = pointDTO.Id;
            }
            var componentId = point.GetComponent<Id>();
            componentId.Value = id;
        }

        public void AddLink(GameObject link)
        {
            SwitchAlfa(link.transform.GetChild(0).gameObject, _active ? 0.8f : 1);
            links.Add(link);
        }


        /// <summary>
        /// Delete project and linked points with links
        /// </summary>
        public void Delete()
        {
            foreach (GameObject link in links)
            {
                DataService.instance.LinkDAO.Delete(link.GetComponent<Id>().Value);
                Destroy(link);
            }
            foreach (GameObject point in points)
            {
                DataService.instance.PointDAO.Delete(point.GetComponent<Id>().Value);
                Destroy(point);
            }
            links = new List<GameObject>();
            points = new List<GameObject>();
            DataService.instance.ProjectDAO.Delete(ProjectDTO);
        }


        /// <summary>
        /// Check if point exist in this project
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public bool Contains(GameObject go)
        {
            return points.Contains(go);
        }

        private void SwitchAlfa(GameObject go, float alfa)
        {
            var nodeRenderer = go.GetComponent<Renderer>();
            Color oldColor = nodeRenderer.material.color;
            nodeRenderer.material.color = new Color(oldColor.r, oldColor.g, oldColor.b, alfa);
        }


        /// <summary>
        /// Removes point and according links from scene and DB
        /// </summary>
        /// <param name="point"></param>
        /// <returns> Points that were linked with deleted point</returns>
        public List<GameObject> RemovePoint(GameObject point)
        {
            var neighbours = new List<GameObject>();
            points.Remove(point);
            foreach (var link in FindLinks(point))
            {
                var targetObjects = link.GetComponent<StretchyTethered>().targetObj;
                if(targetObjects[0] == point.transform)
                {
                    neighbours.Add(targetObjects[1].gameObject);
                }
                else if(targetObjects[1] == point.transform)
                {
                    neighbours.Add(targetObjects[0].gameObject);
                }
                RemoveLink(link);
            }
            var id = point.GetComponent<Id>().Value;
            if(id > 0)
            {
                DataService.instance.PointDAO.Delete(id);
            }
            Destroy(point);
            return neighbours;
        }

        private void RemoveLink(GameObject link)
        {
            var id = link.GetComponent<Id>().Value;
            if(id > 0)
            {
                DataService.instance.LinkDAO.Delete(id);
            }
            links.Remove(link);
            Destroy(link);
        }


        /// <summary>
        /// Finds point in project
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Point or null if not found</returns>
        public GameObject FindPointById(int id)
        {
            foreach (GameObject point in points)
            {
                if (point.GetComponent<Id>().Value == id)
                {
                    return point;
                }
            }

            return null;
        }

        private List<GameObject> FindLinks(GameObject point)
        {
            var list = new List<GameObject>();
            foreach (GameObject link in links)
            {
                var tmp = link.GetComponent<StretchyTethered>();
                if (point.transform == tmp.targetObj[0] || point.transform == tmp.targetObj[1])
                {
                    list.Add(link);

                }
            }

            return list;
        }


        /// <summary>
        /// Calculates total lemgth of project
        /// </summary>
        /// <returns>Sum of links length</returns>
        public double GetTotalLength()
        {
            double length = 0;
            var map = LocationProviderFactory.Instance.mapManager;
            var cameraGeo = map.WorldToGeoPosition(Camera.main.transform.position);
            var ruler = new CheapRuler(cameraGeo.y, CheapRulerUnits.Meters);

            foreach (var link in links)
            {
                var tmp = link.GetComponent<StretchyTethered>();
                var point1 = map.WorldToGeoPosition(tmp.targetObj[0].transform.position);
                var point2 = map.WorldToGeoPosition(tmp.targetObj[1].transform.position);
                length += ruler.Distance(point1.ToArray(), point2.ToArray());
            }
            return length;
        }
    }
}