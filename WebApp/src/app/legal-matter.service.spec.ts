import { TestBed } from '@angular/core/testing';
import { HttpTestingController } from '@angular/common/http/testing';

import { LegalMatterService } from './legal-matter.service';
import { httpTestProvidersWithInterceptors } from '../test-helpers';
import { environment } from '../environments/environment';

describe('LegalMatterService', () => {
  let service: LegalMatterService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        ...httpTestProvidersWithInterceptors,
        LegalMatterService
      ]
    });
    service = TestBed.inject(LegalMatterService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should use the API interceptor to prepend base URL', () => {
    const testData = [{ id: '1', title: 'Test Matter' }];
    
    service.getLegalMatters(1).subscribe(data => {
      expect(data).toBeDefined();
    });

    const req = httpMock.expectOne(`${environment.apiServerBase}/legalmatter?take=15&skip=0`);
    expect(req.request.method).toBe('GET');
    req.flush(testData);
  });
});
