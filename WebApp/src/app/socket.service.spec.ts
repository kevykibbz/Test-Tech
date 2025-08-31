import { TestBed } from '@angular/core/testing';

import { SocketService } from './socket.service';
import { httpTestProviders } from '../test-helpers';

describe('SocketService', () => {
  let service: SocketService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        ...httpTestProviders,
        SocketService
      ]
    });
    service = TestBed.inject(SocketService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
