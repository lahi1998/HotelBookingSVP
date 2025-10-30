import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { BookingInterface } from '../interfaces/booking';
import { RoomTypeDto } from '../interfaces/roomTypeDto';

@Injectable({
  providedIn: 'root'
})
export class BookingService {
  baseUrl = "https://hotel-hyggely.dk/api";
  
  constructor(private httpClient: HttpClient) { }

  getRoomTypes(): Observable<RoomTypeDto[]>{
    return this.httpClient.get<RoomTypeDto[]>(`${this.baseUrl}/roomtypes`).pipe(
          tap((rooms: RoomTypeDto[]) => {
            console.log('Fetched roomtypes:', rooms);
          }));
  }

  getRooms(){

  }

  createBooking(booking: BookingInterface): Observable<BookingInterface> {

    return this.httpClient.post<BookingInterface>(`${this.baseUrl}/bookings`, booking);
  }

  getPrices(){

  }
}
