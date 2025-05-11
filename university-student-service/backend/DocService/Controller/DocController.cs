using Microsoft.AspNetCore.Mvc;
using DocService.Services;
using DocService.Data.Model;
using Microsoft.AspNetCore.Authorization;

namespace DocService.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocController : ControllerBase
    {
        private readonly IDocumentService _documentService;

        public DocController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpPost("upload")]
        [AllowAnonymous]
        public async Task<IActionResult> UploadDocument([FromForm] DocumentUploadModel model)
        {
            try
            {
                if (model.File == null || model.File.Length == 0)
                    return BadRequest("No file uploaded.");

                var document = await _documentService.UploadDocumentAsync(model);
                return Ok(new { DocumentId = document.Id, FileName = document.FileName, S3Url = document.S3Url });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error uploading document: {ex.Message}");
            }
        }

        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetDocumentsByStudentId(string studentId)
        {
            try
            {
                var documents = await _documentService.GetDocumentsByStudentIdAsync(studentId);
                return Ok(documents);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving documents: {ex.Message}");
            }
        }

        [HttpGet("{documentId}")]
        public async Task<IActionResult> DownloadDocument(string documentId)
        {
            try
            {
                var document = await _documentService.GetDocumentByIdAsync(documentId);
                if (document == null)
                    return NotFound("Document not found.");

                var fileStream = await _documentService.DownloadDocumentAsync(documentId);
                return File(fileStream, "application/octet-stream", document.FileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error downloading document: {ex.Message}");
            }
        }

        [HttpDelete("{documentId}")]
        public async Task<IActionResult> DeleteDocument(string documentId)
        {
            try
            {
                var success = await _documentService.DeleteDocumentAsync(documentId);
                if (!success)
                    return NotFound("Document not found.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting document: {ex.Message}");
            }
        }
    }
}