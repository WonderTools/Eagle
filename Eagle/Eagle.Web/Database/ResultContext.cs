using Microsoft.EntityFrameworkCore;

namespace Eagle.Web.Database
{
    public class ResultContext : DbContext
    {
        public ResultContext(DbContextOptions<ResultContext> options) : base(options)
        {
            
        }
        public DbSet<TestResult> TestResults { get; set; }
    }
}