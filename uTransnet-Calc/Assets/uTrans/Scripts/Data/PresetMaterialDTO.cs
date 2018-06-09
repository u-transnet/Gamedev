using SQLite4Unity3d;

namespace uTrans.Data
{
    public class PresetMaterialDTO : BaseDTO
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public long PresetId { get; set; }
        public long MaterialId { get; set; }
        public int Price { get; set; }
    }
}