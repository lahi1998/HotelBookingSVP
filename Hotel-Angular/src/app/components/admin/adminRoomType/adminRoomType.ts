import { Component, ViewChild, AfterViewInit } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { roomTypeDto } from '../../../interfaces/roomTypeDto';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AdminService } from '../../../services/adminService';
import { Router } from '@angular/router';
import { CreateRoomTypeRequest } from '../../../interfaces/createRoomTypeRequest';
import { Observer } from 'rxjs';


@Component({
  selector: 'app-adminRoomType',
  standalone: false,
  templateUrl: './adminRoomType.html',
  styleUrls: ['./adminRoomType.css'],
})
export class AdminRoomType implements AfterViewInit {
  displayedColumns: string[] = ['id', 'name', 'price', 'buttons'];
  filterValue: string = '';
  DATA: roomTypeDto[] = [];
  dataSource = new MatTableDataSource<roomTypeDto>(this.DATA);
  roomTypeForm!: FormGroup;

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(
    private fb: FormBuilder,
    private adminService: AdminService,
  ) { }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  ngOnInit() {
    this.roomTypeForm = this.fb.group({
      name: [''],
      price: [''],
    });

    this.getRoomtypes()

  }

  /* search filter */
  searchFilter() {
    this.dataSource.filter = this.filterValue.trim().toLowerCase();
  }

  onSubmit(): void {
    if (this.roomTypeForm.valid) {
      const newRoomtype: CreateRoomTypeRequest = {
        name: this.roomTypeForm.value.name,
        price: this.roomTypeForm.value.price,
      };

      const observer: Observer<any> = {
        next: (response) => {
          console.log('Create successful.', response);
          alert('Create successful!');

        },
        error: (error) => {
          console.error('Create error.', error);
          alert('Create error!');
        },
        complete: () => {
          // optional cleanup or navigation
        },
      };

      this.adminService.postRoomType(newRoomtype).subscribe(observer);
    }
  }

  getRoomtypes() {
    const observer: Observer<roomTypeDto[]> = {
      next: (roomType) => {
        this.DATA = Array.isArray(roomType) ? roomType : [];
        this.dataSource.data = this.DATA;
        console.log('Room types fetched successfully', roomType);
        alert('Room types fetched!');
      },
      error: (err) => {
        console.error('Room types fetch failed:', err);
        alert('Room types fetch failed!');
      },
      complete: () => {
        // optional cleanup or navigation
      },
    };

    this.adminService.getRoomTypes().subscribe(observer);
  }

  DeleteRow(id: number) {

    const observer: Observer<any> = {
      next: (response) => {
        console.log('Delete successful.', response);
        alert('Delete successful!');
        this.getRoomtypes();
      },
      error: (error) => {
        console.error('Delete error.', error);
        alert('Delete error!');
      },
      complete: () => {
        // optional cleanup or navigation
      },
    };

    this.adminService.deleteRoomType(id).subscribe(observer);
  }

}


