import { Component, TemplateRef, ViewChild } from '@angular/core';
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
  DATABookingDetailsRoom: roomDto[] = [];
  dataSourceRooms = new MatTableDataSource<roomDto>(this.DATABookingDetailsRoom);
  roomIdsArray: number[] = this.DATABookingDetailsRoom.map(room => room.id);
  bookingDetailsForm!: FormGroup;
  bookingid: number = 0;
  startDate: Date = new Date();
  endDate: Date = new Date();
  checkStatus: string = '';
  statusText: string = '';
  editopen = false;

  constructor(
    private fb: FormBuilder,
    private staffService: StaffService,
  ) { }

  @ViewChild(MatPaginator) paginator!: MatPaginator;

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


  /* Edit booking */
  EditRow(id: number) {

    this.editopen = true;

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
            personCount: bookingDetail.personCount,
          });
        }

        this.totalPrice = bookingDetail.totalPrice;
        this.checkStatus = bookingDetail.checkInStatus;
        if (bookingDetail.checkInStatus == 'NotCheckedIn') {
          this.statusText = "Tjek ind"
        }
        else { this.statusText = "Tjek ud" }
        this.bookingid = bookingDetail.id;

        this.startDate = bookingDetail.startDate;
        this.endDate = bookingDetail.endDate;

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
    /*  Remove the room ID from the selected room list */
    this.roomIdsArray = this.roomIdsArray.filter(roomId => roomId !== id);

    /* Remove the full room object from the booked rooms table */
    this.DATABookingDetailsRoom = this.DATABookingDetailsRoom.filter(room => room.id !== id);

    /* Update the Material Table datasource */
    this.dataSourceRooms.data = [...this.DATABookingDetailsRoom];

    console.log('Room deleted from booking:', id);
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


  /* Add room */
  availableRooms: any[] = [];
  allRooms: roomDto[] = [];

  DATAAvailableRooom: roomDto[] = [];
  dataSourceAvailableRooom = new MatTableDataSource<roomDto>(this.DATAAvailableRooom);

  /* Open dialog logik and funtion calls */
  async openAddRoomDialog() {

    const startDateObj = new Date(this.startDate);
    const endDateObj = new Date(this.endDate);

    const startDateString = startDateObj.toISOString().split('T')[0];
    const endDateString = endDateObj.toISOString().split('T')[0];

    this.getAvailableRooms(startDateString, endDateString);
  }





  /* Adds id to roomids array */
  addRoom(newid: number) {
    /* Add the room ID to the array of selected rooms */
    this.roomIdsArray.push(newid);

    /* Remove the room from available rooms */
    this.availableRooms = this.availableRooms.filter(room => room.id !== newid);

    /* Find the full room object in allRomms */
    const roomToAdd = this.allRooms.find(room => room.id === newid);
    if (!roomToAdd) {
      console.error('Room not found in allRomms for ID:', newid);
      return;
    }

    /* Add it to the current booking room list */
    this.DATABookingDetailsRoom.push(roomToAdd);

    /* Update the table datasource */
    this.dataSourceRooms.data = [...this.DATABookingDetailsRoom];

    console.log('Room added:', roomToAdd);
  }

  /* Gets a array of available rooms (id and roomtypeid)*/
  getAvailableRooms(startDate: string, endDate: string){
    if (!startDate || !endDate) {
      this.availableRooms = [];
      return;
    }

    const startDateAsDate = Date.parse(startDate);
    const endDateAsDate = Date.parse(endDate);

    if (isNaN(startDateAsDate) || isNaN(endDateAsDate)) {
      this.availableRooms = [];
      return;
    }

      const roomObserver: Observer<any[]> = {
        next: (response) => {
          this.availableRooms = response;
        },
        error: (error) => {
          console.error('Room error', error);
          alert('Available rooms fetch failed');
        },
        complete: () => {
        },
      };

      this.staffService.getAvailableRoomsDetailed(startDate, endDate).subscribe(roomObserver);
  }



  // begreng også samlet pris med nye værelser.



}

