using Microsoft.AspNetCore.Mvc;
using DocService.Services;
using DocService.Data.Model;
using Microsoft.AspNetCore.Authorization;

namespace DocService.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DocController : ControllerBase
    {
        private readonly IDocumentService _documentService;

        public DocController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadDocument([FromForm] DocumentUploadModel model)
        {
            try
            {
                if (model.File == null || model.File.Length == 0)
                    return BadRequest("No file uploaded.");

                var document = await _documentService.UploadDocumentAsync(model);
                var response = new
                {
                    DocumentId = document.Id,
                    FileName = document.FileName,
                    S3Url = document.S3Url,
                    UploadedAt = document.UploadedAt.ToString("o")
                };
                return Ok(response);
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
                var response = documents.Select(doc => new
                {
                    Id = doc.Id,
                    StudentId = doc.StudentId,
                    Category = doc.Category,
                    UploadedAt = doc.UploadedAt,
                    UploadedBy = doc.UploadedBy,
                    DocumentType = doc.DocumentType,
                    FileName = doc.FileName
                }).ToList();

                return Ok(response);
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
                if (fileStream == null || fileStream.ResponseStream == null || fileStream.ContentLength == 0)
                    return BadRequest("File stream is empty or invalid.");
                string contentType = fileStream.Headers.ContentType ?? "application/octet-stream";
                Response.Headers["Content-Disposition"] = $"attachment; filename=\"{document.FileName}\"; filename*=UTF-8''{Uri.EscapeDataString(document.FileName)}";

                return File(fileStream.ResponseStream, contentType, document.FileName);
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

                return Ok(new { message = "Document deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting document: {ex.Message}");
            }
        }
    }
}