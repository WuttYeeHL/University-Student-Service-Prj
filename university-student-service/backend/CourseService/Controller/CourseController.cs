using System.Reflection.PortableExecutable;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using CourseService.Model;
using CourseService.Data;
using Amazon.S3.Model;
using Amazon.S3;
using Microsoft.Extensions.Configuration;

namespace CourseService.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly CourseRepository _repository;
        private readonly IAmazonS3 _s3Client;
        private readonly IConfiguration _configuration;

        public CourseController(CourseRepository repository, IAmazonS3 s3Client, IConfiguration configuration)
        {
            _repository = repository;
            _s3Client = s3Client;
            _configuration = configuration;
        }


        [HttpGet]
        public async Task<IActionResult> GetQualifications()
        {
            var qualifications = await _repository.GetCoursesAsync();

            return Ok(qualifications);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetQualifications(int id)
        {
            var qualification = await _repository.GetCourseByIdAsync(id);

            return Ok(qualification);
        }

        //[HttpGet("api/s3/sign")]
        //public IActionResult GetPresignedUrl([FromQuery] string key)
        //{
        //    var signer = new S3Presigner();
        //    string url = signer.GeneratePreSignedUrl(key);
        //    return Ok(new { url });
        //}

        [HttpGet("download")]
        public async Task<IActionResult> DownloadFromS3([FromQuery] string key)
        {
            var bucketName = _configuration.GetValue<string>("DownloadBucket"); 

            var request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = key
            };

            try
            {
                var response = await _s3Client.GetObjectAsync(request);
                //var stream = response.ResponseStream;
                var contentType = response.Headers.ContentType ?? "application/octet-stream";
                var fileName = Path.GetFileName(key);

                return File(response.ResponseStream, contentType, fileName);
            }
            catch (AmazonS3Exception ex)
            {
                return NotFound("File not found in S3: " + ex.Message);
            }
        }

        [HttpGet("test-s3")]
        public async Task<IActionResult> TestS3()
        {
            var buckets = await _s3Client.ListBucketsAsync();
            return Ok(buckets.Buckets.Select(b => b.BucketName));
        }

    }

}
