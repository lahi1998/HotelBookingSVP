import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { roomDto } from '../interfaces/roomDto';
import { staffDto } from '../interfaces/staffDto';
import { CreateStaffRequest } from '../interfaces/createStaffRequest';
import { CreateRoomRequest } from '../interfaces/createRoomRequest';
import { CreateRoomTypeRequest } from '../interfaces/create-room-type-request';
import { roomTypeDto } from '../interfaces/roomTypeDto';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  /* Api endpoints */
  url: string = "https://hotel-hyggely.dk/api/rooms";
  url2: string = "https://hotel-hyggely.dk/api/roomImages";
  url3: string = "https://hotel-hyggely.dk/api/staff";
  url4: string = "https://hotel-hyggely.dk/api/roomtypes";
  url5: string = "https://hotel-hyggely.dk/api/roomstatuses";

  constructor(private httpClient: HttpClient) { }

  /* get token */
  getToken() {
    const storedKey = sessionStorage.getItem('authKey');
    const token = sessionStorage.getItem(storedKey ? storedKey : '');
    return token;
  }


  /* room methods */
  postRoom(newRoom: CreateRoomRequest): Observable<CreateRoomRequest> {

    const token = this.getToken();
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
    // Send a POST request to the API for room creation
    return this.httpClient.post<CreateRoomRequest>(`${this.url}`, newRoom, { headers })
  }

  uploadRoomImages(formData: FormData) {

    const token = this.getToken();
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    // Send a POST request to the API for image upload
    return this.httpClient.post(`${this.url2}`, formData, { headers });
  }

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

  deleteRoom(id: number): Observable<any> {

    const token = this.getToken();
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    return this.httpClient.delete<any>(`${this.url}/${id}`, { headers });
  }

  /* Room type methods */
  postRoomType(newRoomType: CreateRoomTypeRequest): Observable<CreateRoomTypeRequest> {

    const token = this.getToken();
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    // Send a POST request to the API for room creation
    return this.httpClient.post<CreateRoomTypeRequest>(`${this.url4}`, newRoomType, { headers })
  }

  getRoomTypes(): Observable<roomTypeDto[]> {

    const token = this.getToken();
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    return this.httpClient.get<roomTypeDto[]>(this.url4, { headers }).pipe(
      tap((roomType: roomTypeDto[]) => {
        console.log('Fetched room types:', roomType);
      })
    );

  }

  deleteRoomType(id: number): Observable<any> {

    const token = this.getToken();
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    return this.httpClient.delete<any>(`${this.url4}/${id}`, { headers });
  }




  /* Worker methods */
  postWorker(newWorker: CreateStaffRequest): Observable<CreateStaffRequest> {

    const token = this.getToken();
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    // Send a POST request to the API for room creation
    return this.httpClient.post<CreateStaffRequest>(`${this.url3}`, newWorker, { headers })
  }

  getWorkers(): Observable<staffDto[]> {

    const token = this.getToken();
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    return this.httpClient.get<staffDto[]>(this.url3, { headers }).pipe(
      tap((Workers: staffDto[]) => {
        console.log('Fetched workers:', Workers);
      })
    );

  }

  deleteWorker(id: number): Observable<any> {

    const token = this.getToken();
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    return this.httpClient.delete<any>(`${this.url3}/${id}`, { headers });
  }


}
