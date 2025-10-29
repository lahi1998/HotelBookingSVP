import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StaffCheckInOut } from './staffCheckInOut';

describe('StaffCheckInOut', () => {
  let component: StaffCheckInOut;
  let fixture: ComponentFixture<StaffCheckInOut>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StaffCheckInOut]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StaffCheckInOut);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
