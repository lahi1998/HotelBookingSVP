import { Component, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Observer } from 'rxjs';
import { roomDto } from '../../../interfaces/roomDto';
import { StaffService } from '../../../services/staffService';

@Component({
  selector: 'app-staffRoomstatus',
  standalone: false,
  templateUrl: './staffRoomstatus.html',
  styleUrl: './staffRoomstatus.css',
})
export class StaffRoomstatus {
  displayedColumns: string[] = ['number', 'floor', 'type', 'bedAmount', 'lastCleaned', 'roomStatus'];
  filterValue: string = '';
  DATA: roomDto[] = [];
  dataSource = new MatTableDataSource<roomDto>(this.DATA);

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.getRooms()
  }

  constructor(
    private staffService: StaffService,
  ) { }

  /* search filter */
  searchFilter() {
    this.dataSource.filter = this.filterValue.trim().toLowerCase();
  }

  getRooms() {
    const observer: Observer<roomDto[]> = {
      next: (rooms) => {
        this.DATA = Array.isArray(rooms) ? rooms : [];
        this.dataSource.data = this.DATA;
      },
      error: (err) => {
        console.error('Rooms fetch failed:', err);
        alert('Rooms fetch failed!');
      },
      complete: () => {
        // optional cleanup or navigation
      },
    };

    this.staffService.getRooms().subscribe(observer);
  }


}


