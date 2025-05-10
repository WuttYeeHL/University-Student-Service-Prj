import { Injectable } from '@angular/core';
import { Enrolment } from '../model/interface/enrolment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ENROLMENT_API_URL } from '../constant/Constant';

@Injectable({
  providedIn: 'root',
})
export class EnrolmentService {
  constructor(private http: HttpClient) {}

  getEnrolments(userId: string): Observable<Enrolment[]> {
    return this.http.get<Enrolment[]>(
      `${ENROLMENT_API_URL}/getEnrolmentByUserId`,
      {
        params: { userId: userId },
      }
    );
  }
}
