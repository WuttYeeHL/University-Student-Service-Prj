using EnrolmentService.Dto;

namespace EnrolmentService.Services;

public interface IStudentEnrolmentService
{
    Task<List<EnrolmentDto>> GetEnrolmentsByUserIdAsync(int studentId);
}
