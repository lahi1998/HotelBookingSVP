import { NgModule, Injectable } from '@angular/core';
import { RouterModule, Routes, CanActivateChild, ActivatedRouteSnapshot, RouterStateSnapshot, Router, UrlTree } from '@angular/router';
import { Login } from './components/login/login';
import { AdminRoom } from './components/admin/admin-room/admin-room';
import { AdminRoomType } from './components/admin/admin-room-type/admin-room-type';
import { AdminWorker } from './components/admin/admin-worker/admin-worker';
import { AdminNav } from './components/admin/admin-nav/admin-nav';

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
      { path: 'nav', component: AdminNav },
    ],
  },

  // --- Staff child group ---
  {
    path: 'staff',
    canActivateChild: [AuthGuard],
    children: [
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
