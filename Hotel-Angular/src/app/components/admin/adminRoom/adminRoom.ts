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
  dataSource = new MatTableDataSource<room>(DATA);
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

    const {number, floor, roomType, bedAmount} = this.roomForm.value;

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
      complete: () => { },
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

  this.adminService.uploadRoomImages(formData).subscribe({
    next: () => {
      console.log('Images uploaded successfully');
      alert('Room images uploaded and saved!');
    },
    error: (err) => {
      console.error('Image upload failed:', err);
      alert('Image upload failed!');
    },
  });
}

}

// --- Mock data ---
const DATA: room[] = [
  { number: 101, floor: 1, roomType: 1, bedAmount: 1, lastCleaned: new Date(), roomStatus: true },
  { number: 102, floor: 1, roomType: 1, bedAmount: 2, lastCleaned: new Date(), roomStatus: false },
  { number: 201, floor: 2, roomType: 2, bedAmount: 3, lastCleaned: new Date(), roomStatus: true },  
];
