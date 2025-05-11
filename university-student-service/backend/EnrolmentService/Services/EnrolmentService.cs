using Microsoft.EntityFrameworkCore;
using EnrolmentService.Dto;
using EnrolmentService.Data;

namespace EnrolmentService.Services;

public class StudentEnrolmentService : IStudentEnrolmentService
{
    private readonly AppDbContext _db;

    public StudentEnrolmentService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<EnrolmentDto>> GetEnrolmentsByUserIdAsync(int userId)
    {
        return await (
            from e in _db.Enrolments
            join s in _db.Students on e.StudentId equals s.Student_id
            join c in _db.Courses on e.CourseId equals c.Id
            join q in _db.Qualifications on e.QualificationId equals q.Id
            where s.User_id == userId
            select new EnrolmentDto
            {
                Period = e.Period,
                CourseCode = c.Code,
                CourseDescription = c.Description,
                Status = e.Status,
                StartDate = e.StartDate.ToString(),
                Location = e.Location,
                Level = q.Level.ToString(),
                Qualification = q.Description
            }).ToListAsync();
    }
}
