using Microsoft.EntityFrameworkCore;
using Lumavate.Common.Properties;

namespace Lumavate.Models
{
    public class LumavateContext: DbContext{
        
        public LumavateContext(DbContextOptions<LumavateContext> options) :base(options)
        {
        }

        public DbSet<LumavateProperty> Properties { get; set; }
    }
}