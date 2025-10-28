import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { room } from '../interfaces/room';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  url: string = "https://localhost:8443/api/auth";
  endpointRoom: string = "room"; // API endpoint

  constructor(private httpClient: HttpClient) { }

  postRoom(/*username: string, password: string*/): Observable<room> {
    const roomData = {/* username, password */};

    // Send a POST request to the API for login
    return this.httpClient.post<room>(`${this.url}/${this.endpointRoom}`, roomData)
  }
}
