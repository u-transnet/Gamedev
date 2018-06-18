using uTrans.Data;

namespace uTrans.Data
{
    public class PresetDAO : BaseDAO<PresetDTO>
    {
        public PresetDAO(DBConnection dataService) : base(dataService)
        {
        }


    }
}