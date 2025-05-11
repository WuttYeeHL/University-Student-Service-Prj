using System.Collections.Generic;
using System.Threading.Tasks;
using DocService.Data.Model;

namespace DocService.Data
{
    public interface IDocumentRepository
    {
        Task<Document> AddDocumentAsync(Document document);
        Task<List<Document>> GetDocumentsByStudentIdAsync(string studentId);
        Task<Document> GetDocumentByIdAsync(string documentId);
        Task<bool> DeleteDocumentAsync(string documentId);
    }
}