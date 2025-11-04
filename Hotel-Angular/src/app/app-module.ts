import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { Login } from './components/login/login';
import { AdminRoom } from './components/admin/adminRoom/adminRoom';
import { AdminRoomType } from './components/admin/adminRoomType/adminRoomType';
import { AdminWorker } from './components/admin/adminWorker/adminWorker';
import { StaffBooking } from './components/staff/staffBooking/staffBooking';
import { StaffCheckInOut } from './components/staff/staffCheckInOut/staffCheckInOut';
import { StaffCleaning } from './components/staff/staffCleaning/staffCleaning';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { provideHttpClient } from '@angular/common/http';
import { AdminNav } from './components/admin/adminNav/adminNav';
import { StaffNav } from './components/staff/staffNav/staffNav';
import { Booking } from './components/booking/booking';
import { MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { StaffRoomstatus } from './components/staff/staffRoomstatus/staffRoomstatus';
import { MatDialogActions, MatDialogContent } from "@angular/material/dialog";

@NgModule({
  declarations: [
    App,
    Login,
    AdminWorker,
    AdminRoomType,
    AdminRoom,
    StaffBooking,
    StaffCheckInOut,
    StaffCleaning,
    AdminNav,
    StaffNav,
    Booking,
    StaffRoomstatus
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    MatButtonModule,
    MatCardModule,
    MatTableModule,
    MatPaginator,
    MatPaginatorModule,
    ReactiveFormsModule,
    MatDialogActions,
    MatDialogContent
],
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideHttpClient()
  ],
  bootstrap: [App]
})
export class AppModule { }
