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
import { roomTypeDto } from '../../../interfaces/roomTypeDto';

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
  roomEditForm!: FormGroup;
  roomTypes: roomTypeDto[] = [];
  fetchFailed: boolean = false

  constructor(
    private fb: FormBuilder,
    private adminService: AdminService,
  ) { }

  ngOnInit() {
    this.roomForm = this.fb.group({
      number: ['', Validators.required],
      floor: ['', Validators.required],
      roomTypeName: ['', Validators.required],
      bedAmount: ['', Validators.required],
    });

    this.roomEditForm = this.fb.group({
      id: [''],
      number: ['', Validators.required],
      floor: ['', Validators.required],
      roomTypeName: ['', Validators.required],
      bedAmount: ['', Validators.required]
    });

    this.getRooms();
    this.getRoomTypes();
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
        roomTypeName: this.roomForm.value.roomTypeName,
        number: this.roomForm.value.number,
        floor: this.roomForm.value.floor,
        bedAmount: this.roomForm.value.bedAmount,
      };


      const observer: Observer<any> = {
        next: (response) => {
          //console.log('Create successful.', response);
          alert('Værelse oprettet!');
          this.roomForm.reset();

          /* Reset form with default role */
          this.roomForm.reset({
            number: '', // 👈 your default value here
            floor: '',
            roomTypeName: 'Enkelt',
            bedAmount: '',
          });

        },
        error: (error) => {
          //console.error('Create error.', error);
          alert('Kunne ikke oprette værelse');
        },
        complete: () => {

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
        //console.log('Rooms fetched successfully', rooms);
      },
      error: (err) => {
        //console.error('Rooms fetch failed:', err);
        this.fetchFailed = true;
      },
      complete: () => {
        // optional cleanup or navigation
      },
    };

    this.adminService.getRooms().subscribe(observer);
  }

  getRoomTypes() {
    const observer: Observer<roomTypeDto[]> = {
      next: (roomTypes) => {
        this.roomTypes = roomTypes
        this.roomForm.patchValue({roomTypeName: roomTypes[0].name})
      },
      error: (err) => {
        //console.error('Roomtypes fetch failed:', err);
        alert('Kunne ikke hente værelsestyperne');
      },
      complete: () => {
        // optional cleanup or navigation
      },
    };

    this.adminService.getRoomTypes().subscribe(observer);
  }

  editRoom() {

    const observer: Observer<roomDto> = {
      next: (response) => {
        //console.log('Room updated');
        const index = this.dataSource.data.findIndex(r => r.id === response.id);
        if (index !== -1) {
          this.dataSource.data[index] = response;
          this.dataSource._updateChangeSubscription();
        }

        alert("Værelse opdateret");
      },
      error: (err) => {
        //console.error('room update failed:', err);
        alert('Kunne ikke opdatere værelse');
      },
      complete: () => {
        // optional cleanup or navigation
      },
    };
    this.adminService.updateRoom(this.roomEditForm.value).subscribe(observer);
    this.closeEditModal();
  }

  DeleteRow(id: number) {

    const observer: Observer<any> = {
      next: (response) => {
        //console.log('Delete successful.', response);
        alert('Værelse slettet');
        this.getRooms();
      },
      error: (error) => {
        //console.error('Delete error.', error);
        alert('Kunne ikke slette værelset');
      },
      complete: () => {
        // optional cleanup or navigation
      },
    };

    this.adminService.deleteRoom(id).subscribe(observer);
  }

  //Modal visibility functions
  openEditModal(room: any) {
    this.roomEditForm.setValue({id: room.id, number: room.number, floor: room.floor, roomTypeName: room.roomTypeName, bedAmount: room.bedAmount})
    this.toggleModal("editModal", true);
  }

  closeEditModal() {
    this.toggleModal("editModal", false);
  }

  toggleModal(id: string, show: boolean) {
    document.getElementById(id)?.classList.toggle('hidden', !show);
  }

  //Closes the modal if the user presses outside the modal
  onBackdropClick(event: MouseEvent) {
    const clickedElement = event.target as HTMLElement;

    if (clickedElement.classList.contains('modal')) {
      this.closeEditModal();
    }
  }
}
