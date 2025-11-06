import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GuestBooking } from './guest-booking';

describe('GuestBooking', () => {
  let component: GuestBooking;
  let fixture: ComponentFixture<GuestBooking>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [GuestBooking]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GuestBooking);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
