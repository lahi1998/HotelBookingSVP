import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { roomDto } from '../interfaces/roomDto';
import { staffDto } from '../interfaces/staffDto';
import { CreateStaffRequest } from '../interfaces/createStaffRequest';
import { CreateRoomRequest } from '../interfaces/createRoomRequest';

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
  postRoom(newRoom: CreateRoomRequest): Observable<CreateRoomRequest> {

    // Send a POST request to the API for room creation
    return this.httpClient.post<CreateRoomRequest>(`${this.url}`, newRoom)
  }

  uploadRoomImages(formData: FormData) {

    // Send a POST request to the API for image upload
    return this.httpClient.post(`${this.url2}`, formData);
  }

  getRooms(): Observable<roomDto[]> {
    return this.httpClient.get<roomDto[]>(this.url).pipe(
      tap((rooms: roomDto[]) => {
        console.log('Fetched rooms:', rooms);
      })
    );
  }

  deleteRoom(id: number): Observable<any> {
    return this.httpClient.delete<any>(`${this.url}/${id}`);
  }

  /* worker methods */
  postWorker(newWorker: CreateStaffRequest): Observable<CreateStaffRequest> {

    // Send a POST request to the API for room creation
    return this.httpClient.post<CreateStaffRequest>(`${this.url}`, newWorker)
  }

  deleteWorker(id: number): Observable<any> {
    return this.httpClient.delete<any>(`${this.url3}/${id}`);
  }

}
