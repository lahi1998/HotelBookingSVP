import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { worker } from '../../../interfaces/worker';
import { Observer } from 'rxjs';
import { Router } from '@angular/router';
import { AdminService } from '../../../services/adminService';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-adminWorker',
  standalone: false,
  templateUrl: './adminWorker.html',
  styleUrl: './adminWorker.css',
})
export class AdminWorker implements AfterViewInit{
  roles: string[] = ['Receptionist', 'Rengøring'];
  displayedColumns: string[] = ['role', 'username', 'fullname', 'buttons'];
  filterValue: string = '';
  dataSource = new MatTableDataSource<worker>(DATA);
  workerForm!: FormGroup;

    @ViewChild(MatPaginator) paginator!: MatPaginator;

    constructor(
    private fb: FormBuilder,
    private adminService: AdminService,
    private router: Router
  ) { }

    ngOnInit() {
    this.workerForm = this.fb.group({
      fullName: ['', Validators.required],
      role: ['', Validators.required],
      bedamount: ['', Validators.required],
    });
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }
    applyFilter() {
    this.dataSource.filter = this.filterValue.trim().toLowerCase();
  }

    onSubmit(): void {}

    DeleteRow(id: number) {
  
          const observer: Observer<worker> = {
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
const DATA: worker[] = [
  { role: 'Receptionist', username: 'Peje22', fullname: 'Peter hensen'},
  { role: 'Receptionist', username: 'Lahi2', fullname: 'Lars hinge'},
  { role: 'Rengøring', username: 'Pepe25', fullname: 'Peter pedersen'},
  { role: 'Rengøring', username: 'Pila22', fullname: 'Pippi langstrømpe'},
  { role: 'Receptionist', username: 'Mila1', fullname: 'Michael laudrup'},
  { role: 'Receptionist', username: 'Pesc3', fullname: 'Peter Scmeichel'}
];

