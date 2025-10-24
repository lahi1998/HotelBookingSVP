import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { LoginInterface } from '../interfaces/login-interface';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  url: string = "https://localhost:8443/api/auth";
  endpointLogin: string = "login"; // API endpoint

  constructor(private httpClient: HttpClient) { }

  postLogin(username: string, password: string): Observable<LoginInterface> {
    const loginData = { username, password };

    // Send a POST request to the API for login
    return this.httpClient.post<LoginInterface>(`${this.url}/${this.endpointLogin}`, loginData)
      .pipe(
        // Handle the response and store the JWT token in session or local storage
        tap((response: any) => {
          if (response.jwtToken) {
            // Store the JWT token in session storage (temp solution not safe todo)
            sessionStorage.setItem('jwtToken', response.jwtToken);
          }
        })
      );
  }
}
