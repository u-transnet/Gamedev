using Mapbox.Unity.Utilities;

namespace Mapbox.Examples
{
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
                    doneButton.interactable = true;
                }
                else
                {
                    doneButton.interactable = false;
                }
            }
        }

        private GameObject activePoint = null;
        private GameObject activeLink = null;

        public bool PointerUsed { get; set; }

        public ProjectsEditor projectsEditor = new ProjectsEditor();

        void Start()
        {
            pointButton.GetComponent<Button>().onClick.AddListener(SpawnOnCenter);
            yesButton.GetComponent<Button>().onClick.AddListener(ConfirmPoint);
            noButton.GetComponent<Button>().onClick.AddListener(RemoveActivePoint);
            doneButton.GetComponent<Button>().onClick.AddListener(FinishLine);

            PointerUsed = false;
        }

        private void Update()
        {

            if (activePoint != null)
            {
                if (PrevPoint != null)
                {
                    float radius = PrevPoint.GetComponentInChildren<AreaRenderer>().GetDistance();
                    float distance = Vector3.Distance(PrevPoint.transform.position, activePoint.transform.position);
                    //Debug.Log("Radius: " + radius + "   Distance: " + distance);
                    if (distance <= radius)
                    {
                        bool collidingLink = (activeLink != null) ? activeLink.GetComponentInChildren<CollidingObject>().colliding : false;
                        yesButton.interactable = !activePoint.GetComponent<CollidingObject>().colliding && !collidingLink;
                    }
                    else
                    {
                        yesButton.interactable = false;
                    }
                }
                else
                {
                    yesButton.interactable = !activePoint.GetComponent<CollidingObject>().colliding;
                }
            }
            else
            {
                yesButton.interactable = false;
            }

            //Debug.Log(Input.mousePosition);
        }

        private GameObject Spawn(Vector3 pos)
        {
            activePoint = Instantiate(_markerPrefab);
            var node = activePoint.GetComponent<OnMapObject>();
            node.Map = _map;
            node.NewPos(pos);
            node.GetComponent<DraggableObject>().BuildingManager = this;
            return activePoint;
        }

        public void SpawnOnCenter()
        {
            if (projectsEditor.ActiveProject == null)
            {
                projectsEditor.NewProject();
            }
            pointButton.interactable = false;
            noButton.interactable = true;
            var mousePos = Input.mousePosition;

            Camera cam = Camera.main;
            float height = 2f * cam.orthographicSize;
            float width = height * cam.aspect;

            var objectPos = cam.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            projectsEditor.ActiveProject.AddPoint(Spawn(objectPos));
            if (activePoint != null && PrevPoint != null)
            {
                projectsEditor.ActiveProject.AddLink(CreateLink());
            }
        }

        public void ConfirmPoint()
        {
            pointButton.interactable = true;
            noButton.interactable = false;

            activePoint.GetComponent<BaseObject>().Active = false;

            PrevPoint = activePoint;

            if (activeLink != null)
            {
                activeLink.GetComponent<BaseObject>().Active = false;
                activeLink = null;
            }
            activePoint = null;

        }

        public void RemoveActivePoint()
        {
            projectsEditor.ActiveProject.RemovePoint(activePoint);
            activePoint = null;
            activeLink = null;
            pointButton.interactable = true;
            noButton.interactable = false;
        }

        public void FinishLine()
        {
            PrevPoint = null;
            if (activePoint != null)
            {
                RemoveActivePoint();
            }
            projectsEditor.FinishProject();
        }

        private GameObject CreateLink()
        {
            activeLink = Instantiate(_linkPrefab);
            activeLink.transform.parent = activePoint.transform;
            activeLink.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            var strechy = activeLink.GetComponent<StretchyTethered>();
            strechy.targetObj[0] = PrevPoint.transform;
            strechy.targetObj[1] = activePoint.transform;

            return activeLink;
        }

        public void OnLongPress(GameObject go)
        {
            Debug.Log("Pressed " + go.name);
            if (projectsEditor.ActiveProject == null)
            {
                projectsEditor.FindActiveProject(go);
                doneButton.interactable = true;
            }
        }

        public void OnClick(GameObject go)
        {
            Debug.Log("Clicked " + go.name);
            if (projectsEditor.ActiveProject != null)
            {
                if (projectsEditor.ActiveProject.Contains(go))
                {
                    projectsEditor.ActiveProject.ActivePoint = go.GetComponent<BaseObject>();

                    pointButton.interactable = false;
                    doneButton.interactable = false;
                    activePoint = go;
                }
            }
        }
    }
}