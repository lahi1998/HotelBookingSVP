import { Component, ViewChild, AfterViewInit } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { room } from '../../../interfaces/room';
import { ImageData } from '../../../interfaces/imageData';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observer } from 'rxjs';
import { AdminService } from '../../../services/adminService';
import { Router } from '@angular/router';

@Component({
  selector: 'app-adminRoom',
  standalone: false,
  templateUrl: './adminRoom.html',
  styleUrl: './adminRoom.css',
})
export class AdminRoom implements AfterViewInit {
  displayedColumns: string[] = ['number', 'floor', 'roomType', 'bedAmount', 'buttons'];
  filterValue: string = '';
  DATA: room[] = [];
  dataSource = new MatTableDataSource<room>(this.DATA);
  roomForm!: FormGroup;

  // Image handling properties
  images: ImageData[] = [];
  currentImageIndex = 0;

  constructor(
    private fb: FormBuilder,
    private adminService: AdminService,
    private router: Router
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

  applyFilter() {
    this.dataSource.filter = this.filterValue.trim().toLowerCase();
  }


  onSubmit(): void {
    if (this.roomForm.invalid) return;

    const { number, floor, roomType, bedAmount } = this.roomForm.value;

    const observer: Observer<room> = {
      next: (response) => {
        console.log('Create successful.', response);
        alert('Create successful!');

        // Upload the images AFTER room creation
        this.uploadRoomImages(number, floor);

      },
      error: (error) => {
        console.error('Create error.', error);
        alert('Create error!');
      },
      complete: () => {
        // optional cleanup or navigation
      },
    };

    this.adminService.postRoom(number, floor, roomType, bedAmount).subscribe(observer);
  }

  /* image carousel and upload logik */
  async onFilesSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files) {
      for (const file of Array.from(input.files)) {
        if (file.type.startsWith('image/')) {
          const dataUrl = await this.readFileAsDataURL(file);
          this.images.push({ file, dataUrl });
        }
      }
    }
  }

  private readFileAsDataURL(file: File): Promise<string> {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.onload = () => resolve(reader.result as string);
      reader.onerror = reject;
      reader.readAsDataURL(file);
    });
  }

  prevImage() {
    if (this.currentImageIndex > 0) {
      this.currentImageIndex--;
    }
  }

  nextImage() {
    if (this.currentImageIndex < this.images.length - 1) {
      this.currentImageIndex++;
    }
  }

  selectImage(index: number) {
    this.currentImageIndex = index;
  }

  deleteCurrentImage() {
    this.images.splice(this.currentImageIndex, 1);
    if (this.currentImageIndex >= this.images.length) {
      this.currentImageIndex = Math.max(0, this.images.length - 1);
    }
  }

  uploadRoomImages(number: string, floor: string) {
    if (this.images.length === 0) return;

    const formData = new FormData();
    formData.append('number', number);
    formData.append('floor', floor);

    this.images.forEach((image) => {
      formData.append('images', image.file);
    });

    const observer: Observer<any> = {
      next: () => {
        console.log('Images uploaded successfully');
        alert('Room images uploaded and saved!');
      },
      error: (err) => {
        console.error('Image upload failed:', err);
        alert('Image upload failed!');
      },
      complete: () => {
        // optional cleanup or navigation
      },
    };

    this.adminService.uploadRoomImages(formData).subscribe(observer);
  }

  getRooms() {
    const observer: Observer<room[]> = {
      next: (rooms) => {
        this.DATA = Array.isArray(rooms) ? rooms : [];
        this.dataSource.data = this.DATA;
        console.log('Rooms fetched successfully', rooms);
        alert('Rooms fetched!');
      },
      error: (err) => {
        console.error('Rooms fetch failed:', err);
        alert('Rooms fetch failed!');
      },
      complete: () => {
        // optional cleanup or navigation
      },
    };

    this.adminService.getRooms().subscribe(observer);
  }

  DeleteRow(id: number) {

        const observer: Observer<room> = {
      next: (response) => {
        console.log('Delete successful.', response);
        alert('Delete successful!');
      },
      error: (error) => {
        console.error('Delete error.', error);
        alert('Delete error!');
      },
      complete: () => {
        // optional cleanup or navigation
      },
    };

    this.adminService.deleteRoom(id).subscribe(observer);
  }

}
