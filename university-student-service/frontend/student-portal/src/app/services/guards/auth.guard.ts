import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../auth.service';
import { firstValueFrom } from 'rxjs';

export const authGuard: CanActivateFn = async (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  try {
    const user = await firstValueFrom(authService.fetchUserInfo());

    if (user) {
      return true;
    }
  } catch (err) {
    console.error('Auth guard error:', err);
  }

  router.navigate(['/login']);
  return false;
};
