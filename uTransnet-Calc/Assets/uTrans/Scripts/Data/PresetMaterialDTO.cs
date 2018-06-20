using SQLite4Unity3d;

namespace uTrans.Data
{
    public class PresetMaterialDTO : BaseDTO
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int PresetId { get; set; }
        public int MaterialId { get; set; }
        public int Price { get; set; }
    }
}