import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router,  } from '@angular/router';
import { Observer } from 'rxjs';
import { LoginService } from '../../services/loginService';
import { LoginInterface } from '../../interfaces/loginInterface';


@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.html',
  styleUrls: ['./login.css'],
})
export class Login implements OnInit {
  loginForm!: FormGroup;
  hidePassword = true;

  constructor(
    private fb: FormBuilder,
    private loginService: LoginService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  togglePassword(): void {
    this.hidePassword = !this.hidePassword;
  }

  onSubmit(): void {
    if (this.loginForm.invalid) return;

    const { username, password } = this.loginForm.value;

    const observer: Observer<LoginInterface> = {
      next: (response) => {
        console.log('Login successful', response);
        const role = this.loginService.getRole();
        
          if (role === 'Admin') {
            this.router.navigate(['/admin/room']);
          } else if (role === 'Receptionist') {
            this.router.navigate(['/staff/check-in-out']);
          } else if (role === 'Cleaning') {
            this.router.navigate(['/cleaning/cleaningSchedule']);
          }
      },
      error: (error) => {
        console.error('Login error', error);
        alert('Invalid credentials.');
      },
      complete: () => { 

      },
    };

    this.loginService.postLogin(username, password).subscribe(observer);
  }

}
