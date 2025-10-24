import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observer } from 'rxjs'
import { LoginService } from '../../services/login-service';


@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login implements OnInit{
  constructor(private loginService: LoginService, private router: Router) {}

  ngOnInit(): void {

  }

}
