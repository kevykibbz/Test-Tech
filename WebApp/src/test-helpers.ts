import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { provideNoopAnimations } from '@angular/platform-browser/animations';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { ApiInterceptor } from './app/api.interceptor';

/**
 * Common test providers for Angular 20 standalone components and services
 */
export const commonTestProviders = [
  provideHttpClient(withInterceptorsFromDi()),
  provideHttpClientTesting(),
  provideRouter([]),
  provideNoopAnimations(), // Use noop animations instead of full animations
  {
    provide: HTTP_INTERCEPTORS,
    useClass: ApiInterceptor,
    multi: true
  }
];

/**
 * Test providers specifically for services that need HTTP client
 */
export const httpTestProviders = [
  provideHttpClient(),
  provideHttpClientTesting(),
];

/**
 * Test providers specifically for services that need HTTP client with interceptors
 */
export const httpTestProvidersWithInterceptors = [
  provideHttpClient(withInterceptorsFromDi()),
  provideHttpClientTesting(),
  {
    provide: HTTP_INTERCEPTORS,
    useClass: ApiInterceptor,
    multi: true
  }
];

/**
 * Test providers for components that need routing
 */
export const routingTestProviders = [
  provideRouter([]),
  provideNoopAnimations(), // Use noop animations instead of full animations
];
