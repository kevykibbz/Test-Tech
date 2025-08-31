import { TestBed } from '@angular/core/testing';

import { PeopleService } from './people.service';
import { httpTestProviders } from '../test-helpers';

describe('PeopleService', () => {
  let service: PeopleService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        ...httpTestProviders,
        PeopleService
      ]
    });
    service = TestBed.inject(PeopleService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
