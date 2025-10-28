import { Component, ViewChild, AfterViewInit } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { room } from '../../../interfaces/room';

interface ImageData {
  file: File;
  dataUrl: string;
}

@Component({
  selector: 'app-admin-room',
  standalone: false,
  templateUrl: './admin-room.html',
  styleUrl: './admin-room.css',
})
export class AdminRoom implements AfterViewInit {
  displayedColumns: string[] = ['number', 'floor', 'type', 'bedCount', 'buttons'];
  dataSource = new MatTableDataSource<room>(DATA);
  
  // Image handling properties
  images: ImageData[] = [];
  currentImageIndex = 0;

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
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
}

// --- Mock data ---
const DATA: room[] = [
  { number: 1, floor: 1, type: 'Enkelt', bedCount: 1},
  { number: 2, floor: 2, type: 'Double', bedCount: 2},
  { number: 3, floor: 1, type: 'Enkelt', bedCount: 1},
  { number: 4, floor: 2, type: 'Double', bedCount: 2},
  { number: 5, floor: 2, type: 'Konge', bedCount: 1},
  { number: 6, floor: 2, type: 'Dronning', bedCount: 1}
];
