import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { StudentDocService } from '../../services/student-documents-service';
import { StudentDocs } from '../../model/interface/studentDocs';
import { AuthService } from '../../services/guards/auth-guard.service'; 

@Component({
  selector: 'app-student-documents',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './student-documents.component.html',
  styleUrls: ['./student-documents.component.scss']
})

export class StudentDocumentsComponent implements OnInit {
  documents: StudentDocs[] = []; 

  constructor(
    private studentDocService: StudentDocService,
    private authService: AuthService
  ) {}

  get isAnyDocumentSelected(): boolean {
    const result = this.documents.some(d => d.selected);
    console.log('isAnyDocumentSelected:', result, 'Selected documents:', this.documents.filter(d => d.selected));
    return result;
  }

  ngOnInit(): void {
    const user = this.authService.currentUser;
    if (!user?.userId) {
      console.error('No user logged in');
       return;
     }
    this.loadDocuments(user.userId);
  }

  private loadDocuments(studentId: string): void {
    this.studentDocService.getDocumentsByStudentId(studentId).subscribe({
      next: (docs) => {
        this.documents = docs.map(doc => ({
          ...doc,
          selected: false,
          uploaded: true
        }));
      },
      error: (err) => {
        console.error('Error fetching documents:', err);
        alert(`Failed to load documents: ${err.message || 'Unknown error'}`);
      }
    });
  }

  addDocument(): void {
    const user = this.authService.currentUser;
    if (!user?.userId || !user?.username) {
      console.error('No user logged in or missing user details');
      alert('You must be logged in to upload a document.');
      return;
    }

    const userId = user.userId;
    const uploadedBy = user.username;
    const input = document.createElement('input');
    input.type = 'file';
    input.accept = '.pdf,.jpg,.png'; // Restrict file types
   
    input.onchange = (e) => {
      const file = (e.target as HTMLInputElement).files?.[0];
      if (!file) {
        console.warn('No file selected.');
        alert('Please select a file.');
        return;
      }

      // Validate file type and size
      const allowedTypes = ['application/pdf', 'image/jpeg', 'image/png', 'application/msword'];
      if (!allowedTypes.includes(file.type)) {
        alert('Invalid file type. Only PDF, JPEG, and PNG are allowed.');
        return;
      }
      if (file.size > 5 * 1024 * 1024) {
        alert('File size exceeds 5MB limit.');
        return;
      }
      console.log('Uploading file:', file.name, file.type, file.size);

        this.studentDocService.uploadDocument(userId, 'Scanned Documents', uploadedBy, 'Other', file).subscribe({
          next: (response) => {
            console.log('Upload response:', response);
              this.documents.push({
                id: response.id,
                studentId: userId,
                category: 'Scanned Documents',
                uploaded: true,
                uploadedAt: response.uploadedAt,
                uploadedBy: response.uploadedBy, 
                type: 'Other',
                fileName: response.fileName,
                selected: false
              });
              alert('Document uploaded successfully!');
              this.loadDocuments(userId); // Refresh document list
          },
          error: (err) => console.error('Error uploading document:', err)
        });
    };
    input.click();
  }

  downloadSelected(): void {
    const selectedDocs = this.documents.filter(d => d.selected);
    console.log('Selected documents:', selectedDocs);
    selectedDocs.forEach(doc => {
      if (doc.id) {
        this.studentDocService.downloadDocument(doc.id).subscribe(blob => {
          const url = window.URL.createObjectURL(blob);
          const a = document.createElement('a');
          a.href = url;
          a.download = doc.fileName || 'document';
          a.click();
          window.URL.revokeObjectURL(url);
        });
      }
    });
    console.log('Downloading:', selectedDocs);
  }

  deleteSelected(): void {
    const selectedDocs = this.documents.filter(d => d.selected);
    selectedDocs.forEach(doc => {
      if (doc.id) {
        this.studentDocService.deleteDocument(doc.id).subscribe({
          next: () => {
            this.documents = this.documents.filter(d => d.id !== doc.id);
          },
          error: (err) => console.error('Error deleting document:', err)
        });
      }
    });
  }
}
