using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Y2mate_Fake.DBContext
{
    public class DbSet_DownLoadJob
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public DownloadStatus status { get; set; }
        public int Bitrate { get; set; }
        public string Path { get; set; }
        public int Progress { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime CompletedAt { get; set; }
    }

    public enum DownloadStatus : byte
    {
        Waiting = 0,
        Downloading = 1,
        Completed = 2,
        Failed = 3
    }
}
