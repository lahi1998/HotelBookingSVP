import { Component, ViewChild, AfterViewInit } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { roomType } from '../../../interfaces/room-type';


@Component({
  selector: 'app-admin-room-type',
  standalone: false,
  templateUrl: './admin-room-type.html',
  styleUrls: ['./admin-room-type.css'],
})
export class AdminRoomType implements AfterViewInit {
  displayedColumns: string[] = ['id', 'type', 'pris', 'buttons'];
  filterValue: string = '';
  dataSource = new MatTableDataSource<roomType>(DATA);

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  applyFilter() {
    this.dataSource.filter = this.filterValue.trim().toLowerCase();
  }


}

// --- Mock data ---
const DATA: roomType[] = [
  { id: 1, type: 'Enkelt', price: 150 },
  { id: 2, type: 'Double', price: 250 },
  { id: 3, type: 'Konge', price: 350 },
  { id: 4, type: 'Dronning', price: 450 },
  { id: 5, type: 'Enkelt hav', price: 250 },
  { id: 6, type: 'Double hav', price: 350 }
];
