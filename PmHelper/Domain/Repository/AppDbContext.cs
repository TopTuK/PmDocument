using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PmHelper.Domain.Repository.Entities;

/*
 * https://jasonwatmore.com/post/2022/09/05/net-6-connect-to-sqlite-database-with-entity-framework-core
*/

namespace PmHelper.Domain.Repository
{
    public class AppDbContext : DbContext
    {
        private readonly ILogger<AppDbContext> _logger;
        private readonly IConfiguration _configuration;

        public AppDbContext(ILogger<AppDbContext> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _logger.LogInformation("Start configuration database context");

            optionsBuilder.UseSqlite(_configuration.GetConnectionString("WebApiDatabase"));

            base.OnConfiguring(optionsBuilder);

            _logger.LogInformation("End configuration database context");
        }

        public DbSet<DbUser> Users { get; set; }
    }
}
