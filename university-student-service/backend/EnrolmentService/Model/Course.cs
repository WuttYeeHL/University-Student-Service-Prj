namespace EnrolmentService.Model;

public class Course
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int QualificationId { get; set; }
}
