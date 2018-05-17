namespace uTrans.Services
{
    using UnityEngine;
    using uTrans.Data;
    using utrans.Services;

    public class DataService : SingletonBehaviour<DataService>
    {


        public DBConnection DBCon { get; private set; }

        public PointDAO PointDAO { get; private set; }

        public LinkDAO LinkDAO { get; private set; }

        public ProjectDAO ProjectDAO { get; private set; }


        [SerializeField]
        bool createNewDB;

        override protected void Init()
        {
            Debug.Log("Connecting to DB");
            DBCon = new DBConnection("app.db");

            PrepareTable<PointDTO>();
            PrepareTable<LinkDTO>();
            PrepareTable<ProjectDTO>();

            PointDAO = new PointDAO(DBCon);
            LinkDAO = new LinkDAO(DBCon);
            ProjectDAO = new ProjectDAO(DBCon);
        }

        private void PrepareTable<T>() where T : BaseDTO
        {
            if (createNewDB)
            {
                DBCon.CreateTable<T>();
            }
            else
            {
                DBCon.CreateOrUpdateTable<T>();
            }
        }
    }


}
