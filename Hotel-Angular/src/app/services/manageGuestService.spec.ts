import { TestBed } from '@angular/core/testing';

import { ManageGuestService } from './manageGuestService';

describe('ManageGuestService', () => {
  let service: ManageGuestService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ManageGuestService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
