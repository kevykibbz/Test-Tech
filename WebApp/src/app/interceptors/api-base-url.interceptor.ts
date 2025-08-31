import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable()
export class ApiBaseUrlInterceptor implements HttpInterceptor {

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Only intercept requests that start with '/api' to avoid affecting external URLs
    if (req.url.startsWith('/api')) {
      const apiReq = req.clone({
        url: `${environment.apiServerBase}${req.url}`
      });
      return next.handle(apiReq);
    }
    
    // For all other requests, pass through unchanged
    return next.handle(req);
  }
}
