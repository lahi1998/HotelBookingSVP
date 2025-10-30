import { Component, ViewChild } from '@angular/core';
import { StaffService } from '../../../services/staffService';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { roomDto } from '../../../interfaces/roomDto';
import { Observer } from 'rxjs';

@Component({
  selector: 'app-staffCleaning',
  standalone: false,
  templateUrl: './staffCleaning.html',
  styleUrl: './staffCleaning.css',
})
export class StaffCleaning {
  displayedColumns: string[] = ['number', 'floor', 'type', 'bedCount', 'lastCleaned', 'roomStatus'];
  filterValue: string = '';
  DATA: roomDto[] = [];
  dataSource = new MatTableDataSource<roomDto>(this.DATA);

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
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
        console.log('Rooms fetched successfully', rooms);
        alert('Rooms fetched!');
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
