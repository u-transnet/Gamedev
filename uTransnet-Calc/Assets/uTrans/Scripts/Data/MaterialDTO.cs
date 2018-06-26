using SQLite4Unity3d;

namespace uTrans.Data
{
    public class MaterialDTO : BaseDTO
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Name { get; set; }
        public Unit Unit { get; set; }
        public bool Income { get; set; }
    }
}