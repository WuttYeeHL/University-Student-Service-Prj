import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { Login } from '../../model/interface/login';
import { HttpClient } from '@angular/common/http';
import { LOGIN_API_URL } from '../../constant/Constant';
import { LoginUser } from '../../model/interface/loginUser';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private router: Router, private http: HttpClient) {}
  private userSubject = new BehaviorSubject<LoginUser | null>(null);
  user$ = this.userSubject.asObservable();

  fetchUserInfo(): Observable<LoginUser> {
    return this.http
      .get<LoginUser>(`${LOGIN_API_URL}/currentuser`, { withCredentials: true })
      .pipe(tap((user) => this.userSubject.next(user)));
  }

  get currentUser(): LoginUser | null {
    return this.userSubject.value;
  }

  login(credentials: Login): Observable<void> {
    return this.http.post<void>(`${LOGIN_API_URL}/login`, credentials, {
      withCredentials: true,
    });
  }

  logout(): Observable<void> {
    return this.http
      .post<void>(`${LOGIN_API_URL}/logout`, {}, { withCredentials: true })
      .pipe(tap(() => this.userSubject.next(null)));
  }
}
