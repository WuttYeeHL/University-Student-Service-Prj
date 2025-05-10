import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { Login } from '../../model/interface/login';
import { HttpClient } from '@angular/common/http';
import { LOGIN_API_URL } from '../../constant/Constant';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private router: Router, private http: HttpClient) {}
  private _token: string | null = null;

  setAuthToken(token: string): void {
    this._token = token;
    sessionStorage.setItem('authToken', token);
  }

  getAuthToken(): string | null {
    if (this._token) return this._token;

    const token = sessionStorage.getItem('authToken');
    if (token) {
      this._token = token;
      return token;
    }
    return null;
  }

  isTokenExpired(token: string): boolean {
    const payload = JSON.parse(atob(token.split('.')[1]));
    const expiry = payload.exp * 1000;
    return Date.now() > expiry;
  }

  login(credentials: Login): Observable<{ token: string }> {
    return this.http.post<{ token: string }>(
      `${LOGIN_API_URL}/login`,
      credentials
    );
  }

  logout(): void {
    this._token = null;
    sessionStorage.removeItem('authToken');
    this.router.navigate(['/login']);
  }
}
