import { Component, ViewChild } from '@angular/core';
import { LoginService } from '../../services/loginService';
import { GuestDto } from '../../interfaces/guestDto';
import { Observer } from 'rxjs';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { ManageGuestService } from '../../services/manageGuestService';

@Component({
  selector: 'app-manageGuest',
  standalone: false,
  templateUrl: './manageGuest.html',
  styleUrl: './manageGuest.css',
})
export class ManageGuest {
  isReception: Boolean = false;
  isAdmin: Boolean = false;

  constructor(
    private manageGuestService: ManageGuestService,
    private loginService: LoginService,
  ) {
    if (this.loginService.isReceptionist()) {
      this.isReception = true
    } else if (this.loginService.isAdmin()) {
      this.isAdmin = true
    }
  }

  displayedColumns: string[] = ['fullName', 'email', 'phoneNumber', 'buttons'];
  filterValue: string = '';
  fetchFailed: boolean = false
  DATA: GuestDto[] = [];
  dataSource = new MatTableDataSource<GuestDto>(this.DATA);

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.getGuest()
  }

  /* search filter */
  searchFilter() {
    this.dataSource.filter = this.filterValue.trim().toLowerCase();
  }

  getGuest() {
    const observer: Observer<GuestDto[]> = {
      next: (guests) => {
        this.DATA = Array.isArray(guests) ? guests : [];
        this.dataSource.data = this.DATA;
      },
      error: (err) => {
        //console.error('Rooms fetch failed:', err);
        this.fetchFailed = true;
      },
      complete: () => {
        // optional cleanup or navigation
      },
    };

    this.manageGuestService.getGuests().subscribe(observer);
  }

  DeleteGuestRow(id: number) {

    const observer: Observer<any> = {
      next: (response) => {
        //console.log('Delete successful.', response);
        this.getGuest();
      },
      error: (error) => {
        //console.error('Delete error.', error);
        alert("Sletning fejlede!")
      },
      complete: () => {
        // optional cleanup or navigation
      },
    };

    this.manageGuestService.deleteGuest(id).subscribe(observer);
  }

}
