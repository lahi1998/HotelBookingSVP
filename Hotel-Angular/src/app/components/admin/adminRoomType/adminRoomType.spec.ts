import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminRoomType } from './adminRoomType';

describe('AdminRoomType', () => {
  let component: AdminRoomType;
  let fixture: ComponentFixture<AdminRoomType>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdminRoomType]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminRoomType);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
