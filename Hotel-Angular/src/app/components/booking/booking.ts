import { Component } from '@angular/core';

@Component({
  selector: 'app-booking',
  standalone: false,
  templateUrl: './booking.html',
  styleUrl: './booking.css',
})
export class Booking {
fullName: String = "";
email: String = "";
phoneNumber: String = "";
personAmount: number = 1;
comment: String = "";
fromDate: Date = new Date();
toDate: Date = new Date();
roomType: String = "";

onSubmit(){

}
}
