import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, tap } from 'rxjs';
import { BookingInterface } from '../interfaces/booking';
import { roomTypeDto } from '../interfaces/roomTypeDto';
import { RoomTypeImageDto } from '../interfaces/roomTypeImageDto';
import { roomDto } from '../interfaces/roomDto';

@Injectable({
  providedIn: 'root'
})
export class BookingService {
  baseUrl = "https://hotel-hyggely.dk/api";

  constructor(private httpClient: HttpClient) { }

  getRoomTypes(): Observable<roomTypeDto[]> {
    return this.httpClient.get<roomTypeDto[]>(`${this.baseUrl}/roomtypes`).pipe(
      tap((rooms: roomTypeDto[]) => {
        console.log('Fetched roomtypes:', rooms);
      }));
  }

  getRooms(startDate: string, endDate: string): Observable<any[]> {
    const params = new HttpParams()
      .set('FromDate', startDate)
      .set('ToDate', endDate);
      debugger;

    return this.httpClient.get<any[]>(`${this.baseUrl}/rooms/available`, { params }).pipe(
      //group items by roomtype id
      map((items: any[]) => this.groupBy(items, 'roomTypeId')));
  }

  getImages() {
    return this.httpClient.get<RoomTypeImageDto[]>(`${this.baseUrl}/roomtypes/images`).pipe(
      //group items by roomtype id
      map((items: RoomTypeImageDto[]) => this.groupBy(items, 'roomTypeId')));
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
