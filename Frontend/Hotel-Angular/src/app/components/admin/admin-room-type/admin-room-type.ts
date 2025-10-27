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
  dataSource = new MatTableDataSource<roomType>(DATA);

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }
}

// --- Mock data ---
const DATA: roomType[] = [
  { id: 1, Type: 'Enkelt', pris: 150},
  { id: 2, Type: 'Double', pris: 250},
  { id: 3, Type: 'Konge', pris: 350},
  { id: 4, Type: 'Dronning', pris: 450},
  { id: 5, Type: 'Enkelt hav', pris: 250},
  { id: 6, Type: 'Double hav', pris: 350}
];
