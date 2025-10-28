import { Component, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { room } from '../../../interfaces/room';

@Component({
  selector: 'app-staffRoomstatus',
  standalone: false,
  templateUrl: './staffRoomstatus.html',
  styleUrl: './staffRoomstatus.css',
})
export class StaffRoomstatus {
  displayedColumns: string[] = ['number', 'floor', 'type', 'bedCount', 'lastCleaned', 'roomStatus'];
  filterValue: string = '';
  dataSource = new MatTableDataSource<room>(DATA);

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  applyFilter() {
    this.dataSource.filter = this.filterValue.trim().toLowerCase();
  }


}

// --- Mock data ---
const DATA: room[] = [
  {
    number: 1,
    floor: 1,
    type: 'enkelt',
    bedCount: 1,
    lastCleaned: new Date('2025-10-23T00:00:00'),
    roomStatus: true,
  },
  {
    number: 2,
    floor: 1,
    type: 'double',
    bedCount: 2,
    lastCleaned: new Date('2025-10-22T09:30:00'),
    roomStatus: false,
  },
  {
    number: 3,
    floor: 1,
    type: 'enkelt',
    bedCount: 1,
    lastCleaned: new Date('2025-10-21T08:15:00'),
    roomStatus: true,
  },
  {
    number: 4,
    floor: 1,
    type: 'double',
    bedCount: 3,
    lastCleaned: new Date('2025-10-20T12:00:00'),
    roomStatus: false,
  },
  {
    number: 5,
    floor: 2,
    type: 'double',
    bedCount: 2,
    lastCleaned: new Date('2025-10-24T07:45:00'),
    roomStatus: true,
  },
  {
    number: 6,
    floor: 2,
    type: 'enkelt',
    bedCount: 1,
    lastCleaned: new Date('2025-10-23T10:00:00'),
    roomStatus: true,
  },
  {
    number: 7,
    floor: 2,
    type: 'double',
    bedCount: 2,
    lastCleaned: new Date('2025-10-22T11:20:00'),
    roomStatus: false,
  },
  {
    number: 8,
    floor: 2,
    type: 'double',
    bedCount: 3,
    lastCleaned: new Date('2025-10-19T09:00:00'),
    roomStatus: true,
  },
  {
    number: 9,
    floor: 3,
    type: 'enkelt',
    bedCount: 1,
    lastCleaned: new Date('2025-10-18T15:30:00'),
    roomStatus: true,
  },
  {
    number: 10,
    floor: 3,
    type: 'double',
    bedCount: 2,
    lastCleaned: new Date('2025-10-24T13:00:00'),
    roomStatus: false,
  },
  {
    number: 11,
    floor: 3,
    type: 'enkelt',
    bedCount: 1,
    lastCleaned: new Date('2025-10-20T10:45:00'),
    roomStatus: true,
  },
  {
    number: 12,
    floor: 3,
    type: 'double',
    bedCount: 2,
    lastCleaned: new Date('2025-10-21T14:10:00'),
    roomStatus: true,
  },
  {
    number: 13,
    floor: 4,
    type: 'double',
    bedCount: 3,
    lastCleaned: new Date('2025-10-23T06:00:00'),
    roomStatus: false,
  },
  {
    number: 14,
    floor: 4,
    type: 'double',
    bedCount: 2,
    lastCleaned: new Date('2025-10-22T07:30:00'),
    roomStatus: true,
  },
  {
    number: 15,
    floor: 4,
    type: 'enkelt',
    bedCount: 1,
    lastCleaned: new Date('2025-10-25T09:00:00'),
    roomStatus: true,
  },
  {
    number: 16,
    floor: 5,
    type: 'double',
    bedCount: 2,
    lastCleaned: new Date('2025-10-20T08:00:00'),
    roomStatus: false,
  },
  {
    number: 17,
    floor: 5,
    type: 'double',
    bedCount: 3,
    lastCleaned: new Date('2025-10-19T12:45:00'),
    roomStatus: true,
  },
  {
    number: 18,
    floor: 5,
    type: 'enkelt',
    bedCount: 1,
    lastCleaned: new Date('2025-10-18T10:20:00'),
    roomStatus: true,
  },
  {
    number: 19,
    floor: 5,
    type: 'double',
    bedCount: 2,
    lastCleaned: new Date('2025-10-24T11:15:00'),
    roomStatus: false,
  },
  {
    number: 20,
    floor: 5,
    type: 'enkelt',
    bedCount: 1,
    lastCleaned: new Date('2025-10-23T05:50:00'),
    roomStatus: true,
  },
];

