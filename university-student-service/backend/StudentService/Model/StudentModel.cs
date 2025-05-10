namespace StudentService.Model
{
    public class Student
    {
        public int Student_Id { get; set; }
        public string Title { get; set; }
        public string Preferred_First_Name { get; set; }
        public string Preferred_Last_Name { get; set; }
        public string Gender { get; set; }
        public string Name { get; set; }
        public string Passport { get; set; }
        public string Country { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string ProfileImage { get; set; }

        public string Permanent_streetNumber { get; set; }
        public string Permanent_streetName { get; set; }
        public string Permanent_suburb { get; set; }
        public string Permanent_city { get; set; }
        public string Permanent_country { get; set; }
        public string Permanent_postcode { get; set; }
        public string Permanent_phone { get; set; }

        public string Study_streetNumber { get; set; }
        public string Study_streetName { get; set; }
        public string Study_suburb { get; set; }
        public string Study_city { get; set; }
        public string Study_country { get; set; }
        public string Study_postcode { get; set; }
        public string Study_phone { get; set; }

        public string Contact_Email { get; set; }
        public string Confirm_Email { get; set; }

        public string EmergencyContact_Name { get; set; }
        public string EmergencyContact_Relationship { get; set; }

        public int? User_Id { get; set; }
        public string Student_Code { get; set; }

        public List<Education> TertiaryRecords { get; set; } = new();
    }
    public class Education
    {
        public int Edu_Id { get; set; }
        public int Student_Id { get; set; }
        public string YearFrom { get; set; }
        public string YearTo { get; set; }
        public string Institution { get; set; }
        public string Qualification { get; set; }
        public string Location { get; set; }
        public bool Completed { get; set; }
        public bool Awaiting { get; set; }
    }
}
