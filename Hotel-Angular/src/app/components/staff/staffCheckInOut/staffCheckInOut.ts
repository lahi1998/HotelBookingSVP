import { ChangeDetectorRef, Component, TemplateRef, ViewChild } from '@angular/core';
import { StaffService } from '../../../services/staffService';
import { MatTableDataSource } from '@angular/material/table';
import { Observer } from 'rxjs';
import { MatPaginator } from '@angular/material/paginator';
import { BookingListItemDto } from '../../../interfaces/bookingListItemDto';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BookingDetailsDto } from '../../../interfaces/bookingDetailsDto';
import { roomDto } from '../../../interfaces/roomDto';
import { UpdateBookingRequest } from '../../../interfaces/updateBookingRequest';
import { roomTypeDto } from '../../../interfaces/roomTypeDto';
import { BookingService } from '../../../services/bookingService';
import { CleaningScheduleDto } from '../../../interfaces/cleaningScheduleDto';
import { CreateCleaningScheduleRequest } from '../../../interfaces/createCleaningScheduleRequest';

@Component({
  selector: 'app-staffCheckInOut',
  standalone: false,
  templateUrl: './staffCheckInOut.html',
  styleUrl: './staffCheckInOut.css',
})
export class StaffCheckInOut {
  displayedColumnsCleaning: string[] = ['number', 'floor', 'cleaningDate', 'buttons'];
  displayedColumnsAvailable: string[] = ['number', 'floor', 'roomTypeName', 'bedAmount', 'buttons'];
  displayedColumnsList: string[] = ['fullName', 'email', 'phoneNumber', 'roomCount', 'startDate', 'endDate', 'buttons'];
  displayedColumnsDetails: string[] = ['number', 'floor', 'roomType', 'bedAmount', 'buttons'];
  filterValue: string = '';
  totalPrice: number = 0;
  /* cleaning schedule */
  CleaningForm!: FormGroup;
  DATACleaningSchedule: CleaningScheduleDto[] = [];
  dataSourceCleaningSchedule = new MatTableDataSource<CleaningScheduleDto>(this.DATACleaningSchedule);
  /* available room */
  DATAAvailableRooom: roomDto[] = [];
  dataSourceAvailableRooom = new MatTableDataSource<roomDto>(this.DATAAvailableRooom);
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
  roomTypes: roomTypeDto[] = []
  today: String = "";
  endDateMin: string = "";

  fetchFailed1: boolean = false
  fetchFailed2: boolean = false
  fetchFailed3: boolean = false
  fetchFailed4: boolean = false

  constructor(
    private fb: FormBuilder,
    private staffService: StaffService,
    private bookingService: BookingService,
    private cdr: ChangeDetectorRef
  ) { }

  @ViewChild('paginatorList') paginatorList!: MatPaginator;
  @ViewChild('paginatorRooms') paginatorRooms!: MatPaginator;
  @ViewChild('paginatorAvailable') paginatorAvailable!: MatPaginator;
  @ViewChild('paginatorCleaning') paginatorCleaning!: MatPaginator;

  ngOnInit() {
    this.bookingDetailsForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', Validators.required],
      phoneNumber: ['', [Validators.required, Validators.pattern(/^[0-9+\-\s()]{5,20}$/)]],
      startDate: ['', Validators.required],
      endDate: ['', Validators.required],
      comment: [],
      personCount: ['', Validators.required, Validators.min(1), Validators.max(10)],
    })

    this.CleaningForm = this.fb.group({
      roomId: ['', Validators.required],
      cleaningDate: ['', Validators.required],
    })

    this.getBookingListItems();
  }

  ngAfterViewInit() {
    this.dataSourceList.paginator = this.paginatorList;
    this.dataSourceRooms.paginator = this.paginatorRooms;
  }

  /* booking list item logic */

  /* search filter */
  searchFilter() {
    this.dataSourceList.filter = this.filterValue.trim().toLowerCase();
  }

  searchFilter2() {
    this.dataSourceAvailableRooom.filter = this.filterValue.trim().toLowerCase();
  }

  searchFilter3() {
    this.dataSourceCleaningSchedule.filter = this.filterValue.trim().toLowerCase();
  }

  getBookingListItems() {
    const observer: Observer<BookingListItemDto[]> = {
      next: (response) => {
        this.DATABookingListItem = Array.isArray(response) ? response : [];
        this.dataSourceList.data = this.DATABookingListItem;
      },
      error: (err) => {
        // console.error('booking List Item fetch failed:', err);
        this.fetchFailed1 = true;
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
        //console.log('Delete successful.', response);
        this.getBookingListItems();
      },
      error: (error) => {
        //console.error('Delete error.', error);
        alert("Sletning fejlede!")
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
        startDate: this.bookingDetailsForm.value.startDate,
        endDate: this.bookingDetailsForm.value.endDate,
        totalPrice: this.totalPrice,
        personCount: this.bookingDetailsForm.value.personCount,
        comment: this.bookingDetailsForm.value.comment,
        fullName: this.bookingDetailsForm.value.fullName,
        email: this.bookingDetailsForm.value.email,
        phoneNumber: this.bookingDetailsForm.value.phoneNumber,
        roomIds: this.roomIdsArray,
      };

      const observer: Observer<any> = {
        next: (response) => {
          //console.log('update successful.', response);
          this.getBookingListItems();
          this.editopen = false;
        },
        error: (error) => {
          //console.error('Create error.', error);
          alert('updatering fejlede!');
        },
        complete: () => {
          // optional cleanup or navigation
        },
      };

      this.staffService.putbookingdetails(updatedBookingDetailsDto).subscribe(observer);
    }
  }


  /*  open edit booking */
  OpenEditRow(id: number) {

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

        this.roomIdsArray = this.DATABookingDetailsRoom.map(room => room.id);

        //console.log('booking details fetched successfully', bookingDetail);
      },
      error: (err) => {
        //console.error('booking details fetch failed:', err);
        this.fetchFailed2 = true;
        alert("Henting fejlede!");
      },
      complete: () => {
        // optional cleanup or navigation
      },
    };

    this.staffService.getBookingDetails(id).subscribe(observer);
  }

  /* repops the array with only id's not matching the id, thats meant to be removed */
  DeleteBookedRoomRow(id: number) {
    /*  remove the room ID from the selected room list */
    this.roomIdsArray = this.roomIdsArray.filter(roomId => roomId !== id);

    /* remove the full room object from the booked rooms table */
    this.DATABookingDetailsRoom = this.DATABookingDetailsRoom.filter(room => room.id !== id);

    /* update the Material Table datasource */
    this.dataSourceRooms.data = [...this.DATABookingDetailsRoom];
    this.CalcNewPrice();

    //console.log('Room deleted from booking:', id);
  }

  CheckInOut(id: number) {

    console.log("here me lord", id)
    const observer: Observer<any> = {
      next: (response) => {
        //console.log('check successful.', response);
        this.OpenEditRow(id);
      },
      error: (error) => {
        //console.error('check error.', error);
        alert("Kunne ikke opdatere check ind/ud status.")
      },
      complete: () => {
        // optional cleanup or navigation
      },
    };

    this.staffService.checkInOut(id, this.checkStatus).subscribe(observer);

  }


  /* add room */
  /* open dialog logik and funtion calls */
  async OpenAddRoomDialog() {

    const startDateObj = new Date(this.startDate);
    const endDateObj = new Date(this.endDate);

    const startDateString = startDateObj.toISOString().split('T')[0];
    const endDateString = endDateObj.toISOString().split('T')[0];
    console.log(startDateString, " ", endDateString)

    await this.GetAvailableRooms(startDateString, endDateString);
  }

  /* adds id to roomids array */
  AddRoom(newid: number) {
    console.log(newid)
    console.log(this.DATAAvailableRooom)
    /* add the room ID to the array of selected rooms */
    this.roomIdsArray.push(newid);

    const roomToAdd = this.DATAAvailableRooom.find(room => room.id === newid);
    if (!roomToAdd) {
      //console.error('Room not found in DATAAvailableRooom for ID:', newid);
      return;
    }

    /* add it to the current booking room list */
    this.DATABookingDetailsRoom.push(roomToAdd);

    /* remove the room from available rooms */
    this.DATAAvailableRooom = this.DATAAvailableRooom.filter(room => room.id !== newid);

    /* update the table datasource's */
    this.dataSourceAvailableRooom.data = [...this.DATAAvailableRooom];
    this.dataSourceRooms.data = [...this.DATABookingDetailsRoom];
    this.CalcNewPrice();

    console.log(this.roomIdsArray);
  }

  /* gets a array of available rooms (id and roomtypeid)*/
  async GetAvailableRooms(startDate: string, endDate: string): Promise<void> {
    if (!startDate || !endDate) {
      this.DATAAvailableRooom = [];
      return;
    }

    const startDateAsDate = Date.parse(startDate);
    const endDateAsDate = Date.parse(endDate);

    if (isNaN(startDateAsDate) || isNaN(endDateAsDate)) {
      this.DATAAvailableRooom = [];
      return;
    }

    return new Promise<void>((resolve, reject) => {
      const roomObserver: Observer<roomDto[]> = {
        next: (response) => {
          this.DATAAvailableRooom = Array.isArray(response) ? response : [];
          this.dataSourceAvailableRooom.data = this.DATAAvailableRooom;

          this.dataSourceAvailableRooom.paginator = this.paginatorAvailable;
          resolve();
        },
        error: (error) => {
          //console.error('Room error', error);
          //alert('Available rooms fetch failed');
          this.fetchFailed3 = true;
          reject(error);
        },
        complete: () => {
        },
      };

      this.staffService.getAvailableRoomsDetailed(startDate, endDate).subscribe(roomObserver);
    });
  }

  /* calck totalPrice */
  async CalcNewPrice() {
    this.totalPrice = 0;
    const start = new Date(this.startDate);
    const end = new Date(this.endDate);

    /* calculate full days between dates */
    const days = Math.max(1, Math.ceil((end.getTime() - start.getTime()) / (1000 * 60 * 60 * 24)));

    /* gets return roomTypeDto[] */
    const roomTypes = await this.GetRoomTypes();

    /* loop through all booked rooms */
    for (const room of this.DATABookingDetailsRoom) {
      let pricePerDay = 0;

      /* find price from roomType */
      const foundType = roomTypes.find(rt => rt.name === room.roomTypeName);
      if (foundType) {
        pricePerDay = foundType.price;
      }
      else {
        //console.log("Could not find price.")
        alert("Kunne ikke finde pris");
      }

      this.totalPrice += pricePerDay * days;
    }

    //console.log(`Total price for ${days} days:`, this.totalPrice);
  }

  /* get room types */
  async GetRoomTypes(): Promise<roomTypeDto[]> {
    return new Promise((resolve, reject) => {
      const observer: Observer<roomTypeDto[]> = {
        next: (response) => resolve(response || []),
        error: (err) => {
          //console.error('Failed to fetch room types', err);
          alert("Kunne ikke hente værelses typer");
          reject();
        },
        complete: () => { },
      };
      this.bookingService.getRoomTypes().subscribe(observer);
    });
  }

  OnDateChanged(newStartDate: string, newEndDate: string) {
    this.startDate = newStartDate ? new Date(newStartDate) : new Date();
    this.endDate = newEndDate ? new Date(newEndDate) : new Date();

    this.CalcNewPrice();
  }

  /* cleaning */
  /* gets a array of cleaning scheduled for the rooms in the booking */
  async GetCleaningSchedule(bookingid: number): Promise<CleaningScheduleDto[]> {

    return new Promise((resolve, reject) => {
      const observer: Observer<CleaningScheduleDto[]> = {
        next: (response) => {
          this.DATACleaningSchedule = Array.isArray(response) ? response : [];
          this.dataSourceCleaningSchedule.data = this.DATACleaningSchedule;

          this.cdr.detectChanges();
          this.dataSourceCleaningSchedule.paginator = this.paginatorCleaning;
          resolve(response);
        },
        error: (err) => {
          //console.error('Failed to fetch cleaning schedule', err);
          this.fetchFailed4 = true;
          reject();
        },
        complete: () => {
          // optional cleanup or navigation
        },
      };

      this.staffService.getCleaningScheduleBooking(bookingid).subscribe(observer);
    });
  }

  /* repops the array with only id's not matching the id, thats meant to be removed */
  DeleteCleaningRow(id: number) {

    const observer: Observer<any> = {
      next: (response) => {
        //console.log('Delete successful.', response);
      },
      error: (error) => {
        console.error('Delete error.', error);
        alert("Sletning fejlede!");
      },
      complete: () => {
        // optional cleanup or navigation
      },
    };

    this.staffService.deleteCleaningSchedule(id).subscribe(observer);

    /* remove the CleaningSchedule object from the CleaningSchedule table */
    this.DATACleaningSchedule = this.DATACleaningSchedule.filter(CleaningSchedule => CleaningSchedule.id !== id);

    /* update the Material Table datasource */
    this.dataSourceCleaningSchedule.data = [...this.DATACleaningSchedule];
    

    console.log('Cleaning Schedule deleted :', id);
  }

  CreateCleaningSchedule(bookingid: number): void {
    if (this.CleaningForm.valid) {
      const newCleaning: CreateCleaningScheduleRequest = {
        roomId: this.CleaningForm.value.roomId,
        cleaningDate: this.CleaningForm.value.cleaningDate,
      };

      const observer: Observer<any> = {
        next: (response) => {
          //console.log('create successful.', response);
          this.GetCleaningSchedule(bookingid);

        },
        error: (error) => {
          //console.error('Create error.', error);
          alert('Opretning fejlede!');
        },
        complete: () => {
          // optional cleanup or navigation
        },
      };

      this.staffService.postCleaningScheduleBooking(newCleaning).subscribe(observer);
    }
  }
}

