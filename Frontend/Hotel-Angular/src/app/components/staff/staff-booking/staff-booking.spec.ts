import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StaffBooking } from './staff-booking';

describe('StaffBooking', () => {
  let component: StaffBooking;
  let fixture: ComponentFixture<StaffBooking>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StaffBooking]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StaffBooking);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
