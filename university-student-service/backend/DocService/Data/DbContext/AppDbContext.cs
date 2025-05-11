using Microsoft.EntityFrameworkCore;
using DocService.Data.Model;

namespace DocService.Data.DbContexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<Document> Documents { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Document>(entity =>
            {
                entity.HasKey(d => d.Id);
                entity.Property(d => d.Id)
                      .HasMaxLength(36) // GUID length as string
                      .ValueGeneratedNever(); // Id is set manually
                entity.Property(d => d.StudentId).IsRequired();
                entity.Property(d => d.Category).IsRequired();
                entity.Property(d => d.UploadedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(d => d.UploadedBy).IsRequired();
                entity.Property(d => d.DocumentType).IsRequired();
                entity.Property(d => d.FileName).IsRequired();
                entity.Property(d => d.S3Url).IsRequired();

                // Foreign key relationship with Student
                /*entity.HasOne(d => d.Student)
                      .WithMany(s => s.Documents)
                      .HasForeignKey(d => d.StudentId)
                      .OnDelete(DeleteBehavior.Cascade);*/
            });

            /*modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.FirstName).IsRequired();
                entity.Property(s => s.LastName).IsRequired();
            });*/

        }
    }
}