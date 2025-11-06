import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LoginService } from '../../../services/loginService';

@Component({
  selector: 'app-staffNav',
  standalone: false,
  templateUrl: './staffNav.html',
  styleUrl: './staffNav.css',
})
export class StaffNav implements OnInit {
  cleaner: boolean = false

  constructor(
    private router: Router,
    private loginService: LoginService
  ) { }

  menuOpen = false;

  toggleMenu() {
    this.menuOpen = !this.menuOpen;
  }

  logout(): void {
    const storedKey = sessionStorage.getItem('authKey');
    if (storedKey) {
      sessionStorage.removeItem(storedKey);
      sessionStorage.removeItem('authKey');
    }

    this.router.navigate(['/login']);
  }

  ngOnInit(): void {
      if (this.loginService.isCleaning()) {
      this.cleaner = true;
      }
  }


}
