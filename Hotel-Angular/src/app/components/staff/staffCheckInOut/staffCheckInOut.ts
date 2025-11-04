import { Component, ViewChild } from '@angular/core';
import { StaffService } from '../../../services/staffService';
import { MatTableDataSource } from '@angular/material/table';
import { Observer } from 'rxjs';
import { MatPaginator } from '@angular/material/paginator';
import { BookingListItemDto } from '../../../interfaces/bookingListItemDto';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BookingDetailsDto } from '../../../interfaces/bookingDetailsDto';
import { roomDto } from '../../../interfaces/roomDto';
import { UpdateBookingRequest } from '../../../interfaces/updateBookingRequest';

@Component({
  selector: 'app-staffCheckInOut',
  standalone: false,
  templateUrl: './staffCheckInOut.html',
  styleUrl: './staffCheckInOut.css',
})
export class StaffCheckInOut {
  displayedColumnsList: string[] = ['fullName', 'email', 'phoneNumber', 'roomCount', 'startDate', 'endDate', 'buttons'];
  displayedColumnsDetails: string[] = ['number', 'floor', 'roomType', 'bedAmount', 'buttons'];
  filterValue: string = '';
  totalPrice: number = 0;
  /* list */
  DATABookingListItem: BookingListItemDto[] = [];
  dataSourceList = new MatTableDataSource<BookingListItemDto>(this.DATABookingListItem);
  /* details */
  DATABookingDetails: BookingDetailsDto[] = [];
  DATABookingDetailsRoom: roomDto[] = [];
  dataSourceRooms = new MatTableDataSource<roomDto>(this.DATABookingDetailsRoom);
  roomIdsArray: number[] = this.DATABookingDetailsRoom.map(room => room.id);
  bookingDetailsForm!: FormGroup;
  bookingid: number = 0;
  /* check in/out status */
  checkStatus: string = '';
  statusText: string = '';

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
      personCount: ['', Validators.required],
      /* todo more is here just to tired to think of it*/
    })

    this.getBookingListItems();
  }

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  ngAfterViewInit() {
    this.dataSourceList.paginator = this.paginator;
  }

  /* booking list item logic */

  /* search filter */
  searchFilter() {
    this.dataSourceList.filter = this.filterValue.trim().toLowerCase();
  }

  getBookingListItems() {
    const observer: Observer<BookingListItemDto[]> = {
      next: (rooms) => {
        this.DATABookingListItem = Array.isArray(rooms) ? rooms : [];
        this.dataSourceList.data = this.DATABookingListItem;
        // console.log('booking List Item fetched successfully', rooms);
      },
      error: (err) => {
        // console.error('booking List Item fetch failed:', err);
      },
      complete: () => {
        // optional cleanup or navigation
      },
    };

    this.staffService.getBookingListItems().subscribe(observer);
  }

  DeletebookingRow(id: number) {

    const observer: Observer<any> = {
      next: (response) => {
        console.log('Delete successful.', response);
        this.getBookingListItems();
      },
      error: (error) => {
        console.error('Delete error.', error);
      },
      complete: () => {
        // optional cleanup or navigation
      },
    };

    this.staffService.deletebooking(id).subscribe(observer);
  }


  /* bookind detail / edit logik */

  onSubmit(): void {
    if (this.bookingDetailsForm.valid) {
      const updatedBookingDetailsDto: UpdateBookingRequest = {
        id: this.bookingid,
        fullName: this.bookingDetailsForm.value.fullName,
        email: this.bookingDetailsForm.value.email,
        phoneNumber: this.bookingDetailsForm.value.phoneNumber,
        startDate: this.bookingDetailsForm.value.startDate,
        endDate: this.bookingDetailsForm.value.endDate,
        comment: this.bookingDetailsForm.value.comment,
        totalPrice: this.totalPrice,
        personCount: this.bookingDetailsForm.value.personCount,
        roomIds: this.roomIdsArray,
      };

      /*const observer: Observer<any> = {
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

      this.staffService.putbookingdetails(updatedBookingDetailsDto).subscribe(observer);*/
    }
  }

  EditRow(id: number) {

    const observer: Observer<BookingDetailsDto> = {
      next: (bookingDetail) => {
        if (bookingDetail) {
          this.bookingDetailsForm.patchValue({
            fullName: bookingDetail.customer.fullName,
            email: bookingDetail.customer.email,
            phoneNumber: bookingDetail.customer.phoneNumber,
            startDate: bookingDetail.startDate
              ? new Date(bookingDetail.startDate).toISOString().slice(0, 10)
              : '',
            endDate: bookingDetail.endDate
              ? new Date(bookingDetail.endDate).toISOString().slice(0, 10)
              : '',
            comment: bookingDetail.comment,
          });
        }

        this.totalPrice = bookingDetail.totalPrice;
        this.checkStatus = bookingDetail.checkInStatus;
        if (bookingDetail.checkInStatus == 'NotCheckedIn') {
          this.statusText = "Tjek ind"
        }
        else { this.statusText = "Tjek ud" }
        this.bookingid = bookingDetail.id;

        this.DATABookingDetails = [bookingDetail];

        this.DATABookingDetailsRoom = bookingDetail.rooms;
        this.dataSourceRooms.data = this.DATABookingDetailsRoom;

        console.log('booking details fetched successfully', bookingDetail);
      },
      error: (err) => {
        console.error('booking details fetch failed:', err);
      },
      complete: () => {
        // optional cleanup or navigation
      },
    };

    this.staffService.getBookingDetails(id).subscribe(observer);
  }

  /* Repops the array with only id's not matching the id, thats meant to be removed */
  DeleteBookedRoomRow(id: number) {
    this.roomIdsArray = this.roomIdsArray.filter(roomId => roomId !== id);
  }

  CheckInOut(id: number) {

    console.log("here me lord", id)
    const observer: Observer<any> = {
      next: (response) => {
        console.log('check successful.', response);
        this.EditRow(id);
      },
      error: (error) => {
        console.error('check error.', error);
      },
      complete: () => {
        // optional cleanup or navigation
      },
    };

    this.staffService.checkInOut(id, this.checkStatus).subscribe(observer);

  }

}

