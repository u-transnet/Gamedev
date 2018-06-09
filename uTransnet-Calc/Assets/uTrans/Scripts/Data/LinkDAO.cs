using System.Collections.Generic;

namespace uTrans.Data
{
    public class LinkDAO : BaseDAO<LinkDTO>
    {
        public LinkDAO(DBConnection dataService) : base(dataService)
        {
        }

        public LinkDTO New(long projectId, long firstPointId, long secondPointId)
        {
            var link = new LinkDTO();
            link.ProjectId = projectId;
            link.FirstPointId = firstPointId;
            link.SecondPointId = secondPointId;
            base.Insert(link);
            return link;
        }

        public IEnumerable<LinkDTO> FindByProject(ProjectDTO project)
        {
            return FindByProject(project.Id);
        }

        public IEnumerable<LinkDTO> FindByProject(long projectId)
        {
            return Find(point => point.ProjectId == projectId);
        }
    }
}