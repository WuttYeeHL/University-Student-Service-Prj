using System;

namespace DocService.Data.Model
{
    public class Document
    {
        public string Id { get; set; }
        public string StudentId { get; set; }
        public string Category { get; set; }
        public DateTime UploadedAt { get; set; }
        public string UploadedBy { get; set; }
        public string DocumentType { get; set; }
        public string FileName { get; set; }
        public string S3Url { get; set; }
        //public Student Student { get; set; }
    }

    public class DocumentUploadModel
    {
        public string StudentId { get; set; }
        public string Category { get; set; }
        public string UploadedBy { get; set; }
        public string DocumentType { get; set; }
        public IFormFile File { get; set; }
    }
}