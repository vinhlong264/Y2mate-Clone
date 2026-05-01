using System.ComponentModel.DataAnnotations;

namespace Y2mate_Fake.DBContext
{
    public class DbSet_VideoInfor
    {
        [Key]
        public string ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Duration { get; set; }
        public string Thumbnail { get; set; }
    }
}
