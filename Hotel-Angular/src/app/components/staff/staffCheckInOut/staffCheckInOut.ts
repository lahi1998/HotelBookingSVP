import { Component, ViewChild } from '@angular/core';
import { StaffService } from '../../../services/staffService';
import { MatTableDataSource } from '@angular/material/table';
import { Observer } from 'rxjs';
import { MatPaginator } from '@angular/material/paginator';
import { BookingListItemDto } from '../../../interfaces/bookingListItemDto';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BookingDetailsDto } from '../../../interfaces/bookingDetailsDto';

@Component({
  selector: 'app-staffCheckInOut',
  standalone: false,
  templateUrl: './staffCheckInOut.html',
  styleUrl: './staffCheckInOut.css',
})
export class StaffCheckInOut {
  displayedColumns: string[] = ['fullName', 'email', 'phoneNumber', 'roomCount', 'startDate', 'endDate', 'buttons'];
  filterValue: string = '';
  DATABookingListItem: BookingListItemDto[] = [];
  dataSource = new MatTableDataSource<BookingListItemDto>(this.DATABookingListItem);
  bookingDetailsForm!: FormGroup;
  bookingid: number = 0;

  constructor(
    private fb: FormBuilder,
    private staffService: StaffService,
  ) { }

  ngOnInit() {

    this.bookingDetailsForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', Validators.required],
      phoneNumber: ['', Validators.required],
      startDate: ['', Validators.required],
      endDate: ['', Validators.required],
      comment: [],
      /* todo more is here just to tired to think of it*/
    })

    this.getBookingListItems();
  }

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  /* search filter */
  searchFilter() {
    this.dataSource.filter = this.filterValue.trim().toLowerCase();
  }




  onSubmit(): void {
    /* if (this.bookingDetailsForm.valid) {
      const updatedBookingDetaulsDto: BookingDetailsDto = {
        id: this.bookingid,
        startDate: this.bookingDetailsForm.startDate,
        endDate: 
        status: 
        price: 
        personCount: 
        comment: 

        };

      const observer: Observer<any> = {
        next: (response) => {
          console.log('Create successful.', response);
          alert('Create successful!');

        },
        error: (error) => {
          console.error('Create error.', error);
          alert('Create error!');
        },
        complete: () => {
          // optional cleanup or navigation
        },
      };

      this.staffService.putbookingdetails(updatedBookingDetaulsDto).subscribe(observer); 
    } */
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

