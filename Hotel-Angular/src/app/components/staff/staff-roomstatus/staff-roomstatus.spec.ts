import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StaffRoomstatus } from './staff-roomstatus';

describe('StaffRoomstatus', () => {
  let component: StaffRoomstatus;
  let fixture: ComponentFixture<StaffRoomstatus>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StaffRoomstatus]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StaffRoomstatus);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
