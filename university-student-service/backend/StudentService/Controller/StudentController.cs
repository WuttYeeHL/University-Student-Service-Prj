﻿using Amazon.S3;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StudentService.Data;
using StudentService.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentService.Controller
{ 

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository _repository;
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;
        private IConfiguration _configuration;
        public StudentsController(IStudentRepository repository, IAmazonS3 s3Client, IConfiguration configuration)
        {
            _repository = repository;
            _s3Client = s3Client;
            _bucketName = configuration["AWS:BucketName"];
            _configuration = configuration;
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

            //var url = await _repository.UploadProfileImageAsync(model, Guid.NewGuid().ToString());
            var url = await _repository.UploadProfileImageAsync(model, Guid.NewGuid().ToString(), _s3Client, _bucketName);


            return Ok(new { url });
        }

    }

}
