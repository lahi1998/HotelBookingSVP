import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { room } from '../interfaces/room';
import { worker } from '../interfaces/worker';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  /* Api endpoints */
  url: string = "https://localhost:8443/api/rooms"; 
  url2: string = "https://localhost:8443/api/roomImages"; 
  url3: string = "https://localhost:8443/api/staff"; 

  constructor(private httpClient: HttpClient) { }

  /* room methods */
  postRoom(number: number, floor: number, roomType: number, bedAmount: number): Observable<room> {
    const roomData = { number, floor, roomType, bedAmount };

    // Send a POST request to the API for room creation
    return this.httpClient.post<room>(`${this.url}`, roomData)
  }

  uploadRoomImages(formData: FormData) {

    // Send a POST request to the API for image upload
    return this.httpClient.post(`${this.url2}`, formData);
  }

  getRooms(): Observable<room[]> {
    return this.httpClient.get<room[]>(this.url).pipe(
      tap((rooms: room[]) => {
        console.log('Fetched rooms:', rooms);
      })
    );
  }

  deleteRoom(id: number): Observable<room> {
    return this.httpClient.delete<room>(`${this.url}/${id}`);
  }

  /* worker methods */
  postWorker(number: number, floor: number, roomType: number, bedAmount: number): Observable<room> {
    const roomData = { number, floor, roomType, bedAmount };

    // Send a POST request to the API for room creation
    return this.httpClient.post<room>(`${this.url}`, roomData)
  }

  deleteWorker(id: number): Observable<worker> {
    return this.httpClient.delete<worker>(`${this.url3}/${id}`);
  }

}
