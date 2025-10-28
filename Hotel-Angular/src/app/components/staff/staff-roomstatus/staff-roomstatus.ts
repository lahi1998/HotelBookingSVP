import { Component, ViewChild} from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { roomType } from '../../../interfaces/room-type';
import { room } from '../../../interfaces/room';

@Component({
  selector: 'app-staff-roomstatus',
  standalone: false,
  templateUrl: './staff-roomstatus.html',
  styleUrl: './staff-roomstatus.css',
})
export class StaffRoomstatus {
  displayedColumns: string[] = ['number', 'floor', 'type', 'bedCount', 'buttons'];
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
  { number: 1, floor: 1, type: 'Enkelt', bedCount: 1 },
  { number: 2, floor: 2, type: 'Double', bedCount: 2 },
  { number: 3, floor: 1, type: 'Enkelt', bedCount: 1 },
  { number: 4, floor: 2, type: 'Double', bedCount: 2 },
  { number: 5, floor: 2, type: 'Konge', bedCount: 1 },
  { number: 6, floor: 2, type: 'Dronning', bedCount: 1 }
];

