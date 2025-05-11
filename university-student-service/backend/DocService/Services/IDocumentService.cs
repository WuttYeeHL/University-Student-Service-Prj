using DocService.Data.Model;

namespace DocService.Services
{
    public interface IDocumentService
    {
        Task<Document> UploadDocumentAsync(DocumentUploadModel model);
        Task<List<Document>> GetDocumentsByStudentIdAsync(string studentId);
        Task<Document> GetDocumentByIdAsync(string documentId);
        Task<Stream> DownloadDocumentAsync(string documentId);
        Task<bool> DeleteDocumentAsync(string documentId);
    }
}