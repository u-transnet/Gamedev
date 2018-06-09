using SQLite4Unity3d;

namespace uTrans.Data
{
    public class BaseObjectMaterialDTO : BaseDTO
    {

        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public long BaseObjectId { get; set; }
        public long MaterialId { get; set; }
        public int Amount { get; set; }
        public bool OnExploitation { get; set; }
    }
}