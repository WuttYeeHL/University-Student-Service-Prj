import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { Login } from '../model/interface/login';
import { HttpClient } from '@angular/common/http';
import { LOGIN_API_URL } from '../constant/Constant';
import { LoginUser } from '../model/interface/loginUser';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private router: Router, private http: HttpClient) {}

  private userSubject = new BehaviorSubject<LoginUser | null>(null);
  user$ = this.userSubject.asObservable();

  fetchUserInfo(): Observable<LoginUser> {
    return this.http
      .get<LoginUser>(`${LOGIN_API_URL}/currentuser`)
      .pipe(tap((user) => this.userSubject.next(user)));
  }

  get currentUser(): LoginUser | null {
    return this.userSubject.value;
  }

  login(credentials: Login): Observable<any> {
    return this.http
      .post<{ token: string }>(`${LOGIN_API_URL}/login`, credentials)
      .pipe(
        tap((res) => {
          document.cookie = `token=${res.token}; path=/; max-age=3600`;
        })
      );
  }

  logout(): void {
    document.cookie = 'token=; path=/; expires=Thu, 01 Jan 1970 00:00:00 UTC;';
    this.userSubject.next(null);
    this.router.navigate(['/login']);
  }
}
