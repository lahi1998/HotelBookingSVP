import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { Login } from './components/login/login';
import { AdminRoom } from './components/admin/admin-room/admin-room';
import { AdminRoomType } from './components/admin/admin-room-type/admin-room-type';
import { AdminWorker } from './components/admin/admin-worker/admin-worker';
import { StaffBooking } from './components/staff/staff-booking/staff-booking';
import { StaffCheckInOut } from './components/staff/staff-check-in-out/staff-check-in-out';
import { StaffCleaning } from './components/staff/staff-cleaning/staff-cleaning';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';

@NgModule({
  declarations: [
    App,
    Login,
    AdminWorker,
    AdminRoomType,
    AdminRoom,
    StaffBooking,
    StaffCheckInOut,
    StaffCleaning
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    MatInputModule,
    MatButtonModule,
    MatFormFieldModule,
    MatCardModule,
    MatIconModule
  ],
  providers: [
    provideBrowserGlobalErrorListeners()
  ],
  bootstrap: [App]
})
export class AppModule { }
