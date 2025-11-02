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
  rooms: String[] = ["a", "a"];
  images: ImageData[] = [];
  currentImageIndex = 0;
  roomTypes: any[] = [];
  roomsFromAPI: any[] = [];
  selectedRoomType: number = 0;
  roomCount: number = 0;


  constructor(
    private fb: FormBuilder,
    private bookingService: BookingService,
    private router: Router
  ) { }

  ngOnInit(): void {
    const roomTypeObserver: Observer<roomTypeDto[]> = {
      next: (response) => {
        this.roomTypes = response;
      },
      error: (error) => {
        console.error('RoomType error', error);
        alert('Kunne ikke hente værelsestyperne');
      },
      complete: () => { },
    };
    const roomObserver: Observer<any[]> = {
      next: (response) => {
        this.roomsFromAPI = response;
      },
      error: (error) => {
        console.error('Room error', error);
        alert('Kunne ikke hente værelserne');
      },
      complete: () => { },
    };
    this.bookingService.getRoomTypes().subscribe(roomTypeObserver);
    
    const today = new Date();
    
    const dayAfterTomorrow = new Date();
    const todayString = today.toISOString().split('T')[0];
    const dayAfterTomorrowString = dayAfterTomorrow.toISOString().split('T')[0];
    dayAfterTomorrow.setDate(today.getDate() + 2);
    this.bookingForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, customEmailValidator()]],
      phoneNumber: ['', [Validators.required, Validators.min(8)]],
      personCount: [2, [Validators.required, Validators.min(1)]],
      comment: [''],
      startDate: [todayString, Validators.required],
      endDate: [dayAfterTomorrowString, Validators.required]
    });
    this.bookingService.getRooms(todayString, dayAfterTomorrowString).subscribe(roomObserver);
    this.images = [{ dataUrl: "/assets/logo.png" }, { dataUrl: "/assets/download.jpg" }]
  }

  validateEmail(){

  }

  onRoomTypeChange(category: String) {
    let categoryAsNumber = Number(category);
    this.selectedRoomType = categoryAsNumber;
    this.roomCount = this.roomsFromAPI[categoryAsNumber]?.length || 0;
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
    }

    const observer: Observer<any> = {
      next: (response) => {
        console.log('Booking created', response);
        alert('Booking created!');
      },
      error: (error) => {
        console.error('Booking error', error);
        alert('Booking error');
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


}
