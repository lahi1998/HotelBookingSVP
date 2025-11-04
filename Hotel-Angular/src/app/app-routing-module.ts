import { NgModule, Injectable } from '@angular/core';
import { RouterModule, Routes, CanActivateChild, ActivatedRouteSnapshot, RouterStateSnapshot, Router, UrlTree } from '@angular/router';
import { Login } from './components/login/login';
import { AdminRoom } from './components/admin/adminRoom/adminRoom';
import { AdminRoomType } from './components/admin/adminRoomType/adminRoomType';
import { AdminWorker } from './components/admin/adminWorker/adminWorker';
import { StaffBooking } from './components/staff/staffBooking/staffBooking';
import { StaffCheckInOut } from './components/staff/staffCheckInOut/staffCheckInOut';
import { StaffCleaning } from './components/staff/staffCleaning/staffCleaning';
import { Booking } from './components/booking/booking'
import { StaffRoomstatus } from './components/staff/staffRoomstatus/staffRoomstatus';
import { StaffNav } from './components/staff/staffNav/staffNav';
import { LoginService } from './services/loginService';

// --- Simple AuthService ---
@Injectable({ providedIn: 'root' })
export class AuthService {
  isAuthenticated(): boolean {
    const storedKey = sessionStorage.getItem('authKey');
    if (!storedKey) return false;
    const token = sessionStorage.getItem(storedKey);
    return !!token;
  }
}

// --- AuthGuard using AuthService ---
@Injectable({ providedIn: 'root' })
export class AdminAuthGuard implements CanActivateChild {
  constructor(private auth: AuthService, private router: Router, private loginService: LoginService) { }

  canActivateChild(): boolean | UrlTree {
    if (this.auth.isAuthenticated()) {
      if (this.loginService.isAdmin()) {
      return true;
      }
    }
    return this.router.parseUrl('/login');
  }
}

@Injectable({ providedIn: 'root' })
export class ReceptionistAuthGuard implements CanActivateChild {
  constructor(private auth: AuthService, private router: Router, private loginService: LoginService) { }

  canActivateChild(): boolean | UrlTree {
    if (this.auth.isAuthenticated()) {
      if (this.loginService.isReceptionist()) {
      return true;
      }
    }
    return this.router.parseUrl('/login');
  }
}

@Injectable({ providedIn: 'root' })
export class CleaningAuthGuard implements CanActivateChild {
  constructor(private auth: AuthService, private router: Router, private loginService: LoginService) { }

  canActivateChild(): boolean | UrlTree {
    if (this.auth.isAuthenticated()) {
      if (this.loginService.isCleaning()) {
      return true;
      }
    }
    return this.router.parseUrl('/login');
  }
}

// --- Routes ---
const routes: Routes = [
  { path: 'login', component: Login },
  { path: 'booking', component: Booking },
  

  // --- Admin child group ---
  {
    path: 'admin',
    canActivateChild: [AdminAuthGuard],
    children: [
      { path: 'room', component: AdminRoom },
      { path: 'room-type', component: AdminRoomType },
      { path: 'worker', component: AdminWorker },
    ],
  },

  // --- Staff child group ---
  {
    path: 'staff',
    canActivateChild: [ReceptionistAuthGuard],
    children: [
      { path: 'booking', component: StaffBooking },
      { path: 'check-in-out', component: StaffCheckInOut },
      { path: 'room-status', component: StaffRoomstatus },
      { path: 'nav', component: StaffNav },
    ],
  },
  {
    path: 'staff',
    canActivateChild: [CleaningAuthGuard],
    children: [
      { path: 'cleaning', component: StaffCleaning },
    ],
  },
  // --- Default redirects ---
  { path: '', redirectTo: 'booking', pathMatch: 'full' },
  { path: '**', redirectTo: 'login' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }
