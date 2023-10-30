using Microsoft.EntityFrameworkCore;
using Sprout.Exam.DataAccess.Entities;

namespace Sprout.Exam.DataAccess.Data
{
    public class DataDbContext : DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options)
        {
            
        }
        public DbSet<EmployeeEntity> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<EmployeeEntity>( entity =>
            {
                entity.HasIndex(e => e.FullName)
                .IsUnique(true);  

                entity.HasIndex(e => e.Tin)
                .IsUnique(true);  

                entity.HasIndex(b => new { b.FullName, b.Tin });

                entity.Property(e => e.Birthdate)
                .HasColumnType("date");
            });  
        }
    }
}