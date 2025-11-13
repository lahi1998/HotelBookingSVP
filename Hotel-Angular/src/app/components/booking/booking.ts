import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { BookingService } from '../../services/bookingService';
import { Router } from '@angular/router';
import { Observer } from 'rxjs';
import { BookingInterface } from '../../interfaces/booking';
import { roomTypeDto } from '../../interfaces/roomTypeDto';
import { roomDto } from '../../interfaces/roomDto';
import { RoomTypeImageDto } from '../../interfaces/roomTypeImageDto';

//Move this to its own  interface file
interface ImageData {
  dataUrl: string;
}

export function customEmailValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const email = control.value;
    if (!email) return null;
    const regex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-z]{2,4}$/;
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
  imagesForCurrentRoomType: any[] = [];
  allImages: any[] = [];
  currentImageIndex = 0;
  roomTypes: any[] = [];
  roomsFromAPI: any[] = [];
  selectedRoomType: number = 0;
  roomCount: number = 0;
  today: String = "";
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
    const dayAfterTomorrow = new Date();
    dayAfterTomorrow.setDate(today.getDate() + 2);
    const todayString = today.toISOString().split('T')[0];

    this.today = todayString;
    const dayAfterTomorrowString = dayAfterTomorrow.toISOString().split('T')[0];

    this.getRoomTypes();
    this.getRooms(todayString, dayAfterTomorrowString);
    this.getImages();


    this.bookingForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, customEmailValidator()]],
      phoneNumber: ['', [Validators.required, Validators.pattern(/^[0-9+\-\s()]{5,20}$/)]],
      personCount: [2, [Validators.required, Validators.min(1)]],
      comment: [''],
      startDate: [todayString, Validators.required],
      endDate: [dayAfterTomorrowString, Validators.required]
    });

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

        //set initial value on roomtype
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

  getImages() {
    const observer: Observer<RoomTypeImageDto[]> = {
      next: (response) => {
        this.allImages = response;
        this.currentImageIndex = 0;
        if (this.allImages[this.selectedRoomType] != null) {
          this.imagesForCurrentRoomType = this.allImages[this.selectedRoomType]
        }
        else {
          this.imagesForCurrentRoomType = [];
        }
      },
      error: (error) => {
        console.error('Image error', error);
        alert('Kunne ikke hente billeder for værelsestyperne');
      },
      complete: () => { },
    };

    this.bookingService.getImages().subscribe(observer);
  }

  onRoomTypeChange(category: number | null) {
    let categoryAsNumber = Number(category);
    this.selectedRoomType = categoryAsNumber;
    this.roomCount = this.roomsFromAPI[categoryAsNumber]?.length || 0;
    this.roomTypePrice = this.roomTypes.find(a => a.id == categoryAsNumber)?.price;
    this.currentImageIndex = 0;
    if (this.allImages[categoryAsNumber] != null) {
      this.imagesForCurrentRoomType = this.allImages[categoryAsNumber]
    }
    else {
      this.imagesForCurrentRoomType = [];
    }
  }

  onSubmit(): void {
    if (this.bookingForm.invalid) {
      alert('Udfyld venligst alle de nødvendige felter');
      return;
    }

    this.createBooking();
  }

  private createBooking() {
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
        alert('Booking oprettet. Du vil snart modtage en bekræftelsesmail');

        const resetToday = new Date();
        const resetDayAfterTomorrow = new Date();
        resetDayAfterTomorrow.setDate(resetToday.getDate() + 2);
        const resetTodayString = resetToday.toISOString().split('T')[0];
        const resetDayAfterTomorrowString = resetDayAfterTomorrow.toISOString().split('T')[0];

        this.bookingForm.reset({
          fullName: '',
          email: '',
          phoneNumber: '',
          personCount: 2,
          comment: '',
          startDate: resetTodayString,
          endDate: resetDayAfterTomorrowString,
        });

        this.addedRooms = []
        this.totalPrice = 0;

      },
      error: (error) => {
        console.error('Booking error', error);
        alert('Kunne ikke oprette booking');
      },
      complete: () => { },
    };

    this.bookingService.createBooking(booking).subscribe(observer);
  }

  onDateChanged(startDate: string, endDate: string) {
    this.getRooms(startDate, endDate);
    this.addedRooms = [];
    this.calcTotalPrice();
  }

  addRoom(roomTypeId: number) {
    if (this.roomsFromAPI[roomTypeId].length <= 0)
      return;

    const room = this.roomsFromAPI[roomTypeId].shift();

    if (room) {
      const roomType = this.roomTypes.find(rt => rt.id == room.roomTypeId)
      const existing = this.addedRooms.find(r => r.roomTypeId === roomType.id);

      if (existing) {
        existing.count++;
        existing.roomIds.push(room.id);
      } else {
        this.addedRooms.push({
          roomTypeId,
          roomType,
          count: 1,
          roomIds: [room.id]
        });
      }
      if (roomTypeId == this.selectedRoomType) {
        this.roomCount--;
      }
    }
    this.calcTotalPrice();
  }

  removeRoom(roomTypeId: number) {
    const existing = this.addedRooms.find(r => r.roomTypeId === roomTypeId);
    if (!existing) return;

    const removedRoomId = existing.roomIds.pop();

    if (removedRoomId) {
      const room = { id: removedRoomId, roomTypeId };
      this.roomsFromAPI[roomTypeId].unshift(room);
    }

    existing.count--;

    if (existing.count <= 0) {
      this.addedRooms = this.addedRooms.filter(r => r.roomTypeId !== roomTypeId);
    }

    if (roomTypeId == this.selectedRoomType) {
      this.roomCount++;
    }

    this.calcTotalPrice();
  }

  getRoomCount(roomTypeId: number): number {
    const existing = this.addedRooms.find(r => r.roomTypeId === roomTypeId);
    return existing ? existing.count : 0;
  }

  //Image carousel
  prevImage() {
    if (this.currentImageIndex > 0) {
      this.currentImageIndex--;
    }
    else {
      this.currentImageIndex = this.allImages[this.selectedRoomType].length - 1;
    }
  }

  nextImage() {
    if (this.currentImageIndex < this.allImages[this.selectedRoomType].length - 1) {
      this.currentImageIndex++;
    }
    else {
      this.currentImageIndex = 0;
    }
  }
}