import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { staffDto } from '../../../interfaces/staffDto';
import { Observer } from 'rxjs';
import { AdminService } from '../../../services/adminService';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CreateStaffRequest } from '../../../interfaces/createStaffRequest';
import { UpdateStaffRequest } from '../../../interfaces/updateStaffRequest';

@Component({
  selector: 'app-adminWorker',
  standalone: false,
  templateUrl: './adminWorker.html',
  styleUrl: './adminWorker.css',
})
export class AdminWorker implements AfterViewInit {
  roles: string[] = ['Admin', 'Receptionist', 'Cleaning'];
  displayedColumns: string[] = ['role', 'username', 'fullname', 'buttons'];
  filterValue: string = '';
  DATA: staffDto[] = [];
  dataSource = new MatTableDataSource<staffDto>(this.DATA);
  newWorkerForm!: FormGroup;
  editWorkerForm!: FormGroup;
  editopen: boolean = false;
  noEdit: boolean = false;
  editId: number = 0;

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(
    private fb: FormBuilder,
    private adminService: AdminService,
  ) { }

  ngOnInit() {
    this.newWorkerForm = this.fb.group({
      role: ['', Validators.required],
      userName: ['', Validators.required],
      password: ['', Validators.required],
      passwordConfirm: ['', Validators.required],
      fullName: ['', Validators.required],
    },
      { validators: this.passwordsMatchValidator }
    );

    this.editWorkerForm = this.fb.group({
      role: [{ value: '', disabled: this.noEdit }, Validators.required,],
      userName: [{ value: '', disabled: this.noEdit }, Validators.required],
      password: [''],
      passwordConfirm: [''],
      fullName: [{ value: '', disabled: this.noEdit }, Validators.required],
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

  createSubmit(): void {
    if (this.newWorkerForm.valid) {
      const newWorker: CreateStaffRequest = {
        role: this.newWorkerForm.value.role,
        userName: this.newWorkerForm.value.userName,
        password: this.newWorkerForm.value.password,
        fullName: this.newWorkerForm.value.fullName,
      };

      const observer: Observer<any> = {
        next: (response) => {
          console.log('Create successful.', response);
          this.getWorkers();
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



  /* edit workers */
  editSubmit() {
    if (this.editWorkerForm.valid) {
      const editWorker: UpdateStaffRequest = {
        id: this.editId,
        role: this.editWorkerForm.getRawValue().role,
        userName: this.editWorkerForm.getRawValue().userName,
        password: this.editWorkerForm.value.password,
        fullName: this.editWorkerForm.getRawValue().fullName,
      };

      const observer: Observer<any> = {
        next: (response) => {
          //console.log('Create successful.', response);
          this.getWorkers();
        },
        error: (error) => {
          console.error('Create error.', error);
          alert('Redigereing fejlede!');
        },
        complete: () => {
          // optional cleanup or navigation
        },
      };

      this.adminService.putWorker(editWorker).subscribe(observer);
    }
  }


  openEdit(id: number, userName: string) {
    /* Assuming you have a flag to toggle the edit form visibility */
    this.editopen = true;
    this.editId = id;
    if (userName === "admin") {
      this.noEdit = true
    }

    /* Find the worker with the matching id from the current DATA array */
    const workerEdit = this.DATA.find(w => w.id === id);

    if (!workerEdit) {
      console.error('Worker not found for edit:', id);
      alert('Worker not found!');
      return;
    }

    /* Patch form values with existing worker data */
    this.editWorkerForm.patchValue({
      role: workerEdit.role,
      userName: workerEdit.userName,
      fullName: workerEdit.fullName,
      /* Password fields left blank intentionally (for security) */
      password: '',
      passwordConfirm: ''
    });

          /* Disable ui input if noEdit is true */
      if (this.noEdit) {
        this.editWorkerForm.get('role')?.disable();
        this.editWorkerForm.get('userName')?.disable();
        this.editWorkerForm.get('fullName')?.disable();
      } else {
        this.editWorkerForm.get('role')?.enable();
        this.editWorkerForm.get('userName')?.enable();
        this.editWorkerForm.get('fullName')?.enable();
      }

  }

  closeEdit() {
    this.editopen = false;
    this.editId = 0;
  }
}


