using StudentService.Model;
using System.Data.SqlClient;
using Dapper;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;
using Amazon.S3.Transfer;

namespace StudentService.Data
{
    public interface IStudentRepository
    {
        Student GetByUserId(int userId);
        void Update(Student student);
        Task<string> UploadProfileImageAsync([FromForm] UploadFileModel model, string studentId);
    }
    public class StudentRepository : IStudentRepository
    {
        private readonly string _connectionString;

        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName = "kllprofileimgbucket";

        public StudentRepository(IConfiguration configuration, IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
            _bucketName = configuration["AWS:BucketName"];
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }        

        public async Task<string> UploadProfileImageAsync([FromForm] UploadFileModel model, string studentId)
        {
            //if (file == null || file.Length == 0)
            //    throw new ArgumentException("Invalid file");

            var fileExtension = Path.GetExtension(model.File.FileName);
            var key = $"profiles/{studentId}{fileExtension}";

            using (var stream = model.File.OpenReadStream())
            {
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = stream,
                    Key = key,
                    BucketName = _bucketName,
                    ContentType = model.File.ContentType
                };

                var transferUtility = new TransferUtility(_s3Client);
                await transferUtility.UploadAsync(uploadRequest);
            }
            return $"{studentId}{fileExtension}";
        }

        public Student GetByUserId(int userId)
        {
            using var connection = new SqlConnection(_connectionString);

            var student = connection.QuerySingleOrDefault<Student>(
                "SELECT * FROM Students WHERE user_id = @UserId",
                new { UserId = userId });

            if (student != null)
            {
                var education = connection.Query<Education>(
                    "SELECT * FROM StudentEducation WHERE student_id = @StudentId",
                    new { StudentId = student.Student_Id }).ToList();

                student.TertiaryRecords = education;
            }

            return student;
        }

        public void Update(Student student)
        {
            using var connection = new SqlConnection(_connectionString);

            var updateSql = @"
                UPDATE Students SET 
                    Title = @Title,
                    Preferred_First_Name = @Preferred_First_Name,
                    Preferred_Last_Name = @Preferred_Last_Name,
                    Gender = @Gender,
                    Name = @Name,
                    Passport = @Passport,
                    Country = @Country,
                    DateOfBirth = @DateOfBirth,
                    ProfileImage = @ProfileImage,
                    Permanent_streetNumber = @Permanent_streetNumber,
                    Permanent_streetName = @Permanent_streetName,
                    Permanent_suburb = @Permanent_suburb,
                    Permanent_city = @Permanent_city,
                    Permanent_country = @Permanent_country,
                    Permanent_postcode = @Permanent_postcode,
                    Permanent_phone = @Permanent_phone,
                    Study_streetNumber = @Study_streetNumber,
                    Study_streetName = @Study_streetName,
                    Study_suburb = @Study_suburb,
                    Study_city = @Study_city,
                    Study_country = @Study_country,
                    Study_postcode = @Study_postcode,
                    Study_phone = @Study_phone,
                    Contact_Email = @Contact_Email,
                    Confirm_Email = @Confirm_Email,
                    EmergencyContact_Name = @EmergencyContact_Name,
                    EmergencyContact_Relationship = @EmergencyContact_Relationship,
                    Student_Code = @Student_Code
                WHERE student_id = @Student_Id";

            connection.Execute(updateSql, student);

            // Clear existing education records
            connection.Execute("DELETE FROM StudentEducation WHERE student_id = @StudentId", new { StudentId = student.Student_Id });

            // Insert new education records
            foreach (var record in student.TertiaryRecords)
            {
                var insertSql = @"
                    INSERT INTO StudentEducation 
                    (student_id, YearFrom, YearTo, Institution, Qualification, Location, Completed, Awaiting)
                    VALUES 
                    (@Student_Id, @YearFrom, @YearTo, @Institution, @Qualification, @Location, @Completed, @Awaiting)";
                connection.Execute(insertSql, new
                {
                    Student_Id = student.Student_Id,
                    record.YearFrom,
                    record.YearTo,
                    record.Institution,
                    record.Qualification,
                    record.Location,
                    record.Completed,
                    record.Awaiting
                });
            }
        }
    }
}
