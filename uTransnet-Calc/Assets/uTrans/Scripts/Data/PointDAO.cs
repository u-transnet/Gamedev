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

        public PointDTO New(long projectId, double x, double y, PointType type)
        {
            var point = new PointDTO();
            point.ProjectId = projectId;
            point.X = x;
            point.Y = y;
            point.Type = (int) type;
            base.Insert(point);
            return point;
        }

        public void UpdateLocation(long id, double x, double y)
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

        public IEnumerable<PointDTO> FindByProject(long projectId)
        {
            return Find(point => point.ProjectId == projectId);
        }
    }
}