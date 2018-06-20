namespace uTrans.Services
{
    using UnityEngine;
    using uTrans.Data;

    public class DataService : SingletonBehaviour<DataService>
    {


        public DBConnection DBCon { get; private set; }

        public PointDAO PointDAO { get; private set; }

        public LinkDAO LinkDAO { get; private set; }

        public ProjectDAO ProjectDAO { get; private set; }

        public BaseObjectDAO BaseObjectDAO { get; private set; }

        public BaseObjectMaterialDAO BaseObjectMaterialDAO { get; private set; }

        public MaterialDAO MaterialDAO { get; private set; }

        public PresetDAO PresetDAO { get; private set; }

        public PresetMaterialDAO PresetMaterialDAO { get; private set; }


        [SerializeField]
        bool createNewDB;

        override protected void Init()
        {
            Debug.Log("Connecting to DB");
            DBCon = new DBConnection("app.db");

            PrepareTable<PointDTO>();
            PrepareTable<LinkDTO>();
            PrepareTable<ProjectDTO>();
            PrepareTable<BaseObjectDTO>();
            PrepareTable<BaseObjectMaterialDTO>();
            PrepareTable<MaterialDTO>();
            PrepareTable<PresetDTO>();
            PrepareTable<PresetMaterialDTO>();


            PointDAO = new PointDAO(DBCon);
            LinkDAO = new LinkDAO(DBCon);
            ProjectDAO = new ProjectDAO(DBCon);
            BaseObjectDAO = new BaseObjectDAO(DBCon);
            BaseObjectMaterialDAO = new BaseObjectMaterialDAO(DBCon);
            MaterialDAO = new MaterialDAO(DBCon);
            PresetDAO = new PresetDAO(DBCon);
            PresetMaterialDAO = new PresetMaterialDAO(DBCon);
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
