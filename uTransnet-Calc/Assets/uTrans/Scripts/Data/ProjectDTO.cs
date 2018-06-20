using SQLite4Unity3d;

namespace uTrans.Data
{
    public class ProjectDTO : BaseDTO
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

    }
}