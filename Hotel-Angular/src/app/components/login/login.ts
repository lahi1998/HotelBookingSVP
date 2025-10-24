import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observer } from 'rxjs'
import { LoginService } from '../../services/login-service';
import { LoginInterface } from '../../interfaces/login-interface';


@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  username: string = '';
  password: string = '';
  hidePassword: boolean = true;

  usernameError: boolean = false;
  passwordError: boolean = false;

  constructor(private loginService: LoginService, private router: Router) { }

  togglePassword() {
    this.hidePassword = !this.hidePassword;
  }

  onSubmit() {
    // Reset error flags
    this.usernameError = !this.username.trim();
    this.passwordError = !this.password.trim();

    if (this.usernameError || this.passwordError) {
      return; // Stop submission if any field is empty
    }

    const self = this;

    const observer: Observer<LoginInterface> = {
      next(response: LoginInterface) {
        console.log('Login successful', response);
        alert('Login successful!');
        self.router.navigate(['/Components/mainContent']);
      },
      error(error) {
        console.error('Login error', error);
        alert('Invalid credentials.');
      },
      complete() { }
    };

    this.loginService.postLogin(this.username, this.password).subscribe(observer);
  }
}