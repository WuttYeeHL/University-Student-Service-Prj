import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { StudentDocs } from '../model/interface/studentDocs';
import { STUDENT_DOCUMENTS_API_URL } from '../../app/constant/Constant';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})

export class StudentDocService {
  private readonly apiUrl = STUDENT_DOCUMENTS_API_URL;
  
  constructor(private http: HttpClient) {}

   // Upload a document
   uploadDocument(studentId: string, category: string, uploadedBy: string, type: string, file: File): Observable<StudentDocs> {
    const formData = new FormData();
    formData.append('StudentId', studentId);
    formData.append('Category', category);
    formData.append('UploadedBy', uploadedBy);
    formData.append('DocumentType', type);
    formData.append('File', file);
    formData.append('DateUploaded', new Date().toISOString());
    
     return this.http
      .post<{ DocumentId: string; FileName: string; S3Url: string; uploadedAt: string }>(`${this.apiUrl}/upload`, formData)
      .pipe(
        map(response => ({
          id: response.DocumentId,
          studentId,
          category,
          uploadedAt: response.uploadedAt, 
          uploadedBy,
          type,
          fileName: response.FileName,
          s3Url: response.S3Url,
          uploaded: true,
          selected: false
        } as StudentDocs)),
        catchError(error => {
          console.error('Upload failed:', error);
          return throwError(() => new Error('Failed to upload document'));
        })
      );
  }

  getDocumentsByStudentId(studentId: string): Observable<StudentDocs[]> {
    return this.http
      .get<{ id: string; studentId: string; category: string; uploadedAt: string; uploadedBy: string; documentType: string; fileName: string; s3Url: string }[]>(`${this.apiUrl}/student/${studentId}`)
      .pipe(
        map(docs => docs.map(doc => ({
          id: doc.id,
          studentId: doc.studentId,
          category: doc.category,
          uploadedAt: doc.uploadedAt,
          uploadedBy: doc.uploadedBy,
          type: doc.documentType,
          fileName: doc.fileName,
          s3Url: doc.s3Url,
          uploaded: true,
          selected: false
        } as StudentDocs))),
        catchError(error => {
          console.error('Error fetching documents:', error);
          return throwError(() => new Error('Failed to fetch documents'));
        })
      );
  }

  // Download a document by document ID
  downloadDocument(documentId: string): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/${documentId}`, { responseType: 'blob' });
  }

  // Delete a document by document ID
  deleteDocument(documentId: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${documentId}`);
  }
}