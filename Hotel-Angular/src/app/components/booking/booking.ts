import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BookingService } from '../../services/booking-service';
import { Router } from '@angular/router';

//Move this to its own  interface file
interface ImageData {
  dataUrl: string;
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
  


  constructor(
    private fb: FormBuilder,
    private loginService: BookingService,
    private router: Router
  ) { }

  ngOnInit(): void {
    const today = new Date();
  const dayAfterTomorrow = new Date()
  dayAfterTomorrow.setDate(today.getDate() + 2);
    this.bookingForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', Validators.required],
      phoneNumber: ['', Validators.required],
      personCount: [2, Validators.required],
      comment: ['', Validators.required],
      fromDate: [today.toISOString().split('T')[0], Validators.required],
      toDate: [dayAfterTomorrow.toISOString().split('T')[0], Validators.required],
      roomType: ['', Validators.required],
    });

    this.images = [{dataUrl: "/assets/logo.png"}, {dataUrl: "/assets/download.jpg"} ]

  }

  onSubmit() {

  }

  prevImage() {
    if (this.currentImageIndex > 0) {
      this.currentImageIndex--;
    }
    else{
      this.currentImageIndex = this.images.length - 1;
    }
  }

  nextImage() {
    if (this.currentImageIndex < this.images.length - 1) {
      this.currentImageIndex++;
    }
    else{
      this.currentImageIndex = 0;
    }
  }


}
