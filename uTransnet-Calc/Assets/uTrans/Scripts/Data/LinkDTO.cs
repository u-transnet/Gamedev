namespace uTrans.Data
{
    using SQLite4Unity3d;

    public class LinkDTO : BaseDTO
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public int ProjectId  { get; set; }



        public int FirstPointId { get; set; }

        public int SecondPointId { get; set; }
    }
}