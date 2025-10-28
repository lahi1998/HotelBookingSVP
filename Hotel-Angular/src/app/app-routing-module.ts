import { NgModule, Injectable } from '@angular/core';
import { RouterModule, Routes, CanActivateChild, ActivatedRouteSnapshot, RouterStateSnapshot, Router, UrlTree } from '@angular/router';
import { Login } from './components/login/login';
import { AdminRoom } from './components/admin/adminRoom/adminRoom';
import { AdminRoomType } from './components/admin/adminRoomType/adminRoomType';
import { AdminWorker } from './components/admin/adminWorker/adminWorker';
import { StaffBooking } from './components/staff/staffBooking/staffBooking';
import { StaffCheckInOut } from './components/staff/staffCheckInOut/staffCheckInOut';
import { StaffCleaning } from './components/staff/staffCleaning/staffCleaning';
import { StaffRoomstatus } from './components/staff/staffRoomstatus/staffRoomstatus';
import { StaffNav } from './components/staff/staffNav/staffNav';

// --- Simple AuthService ---
@Injectable({ providedIn: 'root' })
export class AuthService {
  private loggedIn = true; // change to false to test guard

  isAuthenticated(): boolean {
    return this.loggedIn;
  }

  login() {
    this.loggedIn = true;
  }

  logout() {
    this.loggedIn = false;
  }
}

// --- AuthGuard using AuthService ---
@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivateChild {
  constructor(private auth: AuthService, private router: Router) { }

  canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree {
    if (this.auth.isAuthenticated()) {
      return true;
    }
    return this.router.parseUrl('/login');
  }
}

// --- Routes ---
const routes: Routes = [
  { path: 'login', component: Login },

  // --- Admin child group ---
  {
    path: 'admin',
    canActivateChild: [AuthGuard],
    children: [
      { path: 'room', component: AdminRoom },
      { path: 'room-type', component: AdminRoomType },
      { path: 'worker', component: AdminWorker },
    ],
  },

  // --- Staff child group ---
  {
    path: 'staff',
    canActivateChild: [AuthGuard],
    children: [
      { path: 'booking', component: StaffBooking },
      { path: 'check-in-out', component: StaffCheckInOut },
      { path: 'cleaning', component: StaffCleaning },
      { path: 'room-status', component: StaffRoomstatus },
      { path: 'nav', component: StaffNav },
    ],
  },

  // --- Default redirects ---
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: '**', redirectTo: 'login' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }
