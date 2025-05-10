namespace EnrolmentService.Model;

public class Enrolment
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public int QualificationId { get; set; }
    public DateTime StartDate { get; set; }
    public string Period { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
}