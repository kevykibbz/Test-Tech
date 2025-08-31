import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';

@Injectable()
export class ApiInterceptor implements HttpInterceptor {

  constructor() {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Dirty regex from the stack
    const isAbsoluteUrl = new RegExp('^([a-z]+://|//|blob:)', 'i');

    return next.handle(req.clone({
      url: isAbsoluteUrl.test(req.url) ? req.url : `${environment.apiServerBase}/${req.url.replace(/^\/*/, '')}`,
    }));
  }
}
