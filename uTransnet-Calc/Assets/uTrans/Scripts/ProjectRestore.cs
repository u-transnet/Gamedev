using uTrans.Calc;
using uTrans.Components;
using uTrans.Network;
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

        void Start()
        {
            map.OnInitialized += PlacePoints;
            ServerCommunication.ListPresets(presetDTOs => {
                    foreach (var preset in presetDTOs)
                    {
                        ServerCommunication.GetPresetMaterials(preset.Id, presetMaterials => {
                            Debug.Log(presetMaterials);
                        });
                    }
            });
        }

        void PlacePoints()
        {
            foreach (var project in DataService.instance.ProjectDAO.All())
            {
                spawnOnMapD.ProjectsEditor.NewProject(project.Id);
                foreach (var point in DataService.instance.PointDAO.FindByProject(project))
                {
                    var worldPosition = map.GeoToWorldPosition(new Vector2d(point.X, point.Y));
                    spawnOnMapD.ActivePoint = spawnOnMapD.Spawn((PointType) point.Type, worldPosition, point.Id);
                    spawnOnMapD.ActivePoint.GetComponent<BasePoint>().objectWithHeight.Height = point.Height;
                }

                foreach (var link in DataService.instance.LinkDAO.FindByProject(project))
                {
                    var activeProject = spawnOnMapD.ProjectsEditor.ActiveProject;
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
