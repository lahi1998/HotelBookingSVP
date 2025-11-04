import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, tap } from 'rxjs';
import { BookingInterface } from '../interfaces/booking';
import { roomTypeDto } from '../interfaces/roomTypeDto';
import { roomDto } from '../interfaces/roomDto';

@Injectable({
  providedIn: 'root'
})
export class BookingService {
  baseUrl = "https://hotel-hyggely.dk/api";
  
  constructor(private httpClient: HttpClient) { }

  getRoomTypes(): Observable<roomTypeDto[]>{
    return this.httpClient.get<roomTypeDto[]>(`${this.baseUrl}/roomtypes`).pipe(
          tap((rooms: roomTypeDto[]) => {
            console.log('Fetched roomtypes:', rooms);
          }));
  }

  getRooms(startDate: string, endDate: string): Observable<any[]>{
    const params = new HttpParams()
    .set('FromDate', startDate)
    .set('ToDate', endDate);

    return this.httpClient.get<any[]>(`${this.baseUrl}/rooms/available`, { params }).pipe(
          map((items: any[]) => this.groupBy(items, 'roomTypeId')),
        tap((rooms: any[][]) => {
            console.log('Fetched rooms:', rooms);
          }));
  }

  createBooking(booking: BookingInterface): Observable<BookingInterface> {

    return this.httpClient.post<BookingInterface>(`${this.baseUrl}/bookings`, booking);
  }

  private groupBy(array: any[], key: string) {
    return array.reduce((result, currentItem) => {
      const groupKey = currentItem[key];
      if (!result[groupKey]) {
        result[groupKey] = [];
      }
      result[groupKey].push(currentItem);
      return result;
    }, {} as Record<string, any[]>);
  }
}
