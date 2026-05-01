using Microsoft.EntityFrameworkCore;
namespace Y2mate_Fake.DBContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<DbSet_VideoInfor> VideoInfors { get; set; }
        public DbSet<DbSet_DownLoadJob> DownLoadJobs { get; set; }
    }
}
