import { TestBed } from '@angular/core/testing';

import { FamilyMemberService } from './family-member.service';

describe('FamilyMemberService', () => {
  let service: FamilyMemberService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FamilyMemberService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
