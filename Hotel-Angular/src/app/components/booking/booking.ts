import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BookingService } from '../../services/booking-service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-booking',
  standalone: false,
  templateUrl: './booking.html',
  styleUrl: './booking.css',
})
export class Booking implements OnInit {
  bookingForm!: FormGroup;
  rooms: String[] =  ["a", "a"];

  constructor(
    private fb: FormBuilder,
    private loginService: BookingService,
    private router: Router
  ) { }

ngOnInit(): void {
    this.bookingForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', Validators.required],
      phoneNumber: ['', Validators.required],
      personCount: [2, Validators.required],
      comment: ['', Validators.required],
      fromDate: ['', Validators.required],
      toDate: ['', Validators.required],
      roomType: ['', Validators.required],
    });
  }

onSubmit(){

}
}
