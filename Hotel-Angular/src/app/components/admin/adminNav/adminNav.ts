import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-adminNav',
  standalone: false,
  templateUrl: './adminNav.html',
  styleUrl: './adminNav.css',
})
export class AdminNav {

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
