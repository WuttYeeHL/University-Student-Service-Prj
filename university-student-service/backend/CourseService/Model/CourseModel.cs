using System.Security.Principal;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CourseService.Model
{
    public class Qualification
    {
        public int Id { get; set; }
        public string Code { get; set; }           
        public string Description { get; set; }
        public string Level { get; set; }
        public double Points { get; set; }         
        public List<Course> Courses { get; set; }
    }

    public class Course
    {
        public int Id { get; set; }
        public string Code { get; set; }           
        public string Description { get; set; }    
        public double Points { get; set; }         


        public string QualificationCode { get; set; }     
        public Qualification Qualification { get; set; }  
    }

}
