using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using StudentService.Data;
using StudentService.Model;

namespace StudentService.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository _repository;
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;
        public StudentsController(IStudentRepository repository)
        {
            _repository = repository;
        }

        // GET api/Students/user/1
        [HttpGet("user/{userId}")]
        public ActionResult<Student> GetByUserId(int userId)
        {
            var student = _repository.GetByUserId(userId);
            if (student == null)
                return NotFound();

            return Ok(student);
        }

        // PUT api/Students
        [HttpPut]
        public IActionResult UpdateStudent([FromBody] Student student)
        {
            _repository.Update(student);
            return Ok();
        }

        [HttpPost("UploadProfileImage")]
        public async Task<IActionResult> UploadFile([FromForm] UploadFileModel model)
        {
            if (model.File == null || model.File.Length == 0)
                return BadRequest("No file uploaded.");

            var url = await _repository.UploadProfileImageAsync(model, Guid.NewGuid().ToString());

            return Ok(new { url });
        }

    }

}
