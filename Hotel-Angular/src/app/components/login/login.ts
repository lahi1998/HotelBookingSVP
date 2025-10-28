import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Observer } from 'rxjs';
import { LoginService } from '../../services/login-service';
import { LoginInterface } from '../../interfaces/login-interface';

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
        alert('Login successful!');
        this.router.navigate(['/Components/mainContent']);
      },
      error: (error) => {
        console.error('Login error', error);
        alert('Invalid credentials.');
      },
      complete: () => { },
    };

    this.loginService.postLogin(username, password).subscribe(observer);
  }
}
