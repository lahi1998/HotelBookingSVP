import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, forkJoin, map, Observable, of, tap } from 'rxjs';
import { roomDto } from '../interfaces/roomDto';
import { staffDto } from '../interfaces/staffDto';
import { CreateStaffRequest } from '../interfaces/createStaffRequest';
import { CreateRoomRequest } from '../interfaces/createRoomRequest';
import { CreateRoomTypeRequest } from '../interfaces/createRoomTypeRequest';
import { roomTypeDto } from '../interfaces/roomTypeDto';
import { UpdateStaffRequest } from '../interfaces/updateStaffRequest';
import { RoomTypeImageDto } from '../interfaces/roomTypeImageDto';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  /* Api endpoints */
  baseUrl: string = "https://hotel-hyggely.dk/api"
  url: string = "https://hotel-hyggely.dk/api/rooms";
  url2: string = "https://hotel-hyggely.dk/api/roomTypes/images";
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
      'Authorization': `Bearer ${token}`
    });

    // Send a POST request to the API for image upload
    return this.httpClient.post(`${this.url2}`, formData, { headers });
  }

  deleteRoomTypeImages(ids: number[]): Observable<void>{
    const token = this.getToken();
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    const deleteRequests = ids.map(id =>
      this.httpClient.delete(`${this.baseUrl}/roomtypes/images/${id}`, { headers })
        .pipe(
          map(() => ({ id, success: true })),
          catchError(() => of({ id, success: false })) // catch per-request errors
        )
    );

    return forkJoin(deleteRequests).pipe(
      map(results => {
        const failed = results.filter(r => !r.success);
        if (failed.length > 0) {
          throw new Error(`Failed to delete ${failed.length} images`);
        }
        // ✅ All succeeded
        return void 0;
      })
    );
  }

  getRooms(): Observable<roomDto[]> {

    const token = this.getToken();
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    return this.httpClient.get<roomDto[]>(this.url, {headers}).pipe(
      tap((rooms: roomDto[]) => {
      })
    );
  }

  updateRoom(formData: FormData): Observable<roomDto>{
    const token = this.getToken();
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    return this.httpClient.put<roomDto>(this.url, formData, { headers }).pipe(
      tap((room: roomDto) => {
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

    return this.httpClient.post<CreateRoomTypeRequest>(`${this.url4}`, newRoomType, { headers })
  }

  updateRoomType(formData: FormData): Observable<roomTypeDto>{
    const token = this.getToken();
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    return this.httpClient.put<roomTypeDto>(this.url4, formData, { headers }).pipe(
      tap((roomType: roomTypeDto) => {
      })
    );
  }

  getRoomTypes(): Observable<roomTypeDto[]> {

    const token = this.getToken();
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    return this.httpClient.get<roomTypeDto[]>(this.url4, { headers }).pipe(
      tap((roomType: roomTypeDto[]) => {
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

  getRoomTypeImages(roomTypeId: string | number){
    const token = this.getToken();
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    return this.httpClient.get<RoomTypeImageDto[]>(`${this.baseUrl}/roomtypes/${roomTypeId}/images`, { headers }).pipe(
      tap((roomType: RoomTypeImageDto[]) => {
      })
    );
  }


  /* Worker methods */
  postWorker(newWorker: CreateStaffRequest): Observable<CreateStaffRequest> {

    const token = this.getToken();
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

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

  putWorker(editWorker: UpdateStaffRequest): Observable<UpdateStaffRequest> {

    const token = this.getToken();
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    return this.httpClient.put<UpdateStaffRequest>(`${this.url3}`, editWorker, { headers })
  }

}
