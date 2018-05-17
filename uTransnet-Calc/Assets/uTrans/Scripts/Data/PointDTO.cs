namespace uTrans.Data
{
    using SQLite4Unity3d;

    public class PointDTO : BaseDTO
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public int ProjectId  { get; set; }

        public double X { get; set; }

        public double Y { get; set; }
    }
}
