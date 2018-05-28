namespace uTrans
{
    using System;
    using uTrans.Components;
    using uTrans.Network;
    using Mapbox.Unity.Map;
    using UnityEngine;
    using UnityEngine.UI;

    public class SpawnOnMapD : MonoBehaviour
    {
        [SerializeField]
        AbstractMap _map;

        [SerializeField]
        GameObject _markerPrefab;

        [SerializeField]
        GameObject _linkPrefab;

        [SerializeField]
        Camera cam;

        [SerializeField]
        public Button pointButton;

        [SerializeField]
        public Button yesButton;

        [SerializeField]
        public Button noButton;

        [SerializeField]
        public Button doneButton;

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
                if (_activePoint != null)
                {
                    //                    _activePoint.GetComponent<OnMapObject>().OnLocationUpdate -= DrawPointDebug;
                }
                _activePoint = value;
                if (projectsEditor.ActiveProject != null)
                {
                    projectsEditor.ActiveProject.ActivePoint =
                    (_activePoint != null) ? _activePoint.GetComponent<BasePoint>() : null;
                }
                if (_activePoint != null)
                {
                    //                    _activePoint.GetComponent<OnMapObject>().OnLocationUpdate += DrawPointDebug;
                }
                else
                {
                    debugText.text = "";
                }
            }
        }

        private GameObject _activeLink;

        private GameObject ActiveLink
        {
            get
            {
                return _activeLink;
            }
            set
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

        public ProjectsEditor projectsEditor = new ProjectsEditor();

        void Start()
        {
            pointButton.GetComponent<Button>().onClick.AddListener(SpawnOnCenter);
            yesButton.GetComponent<Button>().onClick.AddListener(ConfirmPoint);
            noButton.GetComponent<Button>().onClick.AddListener(RemoveActivePoint);
            doneButton.GetComponent<Button>().onClick.AddListener(FinishLine);

            projectsEditor.OnProjectSwitch += (oldProject, newProject) =>
            {
                DrawDebug(newProject);
            };
            PointerUsed = false;
            ServerCommunication.SendName("Testing");
        }

        private void Update()
        {

            if (ActivePoint != null)
            {
                if (PrevPoint != null)
                {
                    float radius = PrevPoint.GetComponentInChildren<AreaRenderer>().GetDistance();
                    float distance = Vector3.Distance(PrevPoint.transform.position, ActivePoint.transform.position);
                    //Debug.Log("Radius: " + radius + "   Distance: " + distance);
                    if (distance <= radius)
                    {
                        bool collidingLink = (ActiveLink != null) ? ActiveLink.GetComponentInChildren<CollidingObject>().Colliding : false;
                        yesButton.interactable = !ActivePoint.GetComponent<CollidingObject>().Colliding && !collidingLink;
                    }
                    else
                    {
                        yesButton.interactable = false;
                    }
                }
                else
                {
                    yesButton.interactable = !ActivePoint.GetComponent<CollidingObject>().Colliding;
                }

                noButton.interactable = true;
                pointButton.interactable = false;
                doneButton.interactable = false;
            }
            else
            {
                doneButton.interactable = projectsEditor.ActiveProject != null;
                pointButton.interactable = true;
                yesButton.interactable = false;
                noButton.interactable = false;
            }

            //Debug.Log(Input.mousePosition);
        }

        public GameObject Spawn(Vector3 pos, int id = -1)
        {
            if (projectsEditor.ActiveProject == null)
            {
                projectsEditor.NewProject();
            }

            ActivePoint = Instantiate(_markerPrefab);
            var node = ActivePoint.GetComponent<OnMapObject>();
            node.Map = _map;
            node.NewPos(pos);
            node.GetComponent<DraggableObject>().BuildingManager = this;
            projectsEditor.ActiveProject.AddPoint(ActivePoint, id);
            return ActivePoint;
        }

        public void SpawnOnCenter()
        {

            Camera cam = Camera.main;
            float height = 2f * cam.orthographicSize;
            float width = height * cam.aspect;

            var objectPos = cam.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            Spawn(objectPos);
            if (ActivePoint != null && PrevPoint != null)
            {
                CreateLink();
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
            var points = projectsEditor.ActiveProject.RemovePoint(ActivePoint);
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
            projectsEditor.FinishProject();
        }

        private void CreateLink()
        {
            CreateLink(ActivePoint, PrevPoint);
        }

        private void CreateLink(GameObject g1, GameObject g2)
        {
            ActiveLink = SpawnLink(g1, g2);
            projectsEditor.ActiveProject.AddLink(ActiveLink);
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
            if (projectsEditor.ActiveProject != null)
            {
                var centerCoordinates = projectsEditor.ActiveProject.GetCenterCoordinates();
                _map.SetCenterLatitudeLongitude(centerCoordinates);
            }
        }

        public void OnLongPress(GameObject go)
        {
            Debug.Log("Pressed " + go.name);
            if (projectsEditor.ActiveProject == null)
            {
                projectsEditor.FindActiveProject(go);
                //                doneButton.interactable = true;
            }
        }

        public void OnClick(GameObject go)
        {
            Debug.Log("Clicked " + go.name);
            if (projectsEditor.ActiveProject != null)
            {
                if (projectsEditor.ActiveProject.Contains(go))
                {
                    //                    pointButton.interactable = false;
                    //                    doneButton.interactable = false;
                    ActivePoint = go;
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
