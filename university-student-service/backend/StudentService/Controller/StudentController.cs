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
    }

}
