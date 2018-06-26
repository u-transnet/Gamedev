using Mapbox.CheapRulerCs;
using Mapbox.Utils;
using uTrans.Calc;

namespace uTrans
{
    using System;
    using Mapbox.Unity.Map;
    using UnityEngine;
    using UnityEngine.UI;
    using uTrans.Components;

    public class SpawnOnMapD : MonoBehaviour
    {
        [SerializeField]
        AbstractMap _map;

        [SerializeField]
        GameObject _terminalPrefab;

        [SerializeField]
        GameObject _pylonPrefab;

        [SerializeField]
        GameObject _linkPrefab;

        [SerializeField]
        Camera cam;


        [SerializeField]
        int minLinkDistance = 150;
        [SerializeField]
        int maxLinkDistance = 250;

        [SerializeField]
        public Text debugText;

        private GameObject _prevPoint;

        public GameObject PrevPoint
        {
            get
            {
                return _prevPoint;
            }
            set
            {
                if (_prevPoint != null)
                {
                    _prevPoint.transform.GetChild(0).gameObject.SetActive(false);
                }
                _prevPoint = value;
                if (_prevPoint != null)
                {
                    _prevPoint.transform.GetChild(0).gameObject.SetActive(true);
                }
            }
        }

        private GameObject _activePoint = null;

        public GameObject ActivePoint
        {
            get
            {
                return _activePoint;
            }
            set
            {
                _activePoint = value;
                if (ProjectsEditor.ActiveProject != null)
                {
                    ProjectsEditor.ActiveProject.ActivePoint =
                    (_activePoint != null) ? _activePoint.GetComponent<BasePoint>() : null;
                }
                /*if (_activePoint == null)
                {
                    DrawDebug(null);
                }*/
            }
        }

        private GameObject _activeLink;

        public GameObject ActiveLink
        {
            get
            {
                return _activeLink;
            }
            private set
            {
                if (_activeLink != null)
                {
                    _activeLink.GetComponent<BaseLink>().Active = false;
                }
                _activeLink = value;
                if (_activeLink != null)
                {
                    _activeLink.GetComponent<BaseLink>().Active = true;
                }
            }
        }

        public bool PointerUsed { get; set; }

        public ProjectsEditor _projectsEditor = new ProjectsEditor();

        public ProjectsEditor ProjectsEditor
        {
            get
            {
                return _projectsEditor;
            }
        }

        void Start()
        {

            ProjectsEditor.OnProjectSwitch += (oldProject, newProject) =>
            {
                if(oldProject != null)
                {
                    oldProject.OnProjectChanged -= DrawDebug;
                }

                if(newProject != null)
                {
                    newProject.OnProjectChanged += DrawDebug;
                }
                DrawDebug(newProject);
            };
            PointerUsed = false;
        }

        public GameObject Spawn(PointType pointType, Vector3 pos, int id = -1)
        {
            if (ProjectsEditor.ActiveProject == null)
            {
                ProjectsEditor.NewProject();
            }

            GameObject curPoint;
            if(pointType == PointType.Terminal)
            {
                curPoint = Instantiate(_terminalPrefab);
            }
            else
            {
                curPoint = Instantiate(_pylonPrefab);
            }
            BasePoint basePoint = curPoint.GetComponent<BasePoint>();
            basePoint.SetMap(_map);
            basePoint.SetBuildingManager(this);
            basePoint.onMapObject.NewPos(pos);
            basePoint.PointType = pointType;
            ProjectsEditor.ActiveProject.AddPoint(curPoint, id);
            return curPoint;
        }

        public void SpawnOnCenter(PointType pointType)
        {

            Camera cam = Camera.main;
            float height = 2f * cam.orthographicSize;
            float width = height * cam.aspect;

            var objectPos = cam.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            SpawnPointWithLink(pointType, objectPos);

        }
        public void SpawnPointWithLink(PointType pointType, Vector3 pos)
        {
            ActivePoint = Spawn(pointType, pos);
            if (ActivePoint != null && PrevPoint != null)
            {
                CreateLink(false);
            }
        }

        public void ConfirmPoint()
        {
            ActivePoint.GetComponent<BasePoint>().Active = false;

            PrevPoint = ActivePoint;

            if (ActiveLink != null)
            {
                ActiveLink.GetComponent<BaseLink>().Active = false;
                ActiveLink = null;
            }
            ActivePoint = null;

        }

        public void RemoveActivePoint()
        {
            var points = ProjectsEditor.ActiveProject.RemovePoint(ActivePoint);
            if (points.Count == 2)
            {
                CreateLink(points[0], points[1]);
                ActivePoint = points[0];
            }
            else
            {
                ActivePoint = null;
            }
            ActiveLink = null;
        }

        public void FinishLine()
        {
            PrevPoint = null;
            if (ActivePoint != null)
            {
                RemoveActivePoint();
            }
            ProjectsEditor.FinishProject();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="direct"></param> true if we don't need automatic pylons between points
        private void CreateLink(bool direct)
        {
            if(!direct)
            {
                BasePoint firstPoint = ActivePoint.GetComponent<BasePoint>();
                BasePoint secondPoint = PrevPoint.GetComponent<BasePoint>();
                CheapRuler ruler = new CheapRuler(
                        firstPoint.onMapObject.Location.x,
                        CheapRulerUnits.Meters
                );

                double[] array1 = GeoUtils.LocationToLongLatArray(firstPoint.onMapObject.Location);
                double[] array2 = GeoUtils.LocationToLongLatArray(secondPoint.onMapObject.Location);

                double distance = ruler.Distance(array1, array2);
                double bearing = ruler.Bearing(array1, array2);
                int bestDistance = FindBestDistance(maxLinkDistance, minLinkDistance, distance, 10);
                int pylonsCount = (int) (distance / bestDistance);

                double[] pylonPosition = array1;
                GameObject prevPylon = ActivePoint;
                for (int i = 0; i < pylonsCount; i++)
                {
                    pylonPosition = ruler.Destination(pylonPosition, bestDistance, bearing);
                    GameObject newPylon = Spawn(
                            PointType.Pylon,
                            _map.GeoToWorldPositionXZ( GeoUtils.LongLatArrayToLocation(pylonPosition))
                    );
                    newPylon.GetComponent<BasePoint>().Active = false;
                    CreateLink(newPylon, prevPylon);
                    prevPylon = newPylon;
                }
                CreateLink(PrevPoint, prevPylon);
            }
            else
            {
                CreateLink(ActivePoint, PrevPoint);
            }
        }


        private int FindBestDistance(int maxDistance, int minDistance, double totalDistance, int steps)
        {
            int step = (maxDistance - minDistance) / steps;
            int highestReminder = 0;
            int bestDisteance = 0;
            for(int tmp = minDistance; tmp <= maxDistance; tmp += step)
            {
                int reminder = (int) totalDistance % tmp;
                if(reminder > highestReminder) {
                    highestReminder = reminder;
                    bestDisteance = tmp;
                }
            }

            return bestDisteance;
        }

        private void CreateLink(GameObject g1, GameObject g2)
        {
            ActiveLink = SpawnLink(g1, g2);
            ProjectsEditor.ActiveProject.AddLink(ActiveLink);
        }

        public GameObject SpawnLink(GameObject g1, GameObject g2)
        {
            var link = Instantiate(_linkPrefab);
            link.transform.parent = g1.transform;
            link.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            var strechy = link.GetComponent<BaseLink>().stretchy;
            strechy.FirstTarget = g2.transform;
            strechy.SecondTarget = g1.transform;

            return link;
        }

        public void CenterMap()
        {
            if (ProjectsEditor.ActiveProject != null)
            {
                var centerCoordinates = ProjectsEditor.ActiveProject.GetCenterCoordinates();
                _map.SetCenterLatitudeLongitude(centerCoordinates);
            }
        }

        public void OnLongPress(GameObject go, Vector3 position)
        {
            Transform parent = go.transform.parent;
            if (parent != null && parent.GetComponent<BasePoint>() != null)
            {
                Debug.Log("Pressed " + parent.name);
                if (ProjectsEditor.ActiveProject == null)
                {
                    ProjectsEditor.FindActiveProject(parent.gameObject);
                }
            }
        }

        public void OnClick(GameObject go, Vector3 position)
        {
            Transform parent = go.transform.parent;
            if (parent != null && parent.GetComponent<BasePoint>() != null)
            {
                Debug.Log("Clicked " + parent.name);
                if (ProjectsEditor.ActiveProject != null)
                {
                    if (ProjectsEditor.ActiveProject.Contains(parent.gameObject))
                    {
                        ActivePoint = parent.gameObject;
                    }
                }
            }
        }

        private void DrawDebug(Project project)
        {
            if (project != null)
            {
                debugText.text = String.Format(
                        "Length: {0:0}m\nPoints: {1}",
                        project.GetTotalLength(),
                        project.GetPointsCount()
                );
            }
            else
            {
                debugText.text = "";
            }

        }
    }
}
