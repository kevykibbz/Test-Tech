import { TestBed } from '@angular/core/testing';

import { LogService } from './log.service';
import { httpTestProviders } from '../test-helpers';

describe('LogService', () => {
  let service: LogService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        ...httpTestProviders,
        LogService
      ]
    });
    service = TestBed.inject(LogService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
