import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { StudentDocService } from './student-documents-service';

describe('StudentDocService', () => {
  let service: StudentDocService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [StudentDocService]
    });
    service = TestBed.inject(StudentDocService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify(); 
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should upload a document', () => {
    const mockFile = new File(['test content'], 'test.pdf', { type: 'application/pdf' });
    const formData = new FormData();
    formData.append('file', mockFile);
    formData.append('category', 'PrivateScannedDocs');
    formData.append('type', 'Valid Visa');

    service.uploadDocument(mockFile, 'PrivateScannedDocs', 'Valid Visa').subscribe(response => {
      expect(response).toBeDefined();
    });

    const req = httpMock.expectOne('api/documents/upload');
    expect(req.request.method).toBe('POST');
    req.flush({ fileName: 'test.pdf', category: 'PrivateScannedDocs', type: 'Valid Visa' });
  });
});