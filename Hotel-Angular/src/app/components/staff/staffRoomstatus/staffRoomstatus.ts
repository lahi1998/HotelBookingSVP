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
    roomType: 1,
    bedAmount: 1,
    lastCleaned: new Date('2025-10-23T00:00:00'),
    roomStatus: true,
  },
];

