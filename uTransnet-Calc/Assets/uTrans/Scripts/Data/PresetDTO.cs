using SQLite4Unity3d;

namespace uTrans.Data
{
    public class PresetDTO : BaseDTO
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}