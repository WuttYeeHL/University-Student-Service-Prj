using EnrolmentService.Model;
using Microsoft.EntityFrameworkCore;

namespace EnrolmentService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Enrolment> Enrolments => Set<Enrolment>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Qualification> Qualifications => Set<Qualification>();
    public DbSet<Student> Students => Set<Student>();
}