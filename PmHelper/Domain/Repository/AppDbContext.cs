using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PmHelper.Domain.Repository.Entities;
using System.Xml.Linq;

/*
 * https://jasonwatmore.com/post/2022/09/05/net-6-connect-to-sqlite-database-with-entity-framework-core
*/

namespace PmHelper.Domain.Repository
{
    public class AppDbContext : DbContext
    {
        private readonly ILogger<AppDbContext> _logger;
        private readonly IConfiguration _configuration;

        public DbSet<DbUser> Users { get; set; }
        public DbSet<DbDocumentType> DocumentTypes { get; set; }
        public DbSet<DbDocumentSection> DocumentSections { get; set; }
        public DbSet<DbDocumentRule> DocumentRules { get; set; }
        public DbSet<DbDocumentRuleType> DocumentRuleTypes { get; set; }
        public DbSet<DbUserDocument> UserDocuments { get; set; }

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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
