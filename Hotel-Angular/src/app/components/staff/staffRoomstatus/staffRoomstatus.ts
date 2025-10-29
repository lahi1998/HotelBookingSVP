import { Component, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { RoomStatus } from '../../../interfaces/room-status';

@Component({
  selector: 'app-staffRoomstatus',
  standalone: false,
  templateUrl: './staffRoomstatus.html',
  styleUrl: './staffRoomstatus.css',
})
export class StaffRoomstatus {
  displayedColumns: string[] = ['number', 'floor', 'type', 'bedCount', 'lastCleaned', 'roomStatus'];
  filterValue: string = '';
  dataSource = new MatTableDataSource<RoomStatus>(DATA);

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  applyFilter() {
    this.dataSource.filter = this.filterValue.trim().toLowerCase();
  }


}

// --- Mock data ---
const DATA: RoomStatus[] = [
  {
    number: 1,
    floor: 1,
    roomType: 1,
    bedAmount: 1,
    lastCleaned: '2024-01-01',
    roomStatus: true,
  },
];

