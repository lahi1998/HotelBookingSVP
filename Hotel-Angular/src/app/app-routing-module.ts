import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Login } from './components/login/login';

const routes: Routes = [
  { path: 'login', component: Login },
  {
    path: 'Components',
    children: [
    ],
  },
  // Add a default route for unauthenticated users (can be an empty component)
  { path: 'default', component: Login },
  { path: '**', redirectTo: 'default' }, // Handle any other invalid routes by redirecting to 'default'
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
