import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { roomDto } from '../interfaces/roomDto';
import { CleaningScheduleDto } from '../interfaces/cleaningScheduleDto';

@Injectable({
  providedIn: 'root'
})
export class StaffService {
  /* Api endpoints */
  url: string = "https://hotel-hyggely.dk/api/rooms";
  url2: string = "https://hotel-hyggely.dk/api/cleaningschedules";

  constructor(private httpClient: HttpClient) { }

  /* get token */
  getToken() {
    const storedKey = sessionStorage.getItem('authKey');
    const token = sessionStorage.getItem(storedKey ? storedKey : '');
    return token;
  }

  /* staff room status */
  getRooms(): Observable<roomDto[]> {

    const token = this.getToken();
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    return this.httpClient.get<roomDto[]>(this.url, { headers }).pipe(
      tap((rooms: roomDto[]) => {
        console.log('Fetched rooms:', rooms);
      })
    );
  }


  /* staff cleaning schedule */
  getCleaningSchedule(): Observable<CleaningScheduleDto[]> {

    const token = this.getToken();
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    return this.httpClient.get<CleaningScheduleDto[]>(this.url2, { headers }).pipe(
      tap((cleaningSchedule: CleaningScheduleDto[]) => {
        console.log('Fetched cleaning schedule:', cleaningSchedule);
      })
    );
  }

  deleteCleaningSchedule(id: number): Observable<any> {

    const token = this.getToken();
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    return this.httpClient.delete<any>(`${this.url2}/${id}`, { headers });
  }


}
