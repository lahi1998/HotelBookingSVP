import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { LoginInterface } from '../interfaces/loginInterface';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  url: string = "https://hotel-hyggely.dk/api/auth/login"; // API endpoint

  constructor(private httpClient: HttpClient) { }

  postLogin(username: string, password: string): Observable<LoginInterface> {
    const loginData = { username, password };

    // Send a POST request to the API for login
    return this.httpClient.post<LoginInterface>(`${this.url}`, loginData)
      .pipe(
        // Handle the response and store the JWT token in session or local storage
        tap((response: any) => {
          if (response.token) {
            const randomKey = this.generateRandomString(40);
            // Store the JWT token in session storage (temp solution not safe todo)
            sessionStorage.setItem(randomKey, response.token);
            sessionStorage.setItem('authKey', randomKey);
          }
        })
      );
  }

  generateRandomString(length: number = 40): string {
    const chars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    let result = '';
    for (let i = 0; i < length; i++) {
      result += chars.charAt(Math.floor(Math.random() * chars.length));
    }
    return result;
  }
}
