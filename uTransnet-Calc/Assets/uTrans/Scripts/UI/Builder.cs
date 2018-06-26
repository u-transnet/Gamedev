using Mapbox.Unity.Map;
using UnityEngine;
using UnityEngine.UI;
using uTrans.Calc;
using uTrans.Components;

namespace uTrans.UI
{
    public class Builder : MonoBehaviour
    {
        [SerializeField]
        public Button yesButton;

        [SerializeField]
        public Button noButton;

        [SerializeField]
        public Button doneButton;

        [SerializeField]
        public SpawnOnMapD spawnManager;

        #pragma region building
        //region building
        [SerializeField]
        public GameObject builderCanvas;

        [SerializeField]
        public Button pointLight;

        [SerializeField]
        public Button pointHeavy;

        [SerializeField]
        public Button cancelBuildingButton;
        //endregion
        #pragma endregion

        [SerializeField]
        AbstractMap map;


        private PointType currentPointType = PointType.Unset;
        private Vector3 currentPosition;
        private bool newPointCanBeSpawned = true;

        void Start()
        {
            yesButton.GetComponent<Button>().onClick.AddListener(spawnManager.ConfirmPoint);
            noButton.GetComponent<Button>().onClick.AddListener(spawnManager.RemoveActivePoint);
            doneButton.GetComponent<Button>().onClick.AddListener(spawnManager.FinishLine);

            pointLight.GetComponent<Button>().onClick.AddListener(() =>
            {
                currentPointType = PointType.AnchorPylon;
                BuildPoint();
            });
            pointHeavy.GetComponent<Button>().onClick.AddListener(() => {
                currentPointType = PointType.Terminal;
                BuildPoint();
            });
            cancelBuildingButton.GetComponent<Button>().onClick.AddListener(() => {
                spawnManager.RemoveActivePoint();
                builderCanvas.SetActive(false);
            });

        }

        private void BuildPoint()
        {
            spawnManager.ActivePoint.GetComponent<BasePoint>().PointType = currentPointType;
            builderCanvas.SetActive(false);
            spawnManager.ConfirmPoint();
        }

        private void Update()
        {

            GameObject activePointGO = spawnManager.ActivePoint;
            if (activePointGO != null)
            {
                BasePoint activePoint = activePointGO.GetComponent<BasePoint>();
                GameObject prevPoint = spawnManager.PrevPoint;
                if (prevPoint != null)
                {
                    /*float radius = prevPoint.GetComponentInChildren<AreaRenderer>().GetDistance();
                    float distance = Vector3.Distance(prevPoint.transform.position, activePoint.transform.position);
                    if (distance <= radius)
                    {*/
                        GameObject activeLink = spawnManager.ActiveLink;
                        bool collidingLink = (activeLink != null) ? activeLink.GetComponentInChildren<CollidingObject>().Colliding : false;
                        yesButton.interactable = !activePoint.collidingObject.Colliding && !collidingLink;
                    /*}
                    else
                    {
                        yesButton.interactable = false;
                    }*/
                }
                else
                {
                    yesButton.interactable = !activePoint.collidingObject.Colliding;
                }

                noButton.interactable = true;
                newPointCanBeSpawned = false;
                doneButton.interactable = false;

                if(Input.GetKeyUp(KeyCode.Plus) || Input.GetKeyUp(KeyCode.KeypadPlus))
                {
                    activePoint.Up();
                }
                else if(Input.GetKeyUp(KeyCode.Minus) || Input.GetKeyUp(KeyCode.KeypadMinus))
                {
                    activePoint.Down();
                }
            }
            else
            {
                doneButton.interactable = spawnManager.ProjectsEditor.ActiveProject != null;
                newPointCanBeSpawned = true;
                yesButton.interactable = false;
                noButton.interactable = false;
            }
        }

        public void OnLongPress(GameObject go, Vector3 position)
        {
            if (go == gameObject)
            {
                currentPosition = Camera.main.ScreenToWorldPoint(position);
                builderCanvas.SetActive(true);
                builderCanvas.GetComponent<OnMapObject>().NewPos(currentPosition);

                spawnManager.SpawnPointWithLink(PointType.Terminal, currentPosition);
                BasePoint point = spawnManager.ActivePoint.GetComponent<BasePoint>();
                point.Active = false;
            }
        }

        public void OnClick(GameObject go, Vector3 position)
        {
            if (go == gameObject)
            {
                if(currentPointType != PointType.Unset)
                {
                    currentPosition = Camera.main.ScreenToWorldPoint(position);
                    spawnManager.SpawnPointWithLink(currentPointType, currentPosition);
                    spawnManager.ConfirmPoint();
                }
            }
        }
    }
}