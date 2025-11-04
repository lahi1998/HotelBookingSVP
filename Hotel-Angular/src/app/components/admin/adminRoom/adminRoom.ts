import { Component, ViewChild, AfterViewInit } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { roomDto } from '../../../interfaces/roomDto';
import { ImageData } from '../../../interfaces/imageData';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observer } from 'rxjs';
import { AdminService } from '../../../services/adminService';
import { Router } from '@angular/router';
import { CreateRoomRequest } from '../../../interfaces/createRoomRequest';

@Component({
  selector: 'app-adminRoom',
  standalone: false,
  templateUrl: './adminRoom.html',
  styleUrl: './adminRoom.css',
})
export class AdminRoom implements AfterViewInit {
  displayedColumns: string[] = ['number', 'floor', 'roomType', 'bedAmount', 'buttons'];
  filterValue: string = '';
  DATA: roomDto[] = [];
  dataSource = new MatTableDataSource<roomDto>(this.DATA);
  roomForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private adminService: AdminService,
  ) { }

  ngOnInit() {
    this.roomForm = this.fb.group({
      number: ['', Validators.required],
      floor: ['', Validators.required],
      roomType: ['', Validators.required],
      bedamount: ['', Validators.required],
    });

    this.getRooms();
  }

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }
  
  /* search filter */
  searchFilter() {
    this.dataSource.filter = this.filterValue.trim().toLowerCase();
  }

  onSubmit(): void {
    if (this.roomForm.valid) {
      const newRoom: CreateRoomRequest = {
        roomType: this.roomForm.value.roomTypeName,
        lastCleaned: new Date(),
        number: this.roomForm.value.number,
        floor: this.roomForm.value.floor,
        bedAmount: this.roomForm.value.bedamount,
      };


      const observer: Observer<any> = {
        next: (response) => {
          console.log('Create successful.', response);
          alert('Værelse oprettet!');
        },
        error: (error) => {
          console.error('Create error.', error);
          alert('Kunne ikke oprette værelse');
        },
        complete: () => {
          // optional cleanup or navigation
        },
      };

      this.adminService.postRoom(newRoom).subscribe(observer);
    }
  }

  getRooms() {
    const observer: Observer<roomDto[]> = {
      next: (rooms) => {
        this.DATA = Array.isArray(rooms) ? rooms : [];
        this.dataSource.data = this.DATA;
        console.log('Rooms fetched successfully', rooms);
      },
      error: (err) => {
        console.error('Rooms fetch failed:', err);
        alert('Kunne ikke hente værelserne');
      },
      complete: () => {
        // optional cleanup or navigation
      },
    };

    this.adminService.getRooms().subscribe(observer);
  }

  DeleteRow(id: number) {

    const observer: Observer<any> = {
      next: (response) => {
        console.log('Delete successful.', response);
        alert('Værelse slettet');
        this.getRooms();
      },
      error: (error) => {
        console.error('Delete error.', error);
        alert('Kunne ikke slette værelset');
      },
      complete: () => {
        // optional cleanup or navigation
      },
    };

    this.adminService.deleteRoom(id).subscribe(observer);
  }

}
