using autoUploadWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace autoUploadWebApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<FileRecord> FileRecords { get; set; }
    }
}
