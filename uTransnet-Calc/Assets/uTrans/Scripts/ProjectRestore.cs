using uTrans.Components;
using uTrans.Services;

namespace uTrans
{
    using Mapbox.Unity.Map;
    using Mapbox.Utils;
    using UnityEngine;

    public class ProjectRestore : MonoBehaviour
    {
        [SerializeField]
        SpawnOnMapD spawnOnMapD;

        [SerializeField]
        AbstractMap map;

        void Awake()
        {
            map.OnInitialized += PlacePoints;
        }

        void PlacePoints()
        {
            foreach (var project in DataService.instance.ProjectDAO.All())
            {
                spawnOnMapD.projectsEditor.NewProject(project.Id);
                foreach (var point in DataService.instance.PointDAO.FindByProject(project))
                {
                    var worldPosition = map.GeoToWorldPosition(new Vector2d(point.X, point.Y));
                    spawnOnMapD.Spawn(worldPosition, point.Id);
                }

                foreach (var link in DataService.instance.LinkDAO.FindByProject(project))
                {
                    var activeProject = spawnOnMapD.projectsEditor.ActiveProject;
                    var firstPoint = activeProject.FindPointById(link.FirstPointId);
                    var secondPoint = activeProject.FindPointById(link.SecondPointId);
                    var onMapLink = spawnOnMapD.SpawnLink(firstPoint, secondPoint);
                    onMapLink.GetComponent<Id>().Value = link.Id;
                    activeProject.AddLink(onMapLink);
                }
                spawnOnMapD.ActivePoint = null;
                spawnOnMapD.FinishLine();
            }
        }
    }
}
