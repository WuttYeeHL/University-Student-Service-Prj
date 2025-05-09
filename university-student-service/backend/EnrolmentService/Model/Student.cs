using System.ComponentModel.DataAnnotations;

namespace EnrolmentService.Model;

public class Student
{
    [Key]
    public int Student_id { get; set; }
    public int User_id { get; set; }

}