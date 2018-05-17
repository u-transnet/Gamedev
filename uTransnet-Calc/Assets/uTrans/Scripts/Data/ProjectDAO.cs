namespace uTrans.Data
{
    public class ProjectDAO : BaseDAO<ProjectDTO>
    {
        public ProjectDAO(DBConnection dataService) : base(dataService)
        {
        }


        public ProjectDTO New()
        {
            var project = new ProjectDTO();
            base.Insert(project);
            return project;
        }

    }
}