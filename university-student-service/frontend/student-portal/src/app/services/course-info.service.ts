import { Injectable } from '@angular/core';
import { iCourseInfo, iQualificationInfo } from '../model/interface/courseinfo';
import { HttpClient } from '@angular/common/http';
import { COURSE_API_URL } from '../constant/Constant';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

 
export class CourseInfoService {

  constructor(private http: HttpClient) {}
  getQualificationInfo() {
   // return this.http.get<iQualificationInfo[]>(COURSE_API_URL);
    return this.http.get<iQualificationInfo[]>(`${COURSE_API_URL}/getQualifications`);
  }

  getQualificationById(id: number) {
    return this.http.get<iQualificationInfo>(`${COURSE_API_URL}/${id}`);
  }

  downloadCourseFile(code: string): Observable<Blob> {
  return this.http.get(code, { responseType: 'blob' });
}
  
}