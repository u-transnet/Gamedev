using SQLite4Unity3d;

namespace uTrans.Data
{
    public class PresetDTO : BaseDTO
    {
        [PrimaryKey]
        public long Id { get; set; }

        public string Name { get; set; }
    }
}