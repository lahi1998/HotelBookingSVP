import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { room } from '../interfaces/room';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  url: string = "https://localhost:8443/api/rooms"; // API endpoint

  constructor(private httpClient: HttpClient) { }

  postRoom(number: number, floor: number, roomType : number, bedAmount : number): Observable<room> {
    const roomData = {number, floor, roomType, bedAmount};

    // Send a POST request to the API for login
    return this.httpClient.post<room>(`${this.url}`, roomData)
  }


}
