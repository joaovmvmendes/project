import { bootstrapApplication } from '@angular/platform-browser';
import { provideRouter, Route } from '@angular/router';
import { LoginComponent } from './app/features/auth/components/login/login.component';
import { TaskListComponent } from './app/features/task/pages/task-list/task-list.component';
import { AppComponent } from './app/app.component';
import { Routes } from '@angular/router';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: '**', redirectTo: '/login' },
  { path: 'tasks', component: TaskListComponent },
];

bootstrapApplication(AppComponent, {
  providers: [provideRouter(routes)],
});
