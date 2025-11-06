import { Component, ViewChild, AfterViewInit } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { roomTypeDto } from '../../../interfaces/roomTypeDto';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AdminService } from '../../../services/adminService';
import { Router } from '@angular/router';
import { CreateRoomTypeRequest } from '../../../interfaces/createRoomTypeRequest';
import { Observer } from 'rxjs';
import { ImageData } from '../../../interfaces/imageData';


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
  
  roomTypeEditForm!: FormGroup;


  @ViewChild(MatPaginator) paginator!: MatPaginator;
  images: ImageData[] = [];
  currentImageIndex = 0;

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

    this.roomTypeEditForm = this.fb.group({
      id: [''],
      name: [''],
      price: ['']
    })

    this.getRoomtypes()

  }

  /* search filter */
  searchFilter() {
    this.dataSource.filter = this.filterValue.trim().toLowerCase();
  }

  onSubmit(): void {
    console.log("Here");
    if (this.roomTypeForm.valid) {
      const newRoomtype: CreateRoomTypeRequest = {
        name: this.roomTypeForm.value.name,
        price: this.roomTypeForm.value.price,
      };

      const observer: Observer<any> = {
        next: (response) => {
          console.log('Create successful.', response);
          alert('Værelsestype oprettet');
          this.uploadRoomImages(response.id);
        },
        error: (error) => {
          console.error('Create error.', error);
          alert('Kunne ikke oprettet værelsestype');
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
      },
      error: (err) => {
        console.error('Room types fetch failed:', err);
        alert('Kunne ikke hente værelsestyper');
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
        alert('Værelsestype slettet');
        this.getRoomtypes();
      },
      error: (error) => {
        console.error('Delete error.', error);
        alert('Kunne ikke slette værelsestype');
      },
      complete: () => {
        // optional cleanup or navigation
      },
    };

    this.adminService.deleteRoomType(id).subscribe(observer);
  }


  //Images logic
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
    else {
      this.currentImageIndex = this.images.length - 1;
    }
  }

  nextImage() {
    if (this.currentImageIndex < this.images.length - 1) {
      this.currentImageIndex++;
    }
    else {
      this.currentImageIndex = 0;
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

  uploadRoomImages(roomTypeId: string) {
    if (this.images.length === 0) return;

    const formData = new FormData();
    debugger;
    this.images.forEach((image) => {

      formData.append('images', image.file);
    });
    formData.append('roomTypeId', roomTypeId);

    const observer: Observer<any> = {
      next: () => {
        console.log('Images uploaded successfully');
      },
      error: (err) => {
        console.error('Image upload failed:', err);
        alert('Kunne ikke uploade billeder');
      },
      complete: () => {
        // optional cleanup or navigation
      },
    };

    this.adminService.uploadRoomImages(formData).subscribe(observer);
  }

  editRoomType(){

    const observer: Observer<roomTypeDto> = {
      next: (response) => {
        console.log('RoomType updated');
        const index = this.dataSource.data.findIndex(r => r.id === response.id);
        if (index !== -1) this.dataSource.data[index] = response;
        this.dataSource._updateChangeSubscription(); 
        
        alert("Værelsestype opdateret");
      },
      error: (err) => {
        console.error('roomtype update failed:', err);
        alert('Kunne ikke opdatere værelsestype');
      },
      complete: () => {
        // optional cleanup or navigation
      },
    };
    debugger;
    this.adminService.updateRoomType(this.roomTypeEditForm.value).subscribe(observer);
    this.closeEditModal();
  }

  //Modal visibility functions
  openEditModal(roomType: any) {
    debugger;
    this.roomTypeEditForm.setValue(roomType)
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

