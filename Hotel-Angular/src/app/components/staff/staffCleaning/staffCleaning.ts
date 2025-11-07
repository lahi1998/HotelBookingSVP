import { Component, ViewChild } from '@angular/core';
import { StaffService } from '../../../services/staffService';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { Observer } from 'rxjs';
import { CleaningScheduleDto } from '../../../interfaces/cleaningScheduleDto';

@Component({
  selector: 'app-staffCleaning',
  standalone: false,
  templateUrl: './staffCleaning.html',
  styleUrl: './staffCleaning.css',
})
export class StaffCleaning {
  displayedColumns: string[] = ['roomNumber', 'roomFloor', 'cleaningDate', 'buttons'];
  filterValue: string = '';
  fetchFailed: boolean = false
  DATA: CleaningScheduleDto[] = [];
  dataSource = new MatTableDataSource<CleaningScheduleDto>(this.DATA);

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  ngOnInit() {
    this.getCleaningSchedule();
  }

  constructor(
    private staffService: StaffService,
  ) { }

  /* search filter */
  searchFilter() {
    this.dataSource.filter = this.filterValue.trim().toLowerCase();
  }

  confirmingId: number | null = null;

  confirm(id: number) {
    this.cleaned(id);        // Kald din eksisterende funktion
    this.confirmingId = null; // Luk popover
  }

  cancel() {
    this.confirmingId = null; // Luk popover uden at gøre noget
  }

  getCleaningSchedule() {
    const observer: Observer<CleaningScheduleDto[]> = {
      next: (cleaningSchedule) => {
        this.DATA = Array.isArray(cleaningSchedule) ? cleaningSchedule : [];
        this.dataSource.data = this.DATA;
        //console.log('Cleaning schedule fetched successfully', cleaningSchedule);
      },
      error: (err) => {
        //console.error('Cleaning schedule fetch failed:', err);
        //alert('Cleaning schedule fetch failed!');
        this.fetchFailed = true;
      },
      complete: () => {
        // optional cleanup or navigation
      },
    };

    this.staffService.getCleaningSchedule().subscribe(observer);
  }

  cleaned(id: number) {

    const observer: Observer<any> = {
      next: (response) => {
        //console.log('patch successful.', response);
        this.getCleaningSchedule();
      },
      error: (error) => {
        //console.error('patch error.', error);
        alert('Updatering fejlede!');
      },
      complete: () => {
        // optional cleanup or navigation
      },
    };

    this.staffService.MarkCleanedCleaningSchedule(id).subscribe(observer);
  }
}

