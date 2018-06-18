namespace uTrans.Data
{
    using SQLite4Unity3d;

    public class PointDTO : BaseDTO
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }

        [Indexed]
        public long ProjectId  { get; set; }

        public long BaseObjectId  { get; set; }

        public double X { get; set; }

        public double Y { get; set; }

        public int Type{ get; set; }
    }
}
