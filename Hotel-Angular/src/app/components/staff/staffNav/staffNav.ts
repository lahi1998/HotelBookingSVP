import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-staffNav',
  standalone: false,
  templateUrl: './staffNav.html',
  styleUrl: './staffNav.css',
})
export class StaffNav {

  constructor(
    private router: Router,
  ) { }

  logout(): void {
    const storedKey = sessionStorage.getItem('authKey');
    if (storedKey) {
      sessionStorage.removeItem(storedKey);
      sessionStorage.removeItem('authKey');
    }

    this.router.navigate(['/login']);
  }

}
