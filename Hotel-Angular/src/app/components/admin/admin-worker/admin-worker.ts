import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { worker } from '../../../interfaces/worker';

@Component({
  selector: 'app-admin-worker',
  standalone: false,
  templateUrl: './admin-worker.html',
  styleUrl: './admin-worker.css',
})
export class AdminWorker implements AfterViewInit{
  roles: string[] = ['Receptionist', 'Rengøring'];
  displayedColumns: string[] = ['role', 'username', 'fullname', 'buttons'];
  filterValue: string = '';
  dataSource = new MatTableDataSource<worker>(DATA);

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }
    applyFilter() {
    this.dataSource.filter = this.filterValue.trim().toLowerCase();
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

