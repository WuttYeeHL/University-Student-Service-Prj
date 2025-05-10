import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const token = sessionStorage.getItem('authToken');

  // Only include Authorization header for these API endpoints
  const protectedEndpoints = ['/api/Enrolment'];

  const shouldAttachToken = protectedEndpoints.some((path) =>
    req.url.includes(path)
  );

  if (shouldAttachToken && token) {
    const authReq = req.clone({
      setHeaders: { Authorization: `Bearer ${token}` },
    });
    return next(authReq);
  }

  return next(req);
};
