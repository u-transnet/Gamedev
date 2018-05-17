using System.Collections.Generic;

namespace uTrans.Data
{
    public class PointDAO : BaseDAO<PointDTO>
    {
        public PointDAO(DBConnection dataService) : base(dataService)
        {

        }

        public PointDTO New()
        {
            var point = new PointDTO();
            base.Insert(point);
            return point;
        }

        public PointDTO New(int projectId, double x, double y)
        {
            var point = new PointDTO();
            point.ProjectId = projectId;
            point.X = x;
            point.Y = y;
            base.Insert(point);
            return point;
        }

        public void UpdateLocation(int id, double x, double y)
        {
            var point = base.GetById(id);
            point.X = x;
            point.Y = y;
            base.Update(point);
        }

        public IEnumerable<PointDTO> FindByProject(ProjectDTO project)
        {
            return FindByProject(project.Id);
        }

        public IEnumerable<PointDTO> FindByProject(int projectId)
        {
            return Find(point => point.ProjectId == projectId);
        }
    }
}