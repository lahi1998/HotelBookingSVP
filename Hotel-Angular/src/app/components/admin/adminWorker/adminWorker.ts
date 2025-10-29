import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { staffDto } from '../../../interfaces/staffDto';
import { Observer } from 'rxjs';
import { Router } from '@angular/router';
import { AdminService } from '../../../services/adminService';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CreateStaffRequest } from '../../../interfaces/createStaffRequest';

@Component({
  selector: 'app-adminWorker',
  standalone: false,
  templateUrl: './adminWorker.html',
  styleUrl: './adminWorker.css',
})
export class AdminWorker implements AfterViewInit {
  roles: string[] = ['Receptionist', 'Rengøring'];
  displayedColumns: string[] = ['role', 'username', 'fullname', 'buttons'];
  filterValue: string = '';
  dataSource = new MatTableDataSource<staffDto>(DATA);
  workerForm!: FormGroup;

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(
    private fb: FormBuilder,
    private adminService: AdminService,
    private router: Router
  ) { }

  ngOnInit() {
    this.workerForm = this.fb.group({
      role: ['', Validators.required],
      userName: ['', Validators.required],
      password: ['', Validators.required],
      passwordConfirm: ['', Validators.required],
      fullName: ['', Validators.required],
    });
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }
  applyFilter() {
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

      if (this.workerForm.value.password !== this.workerForm.value.passwordConfirm) {
        this.workerForm.get('passwordConfirm')?.setErrors({ mismatch: true });
      } else {
        this.workerForm.get('passwordConfirm')?.setErrors(null);
      }

      const observer: Observer<CreateStaffRequest> = {
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

  DeleteRow(id: number) {

    const observer: Observer<any> = {
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

    this.adminService.deleteWorker(id).subscribe(observer);
  }
}

// --- Mock data ---
const DATA: staffDto[] = [

];

