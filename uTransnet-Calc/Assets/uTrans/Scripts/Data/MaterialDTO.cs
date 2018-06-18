using SQLite4Unity3d;

namespace uTrans.Data
{
    public class MaterialDTO : BaseDTO
    {
        [PrimaryKey]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public bool Income { get; set; }
    }
}