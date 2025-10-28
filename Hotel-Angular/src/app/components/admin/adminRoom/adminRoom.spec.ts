import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminRoom } from './adminRoom';

describe('AdminRoom', () => {
  let component: AdminRoom;
  let fixture: ComponentFixture<AdminRoom>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdminRoom]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminRoom);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
