import { TestBed } from '@angular/core/testing';

import { EventService } from './event.service';
import { httpTestProviders } from '../test-helpers';

describe('EventService', () => {
  let service: EventService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        ...httpTestProviders,
        EventService
      ]
    });
    service = TestBed.inject(EventService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
