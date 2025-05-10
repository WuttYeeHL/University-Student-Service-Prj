export interface Education {
  edu_Id?: number;
  student_Id?: number;
  yearFrom: string;
  yearTo: string;
  institution: string;
  qualification: string;
  location: string;
  completed: boolean;
  awaiting: boolean;
}