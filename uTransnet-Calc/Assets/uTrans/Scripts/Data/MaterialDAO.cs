using uTrans.Data;

namespace uTrans.Data
{
    public class MaterialDAO : BaseDAO<MaterialDTO>
    {
        public MaterialDAO(DBConnection dataService) : base(dataService)
        {
        }


    }
}