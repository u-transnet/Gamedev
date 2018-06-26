using SQLite4Unity3d;

namespace uTrans.Data
{
    public class BaseObjectMaterialDTO : BaseDTO
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int BaseObjectId { get; set; }
        public int MaterialId { get; set; }
        public int Amount { get; set; }
        public bool OnExploitation { get; set; }
        public bool UserEditable { get; set; }
        public int GroupNumber { get; set; }
        public int NumberInGroup { get; set; }
    }
}