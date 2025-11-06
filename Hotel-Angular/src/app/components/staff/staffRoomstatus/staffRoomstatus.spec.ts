import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of, throwError } from 'rxjs';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { StaffRoomstatus } from './staffRoomstatus';
import { StaffService } from '../../../services/staffService';
import { roomDto } from '../../../interfaces/roomDto';

describe('StaffRoomstatus', () => {
  let component: StaffRoomstatus;
  let fixture: ComponentFixture<StaffRoomstatus>;
  let staffServiceMock: any;

  const mockRooms: roomDto[] = [
    { id:1, number: 101, floor: 1, roomTypeName: 'Enkelt', bedAmount: 1, lastCleaned: new Date('2025-11-01'), roomstatus: 'Ledigt' },
    { id:2, number: 102, floor: 1, roomTypeName: 'Dobbelt', bedAmount: 2, lastCleaned: new Date('2025-11-02'), roomstatus: 'Ledigt' },
  ];

  beforeEach(async () => {
    staffServiceMock = {
      getRooms: jasmine.createSpy('getRooms').and.returnValue(of(mockRooms))
    };

    await TestBed.configureTestingModule({
      declarations: [StaffRoomstatus],
      imports: [MatTableModule, MatPaginatorModule],
      providers: [
        { provide: StaffService, useValue: staffServiceMock }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(StaffRoomstatus);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch rooms and update dataSource', () => {
    component.getRooms();
    expect(staffServiceMock.getRooms).toHaveBeenCalled();
    expect(component.dataSource.data.length).toBe(mockRooms.length);
    expect(component.dataSource.data).toEqual(mockRooms);

    // Ekstra tjek for Date type
    component.dataSource.data.forEach((room, index) => {
      expect(room.lastCleaned instanceof Date).toBeTrue();
      expect(room.lastCleaned.getTime()).toBe(mockRooms[index].lastCleaned.getTime());
    });
  });

  it('should handle errors when fetching rooms', () => {
    staffServiceMock.getRooms.and.returnValue(throwError(() => new Error('Network error')));
    spyOn(window, 'alert');

    component.getRooms();

    expect(staffServiceMock.getRooms).toHaveBeenCalled();
    expect(window.alert).toHaveBeenCalledWith('Rooms fetch failed!');
  });
});
