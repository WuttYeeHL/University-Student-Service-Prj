export interface StudentDocs {
    id: string;
    studentId: string;
    category: string;
    uploadedAt: string;
    uploadedBy: string;
    type: string;
    fileName: string;
    s3Url?: string; 
    uploaded?: boolean;
    selected?: boolean;
}
