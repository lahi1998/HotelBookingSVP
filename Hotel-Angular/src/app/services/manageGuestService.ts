import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { GuestDto } from '../interfaces/guestDto';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ManageGuestService {

  constructor(private httpClient: HttpClient) { }

  /* get token */
  getToken() {
    const storedKey = sessionStorage.getItem('authKey');
    const token = sessionStorage.getItem(storedKey ? storedKey : '');
    return token;
  }

  getGuests(): Observable<GuestDto[]> {

    const token = this.getToken();
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    return this.httpClient.get<GuestDto[]>("https://hotel-hyggely.dk/api/guest", { headers }).pipe(
      tap((guests: GuestDto[]) => {
        console.log('Fetched guests:', guests);
      })
    );
  }

  deleteGuest(id: number): Observable<any> {

    const token = this.getToken();
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    return this.httpClient.delete<any>(`https://hotel-hyggely.dk/api/guest/${id}`, { headers });
  }
}
