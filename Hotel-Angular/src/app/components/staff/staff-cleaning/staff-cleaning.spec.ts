import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StaffCleaning } from './staff-cleaning';

describe('StaffCleaning', () => {
  let component: StaffCleaning;
  let fixture: ComponentFixture<StaffCleaning>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StaffCleaning]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StaffCleaning);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
