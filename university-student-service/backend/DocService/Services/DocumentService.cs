using Amazon.S3;
using Amazon.S3.Model;
using DocService.Data;
using DocService.Data.Model;

namespace DocService.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName = "university-student-service-documents";

        public DocumentService(IDocumentRepository documentRepository, IAmazonS3 s3Client)
        {
            _documentRepository = documentRepository;
            _s3Client = s3Client;
        }

        public async Task<Document> UploadDocumentAsync(DocumentUploadModel model)
        {
            var fileName = $"{Guid.NewGuid()}_{model.File.FileName}";
            var s3Key = $"documents/{model.StudentId}/{fileName}";

            using (var stream = model.File.OpenReadStream())
            {
                var putRequest = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = s3Key,
                    InputStream = stream,
                    ContentType = model.File.ContentType
                };
                await _s3Client.PutObjectAsync(putRequest);
            }

            var document = new Document
            {
                Id = Guid.NewGuid().ToString(),
                StudentId = model.StudentId,
                Category = model.Category,
                UploadedAt = DateTime.UtcNow,
                UploadedBy = model.UploadedBy,
                DocumentType = model.DocumentType,
                FileName = model.File.FileName,
                S3Url = $"https://{_bucketName}.s3.amazonaws.com/{s3Key}"
            };

            return await _documentRepository.AddDocumentAsync(document);
        }

        public async Task<List<Document>> GetDocumentsByStudentIdAsync(string studentId)
        {
            return await _documentRepository.GetDocumentsByStudentIdAsync(studentId);
        }

        public async Task<Document> GetDocumentByIdAsync(string documentId)
        {
            return await _documentRepository.GetDocumentByIdAsync(documentId);
        }

        public async Task<Stream> DownloadDocumentAsync(string documentId)
        {
            var document = await _documentRepository.GetDocumentByIdAsync(documentId);
            if (document == null)
                throw new Exception("Document not found.");

            var s3Key = new Uri(document.S3Url).AbsolutePath.TrimStart('/');
            var getRequest = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = s3Key
            };

            var response = await _s3Client.GetObjectAsync(getRequest);
            return response.ResponseStream;
        }

        public async Task<bool> DeleteDocumentAsync(string documentId)
        {
            var document = await _documentRepository.GetDocumentByIdAsync(documentId);
            if (document == null)
                return false;

            var s3Key = new Uri(document.S3Url).AbsolutePath.TrimStart('/');
            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = s3Key
            };
            await _s3Client.DeleteObjectAsync(deleteRequest);

            return await _documentRepository.DeleteDocumentAsync(documentId);
        }
    }
}