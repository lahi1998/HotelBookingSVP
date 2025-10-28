import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StaffNav } from './staffNav';

describe('StaffNav', () => {
  let component: StaffNav;
  let fixture: ComponentFixture<StaffNav>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StaffNav]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StaffNav);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
