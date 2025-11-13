import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageGuest } from './manageGuest';

describe('ManageGuest', () => {
  let component: ManageGuest;
  let fixture: ComponentFixture<ManageGuest>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ManageGuest]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManageGuest);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
