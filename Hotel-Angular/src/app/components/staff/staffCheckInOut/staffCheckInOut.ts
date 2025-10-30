import { Component, ViewChild } from '@angular/core';
import { StaffService } from '../../../services/staffService';
import { MatTableDataSource } from '@angular/material/table';
import { Observer } from 'rxjs';
import { MatPaginator } from '@angular/material/paginator';
import { BookingListItemDto } from '../../../interfaces/bookingListItemDto';

@Component({
  selector: 'app-staffCheckInOut',
  standalone: false,
  templateUrl: './staffCheckInOut.html',
  styleUrl: './staffCheckInOut.css',
})
export class StaffCheckInOut {
  displayedColumns: string[] = ['number', 'floor', 'roomType', 'bedAmount', 'buttons'];
  filterValue: string = '';
  DATABookingListItem: BookingListItemDto[] = [];
  dataSource = new MatTableDataSource<BookingListItemDto>(this.DATABookingListItem);

  // Image handling properties
  images: ImageData[] = [];
  currentImageIndex = 0;

  constructor(
    private staffService: StaffService,
  ) { }

  ngOnInit() {

    /*this.getBookingListItems();*/
  }

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }
  
  /* search filter */
  searchFilter() {
    this.dataSource.filter = this.filterValue.trim().toLowerCase();
  }

  getBookingListItems() {
    const observer: Observer<BookingListItemDto[]> = {
      next: (rooms) => {
        this.DATABookingListItem = Array.isArray(rooms) ? rooms : [];
        this.dataSource.data = this.DATABookingListItem;
        console.log('booking List Item fetched successfully', rooms);
        alert('booking ListItem fetched!');
      },
      error: (err) => {
        console.error('booking List Item fetch failed:', err);
        alert('booking List Item fetch failed!');
      },
      complete: () => {
        // optional cleanup or navigation
      },
    };

    this.staffService.getBookingListItems().subscribe(observer);
  }

}

