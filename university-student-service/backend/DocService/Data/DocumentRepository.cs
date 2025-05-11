using Microsoft.EntityFrameworkCore;
using DocService.Data.DbContexts;
using DocService.Data.Model;

namespace DocService.Data
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly AppDbContext _context;

        public DocumentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Document> AddDocumentAsync(Document document)
        {
            //document.Id = Guid.NewGuid().ToString();
            document.UploadedAt = DateTime.UtcNow;
            _context.Documents.Add(document);
            await _context.SaveChangesAsync();
            return document;
        }

        public async Task<List<Document>> GetDocumentsByStudentIdAsync(string studentId)
        {
            return await _context.Documents
                .Where(d => d.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<Document> GetDocumentByIdAsync(string documentId)
        {
            return await _context.Documents
            .FirstOrDefaultAsync(d => d.Id == documentId);
        }

        public async Task<bool> DeleteDocumentAsync(string documentId)
        {
            var document = await _context.Documents
                .FirstOrDefaultAsync(d => d.Id == documentId);
            if (document == null)
                return false;

            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}