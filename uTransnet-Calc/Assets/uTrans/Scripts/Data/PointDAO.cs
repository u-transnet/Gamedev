using System.Collections.Generic;
using uTrans.Calc;

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

        public PointDTO New(int projectId, double x, double y, float height, PointType type)
        {
            var point = new PointDTO();
            point.ProjectId = projectId;
            point.X = x;
            point.Y = y;
            point.Height = height;
            point.Type = (int) type;
            base.Insert(point);
            return point;
        }

        public void UpdateLocationAndHeight(int id, double x, double y, float height)
        {
            var point = base.GetById(id);
            point.X = x;
            point.Y = y;
            point.Height = height;
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