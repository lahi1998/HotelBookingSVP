import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminNav } from './admin-nav';

describe('AdminNav', () => {
  let component: AdminNav;
  let fixture: ComponentFixture<AdminNav>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdminNav]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminNav);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
