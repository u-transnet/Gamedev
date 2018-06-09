namespace uTrans.Data
{
    using SQLite4Unity3d;

    public class LinkDTO : BaseDTO
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }

        [Indexed]
        public long ProjectId  { get; set; }



        public long FirstPointId { get; set; }

        public long SecondPointId { get; set; }
    }
}