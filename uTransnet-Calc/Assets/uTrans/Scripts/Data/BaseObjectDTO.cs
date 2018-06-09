using SQLite4Unity3d;

namespace uTrans.Data
{
    public class BaseObjectDTO : BaseDTO
    {
        [PrimaryKey]
        public long Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int MinSize { get; set; }
        public int MaxSize { get; set; }
    }
}