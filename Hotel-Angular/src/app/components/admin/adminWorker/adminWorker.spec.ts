import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminWorker } from './adminWorker';

describe('AdminWorker', () => {
  let component: AdminWorker;
  let fixture: ComponentFixture<AdminWorker>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdminWorker]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminWorker);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
