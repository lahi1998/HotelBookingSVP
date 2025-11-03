import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { BookingService } from '../../services/bookingService';
import { Router } from '@angular/router';
import { Observable, Observer } from 'rxjs';
import { BookingInterface } from '../../interfaces/booking';
import { roomTypeDto } from '../../interfaces/roomTypeDto';
import { roomDto } from '../../interfaces/roomDto';

//Move this to its own  interface file
interface ImageData {
  dataUrl: string;
}

export function customEmailValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const email = control.value;
    if (!email) return null;
    const regex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-z]{2,4}$/;
    console.log("a");
    return regex.test(email) ? null : { invalidEmail: true };
  };
}

@Component({
  selector: 'app-booking',
  standalone: false,
  templateUrl: './booking.html',
  styleUrl: './booking.css',
})

export class Booking implements OnInit {
  bookingForm!: FormGroup;
  images: ImageData[] = [];
  currentImageIndex = 0;
  roomTypes: any[] = [];
  roomsFromAPI: any[] = [];
  selectedRoomType: number = 0;
  roomCount: number = 0;
  today: String = "";
  tomorrow: String = "";
  addedRooms: any[] = [];
  roomTypePrice: number = 0;
  selectedRoomTypeId: number | null = null;
  totalPrice: number = 0;
  endDateMin: string = "";


  constructor(
    private fb: FormBuilder,
    private bookingService: BookingService,
    private router: Router
  ) { }

  ngOnInit(): void {
    const today = new Date();
    const tomorrow = new Date();
    tomorrow.setDate(today.getDate() + 1);
    this.tomorrow = tomorrow.toISOString().split('T')[0];
    const dayAfterTomorrow = new Date();
    dayAfterTomorrow.setDate(today.getDate() + 2);
    const todayString = today.toISOString().split('T')[0];

    this.today = todayString;
    const dayAfterTomorrowString = dayAfterTomorrow.toISOString().split('T')[0];

    this.getRoomTypes();
    this.getRooms(todayString, dayAfterTomorrowString);

    this.bookingForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, customEmailValidator()]],
      phoneNumber: ['', [Validators.required, Validators.min(8)]],
      personCount: [2, [Validators.required, Validators.min(1)]],
      comment: [''],
      startDate: [todayString, Validators.required],
      endDate: [dayAfterTomorrowString, Validators.required]
    });

    this.images = [{ dataUrl: "/assets/logo.png" }, { dataUrl: "/assets/download.jpg" }]

    this.bookingForm.get('startDate')?.valueChanges.subscribe(() => {
    this.updateEndDateMinAndValue();
  });
  this.updateEndDateMinAndValue();

  }

  updateEndDateMinAndValue() {
  const start = this.bookingForm.get('startDate')?.value;
  const endControl = this.bookingForm.get('endDate');

  const baseDate = start ? new Date(start) : new Date();
  baseDate.setDate(baseDate.getDate() + 1);

  // Format to yyyy-MM-dd for the date input
  const minDateString = baseDate.toISOString().split('T')[0];
  this.endDateMin = minDateString;

  // Update endDate if it's null or before the new min date
  const currentEnd = endControl?.value ? new Date(endControl.value) : null;

  if (!currentEnd || currentEnd < baseDate) {
    endControl?.setValue(minDateString);
  }
}

  calcTotalPrice() {
    this.totalPrice = 0;
    const start = new Date(this.bookingForm.value.startDate);
    const end = new Date(this.bookingForm.value.endDate);

    const days =
      (end.getTime() - start.getTime()) / (1000 * 60 * 60 * 24);

    this.addedRooms.forEach(room => {
      this.totalPrice += room.roomType.price * room.count;
    });
    this.totalPrice *= days;
  }

  getRoomTypes() {
    const roomTypeObserver: Observer<roomTypeDto[]> = {
      next: (response) => {
        this.roomTypes = response;
        this.onRoomTypeChange(this.roomTypes[0].id);
        if (this.roomTypes.length > 0) {
          this.selectedRoomTypeId = this.roomTypes[0].id;
          this.selectedRoomType = this.roomTypes[0].id;
        }
      },
      error: (error) => {
        console.error('RoomType error', error);
        alert('Kunne ikke hente værelsestyperne');
      },
      complete: () => { },
    };

    this.bookingService.getRoomTypes().subscribe(roomTypeObserver);
  }

  getRooms(startDate: string, endDate: string) {
    if (startDate == "" || endDate == "") {
      this.roomsFromAPI = [];
      return;
    }

    const startDateAsDate = Date.parse(startDate);
    const endDateAsDate = Date.parse(startDate);

    if (isNaN(startDateAsDate) || isNaN(endDateAsDate)) {
      this.roomsFromAPI = [];
      return;
    }

    const roomObserver: Observer<any[]> = {
      next: (response) => {
        this.roomsFromAPI = response;
        this.roomCount = this.roomsFromAPI[this.selectedRoomType]?.length || 0;
      },
      error: (error) => {
        console.error('Room error', error);
        alert('Kunne ikke hente værelserne');
      },
      complete: () => { },
    };

    this.bookingService.getRooms(startDate, endDate).subscribe(roomObserver);
  }

  onRoomTypeChange(category: number | null) {
    let categoryAsNumber = Number(category);
    this.selectedRoomType = categoryAsNumber;
    this.roomCount = this.roomsFromAPI[categoryAsNumber]?.length || 0;
    this.roomTypePrice = this.roomTypes.find(a => a.id == categoryAsNumber)?.price;
  }

  onSubmit(): void {
    if (this.bookingForm.invalid) {
      alert('Udfyld venligst alle de nødvendige felter');
      return;
    }

    const booking: BookingInterface = {
      fullName: this.bookingForm.value.fullName,
      email: this.bookingForm.value.email,
      phoneNumber: this.bookingForm.value.phoneNumber,
      personCount: this.bookingForm.value.personCount,
      comment: this.bookingForm.value.comment,
      startDate: this.bookingForm.value.startDate,
      endDate: this.bookingForm.value.endDate,
      roomIds: this.addedRooms.flatMap(r => r.roomIds)
    }

    const observer: Observer<any> = {
      next: (response) => {
        console.log('Booking created', response);
        alert('Booking oprettet. Du vil snart modtage en bekræftelsesmail');
      },
      error: (error) => {
        console.error('Booking error', error);
        alert('Kunne ikke oprette booking');
      },
      complete: () => { },
    };

    this.bookingService.createBooking(booking).subscribe(observer);
  }

  prevImage() {
    if (this.currentImageIndex > 0) {
      this.currentImageIndex--;
    }
    else {
      this.currentImageIndex = this.images.length - 1;
    }
  }

  nextImage() {
    if (this.currentImageIndex < this.images.length - 1) {
      this.currentImageIndex++;
    }
    else {
      this.currentImageIndex = 0;
    }
  }

  onDateChanged(startDate: string, endDate: string) {
    this.getRooms(startDate, endDate);
    this.addedRooms = [];
    this.calcTotalPrice();
  }

  addRoom() {
    if (this.roomCount <= 0)
      return;

    const room = this.roomsFromAPI[this.selectedRoomType].shift();

    if (room) {
      const roomType = this.roomTypes.find(rt => rt.id == room.roomTypeId)
      const existing = this.addedRooms.find(r => r.roomTypeId === roomType.id);

      if (existing) {
        existing.count++;
        existing.roomIds.push(room.id);
      } else {
        this.addedRooms.push({
          roomTypeId: roomType.id,
          roomType,
          count: 1,
          roomIds: [room.id]
        });
      }
      this.roomCount--;
    }
    this.calcTotalPrice();
  }
}
