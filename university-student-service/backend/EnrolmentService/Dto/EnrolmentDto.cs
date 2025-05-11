namespace EnrolmentService.Dto;

public class EnrolmentDto
{
    public string Period { get; set; } = string.Empty;
    public string CourseCode { get; set; } = string.Empty;
    public string CourseDescription { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string StartDate { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public string Qualification { get; set; }  = string.Empty;
}