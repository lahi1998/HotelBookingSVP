import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { staffDto } from '../../../interfaces/staffDto';
import { Observer } from 'rxjs';
import { Router } from '@angular/router';
import { AdminService } from '../../../services/adminService';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CreateStaffRequest } from '../../../interfaces/createStaffRequest';

@Component({
  selector: 'app-adminWorker',
  standalone: false,
  templateUrl: './adminWorker.html',
  styleUrl: './adminWorker.css',
})
export class AdminWorker implements AfterViewInit {
  roles: string[] = ['Receptionist', 'Cleaning'];
  displayedColumns: string[] = ['role', 'username', 'fullname', 'buttons'];
  filterValue: string = '';
  DATA: staffDto[] = [];
  dataSource = new MatTableDataSource<staffDto>(this.DATA);
  workerForm!: FormGroup;

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(
    private fb: FormBuilder,
    private adminService: AdminService,
  ) { }

  ngOnInit() {
    this.workerForm = this.fb.group({
      role: ['', Validators.required],
      userName: ['', Validators.required],
      password: ['', Validators.required],
      passwordConfirm: ['', Validators.required],
      fullName: ['', Validators.required],
    },
      { validators: this.passwordsMatchValidator }
    );

    this.getWorkers();
  }

  passwordsMatchValidator(control: AbstractControl) {
    const password = control.get('password')?.value;
    const confirm = control.get('passwordConfirm')?.value;
    if (password && confirm && password !== confirm) {
      control.get('passwordConfirm')?.setErrors({ mismatch: true });
    } else {
      // only clear mismatch error (not required)
      if (control.get('passwordConfirm')?.hasError('mismatch')) {
        control.get('passwordConfirm')?.setErrors(null);
      }
    }
    return null;
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  /* search filter */
  searchFilter() {
    this.dataSource.filter = this.filterValue.trim().toLowerCase();
  }

  onSubmit(): void {
    if (this.workerForm.valid) {
      const newWorker: CreateStaffRequest = {
        role: this.workerForm.value.role,
        userName: this.workerForm.value.userName,
        password: this.workerForm.value.password,
        fullName: this.workerForm.value.fullName,
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

      this.adminService.postWorker(newWorker).subscribe(observer);
    }
  }

  getWorkers() {
    const observer: Observer<staffDto[]> = {
      next: (worker) => {
        this.DATA = Array.isArray(worker) ? worker : [];
        this.dataSource.data = this.DATA;
        console.log('Workers fetched successfully', worker);
        alert('Workers fetched!');
      },
      error: (err) => {
        console.error('Workers fetch failed:', err);
        alert('Workers fetch failed!');
      },
      complete: () => {
        // optional cleanup or navigation
      },
    };

    this.adminService.getWorkers().subscribe(observer);
  }

  DeleteRow(id: number) {

    const observer: Observer<any> = {
      next: (response) => {
        console.log('Delete successful.', response);
        alert('Delete successful!');
        this.getWorkers();
      },
      error: (error) => {
        console.error('Delete error.', error);
        alert('Delete error!');
      },
      complete: () => {
        // optional cleanup or navigation
      },
    };

    this.adminService.deleteWorker(id).subscribe(observer);
  }
}

