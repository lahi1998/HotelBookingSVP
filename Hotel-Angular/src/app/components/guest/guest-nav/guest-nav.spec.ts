import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GuestNav } from './guest-nav';

describe('GuestNav', () => {
  let component: GuestNav;
  let fixture: ComponentFixture<GuestNav>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [GuestNav]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GuestNav);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
