using uTrans.Data;

namespace uTrans.Data
{
    public class BaseObjectDAO : BaseDAO<BaseObjectDTO>
    {
        public BaseObjectDAO(DBConnection dataService) : base(dataService)
        {
        }


    }
}