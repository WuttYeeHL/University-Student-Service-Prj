import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { Login } from '../../model/interface/login';

@Component({
  selector: 'app-login',
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  loginObj: Login = {
    loginCode: '',
    password: '',
  };

  router = inject(Router);

  constructor(private authService: AuthService) {}

  onLogin() {
    if (!this.loginObj.loginCode || !this.loginObj.password) {
      alert('Please enter both username and password');
      return;
    }
    this.authService.login(this.loginObj).subscribe({
      next: () => {
        this.router.navigate(['/home']);
      },
      error: (err) => {
        console.error('Login failed', err);
        alert('Invalid credentials');
      },
    });
  }
}
