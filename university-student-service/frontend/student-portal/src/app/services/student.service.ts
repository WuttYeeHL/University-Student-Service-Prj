import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Student } from '../model/interface/student';
import { STUDENT_API_URL } from '../constant/Constant';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class StudentService {

  private apiUrl = STUDENT_API_URL;

  constructor(private http: HttpClient) {}

  getStudentByUserId(userId: number): Observable<Student> {
    return this.http.get<Student>(`${this.apiUrl}/Students/user/${userId}`)
  }

  updateStudent(student: Student): Observable<any> {
    return this.http.put(this.apiUrl + '/Students', student);
  }
}
