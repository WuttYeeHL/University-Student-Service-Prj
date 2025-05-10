import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from './auth-guard.service';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const token = authService.getAuthToken();

  if (token && !authService.isTokenExpired(token)) {
    return true;
  } else {
    router.navigate(['/login']);
    return false;
  }
};
