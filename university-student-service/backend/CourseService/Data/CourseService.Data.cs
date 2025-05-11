using Dapper;
using Microsoft.Data.SqlClient; 
using CourseService.Model;
using System.Data;

namespace CourseService.Data
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Qualification>> GetCoursesAsync();
    }

    public class CourseRepository : ICourseRepository
    {
        private readonly IConfiguration _configuration;

        public CourseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private async Task<IDbConnection> CreateConnection()
        {
            var connStr = _configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connStr))
                throw new InvalidOperationException("DefaultConnection string is missing or empty.");

            var connection = new SqlConnection(connStr);
            await connection.OpenAsync();

            return connection;
        }

        public async Task<IEnumerable<Qualification>> GetCoursesAsync()
        {
            var sql = @"
                    SELECT 
                        q.Id, q.Code, q.Description, q.Points, q.Level,
                        c.Id, c.Code, c.Description, c.Points, c.QualificationId
                    FROM Qualifications q
                    LEFT JOIN Courses c ON q.Id = c.QualificationId
                    ORDER BY q.Id;
                ";

            using var connection = await CreateConnection(); // Fix: Await the CreateConnection method to get the IDbConnection instance

            var qualificationDict = new Dictionary<int, Qualification>();

            var result = await connection.QueryAsync<Qualification, Course, Qualification>(
                sql,
                (q, c) =>
                {
                    if (!qualificationDict.TryGetValue(q.Id, out var qualification))
                    {
                        qualification = q;
                        qualification.Courses = new List<Course>();
                        qualificationDict.Add(q.Id, qualification);
                    }

                    if (c != null)
                        qualification.Courses.Add(c);

                    return qualification;
                },
                splitOn: "Id"
            );

            return qualificationDict.Values.ToList();
        }

        public async Task<Qualification> GetCourseByIdAsync(int Id)
        {
            var sql = @"
                    SELECT 
                        q.Id, q.Code, q.Description, q.Points, q.Level,
                        c.Id, c.Code, c.Description, c.Points, c.QualificationId
                    FROM Qualifications q
                    LEFT JOIN Courses c ON q.Id = c.QualificationId
                    WHERE q.Id = @Id
                    ORDER BY q.Id;
                ";

            using var connection = await CreateConnection(); // Fix: Await the CreateConnection method to get the IDbConnection instance

            var qualificationDict = new Dictionary<int, Qualification>();

            var result = await connection.QueryAsync<Qualification, Course, Qualification>(
                sql,
                (q, c) =>
                {
                    if (!qualificationDict.TryGetValue(q.Id, out var qualification))
                    {
                        qualification = q;
                        qualification.Courses = new List<Course>();
                        qualificationDict.Add(q.Id, qualification);
                    }

                    if (c != null)
                        qualification.Courses.Add(c);

                    return qualification;
                },
                param: new { Id },
                splitOn: "Id"
            );

            return qualificationDict.Values.FirstOrDefault();
        }
    }
}



