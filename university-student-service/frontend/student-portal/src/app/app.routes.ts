import { Routes } from '@angular/router';
import { EnrolmentInfoComponent } from './components/enrolment-info/enrolment-info.component';
import { StudentInfoComponent } from './components/student-info/student-info.component';
import { HomeComponent } from './components/home/home.component';
import { StudentDocumentsComponent } from './components/student-documents/student-documents.component';
import { CourseInfoComponent } from './components/course-info/course-info.component';
import { CourseDetailComponent } from './components/course-detail/course-detail.component';
import { LoginComponent } from './components/login/login.component';
import { MasterComponent } from './components/master/master.component';
import { authGuard } from './services/guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  {
    path: '',
    component: MasterComponent,
    children: [
      { path: 'home', component: HomeComponent, canActivate: [authGuard] },
      {
        path: 'student-info',
        component: StudentInfoComponent,
        canActivate: [authGuard],
      },
      {
        path: 'enrolment-info',
        component: EnrolmentInfoComponent,
        canActivate: [authGuard],
      },
      {
        path: 'student-documents',
        component: StudentDocumentsComponent,
        canActivate: [authGuard],
      },
      {
        path: 'course-info',
        component: CourseInfoComponent,
        canActivate: [authGuard],
      },
      {
        path: 'course/:id',
        component: CourseDetailComponent,
        canActivate: [authGuard],
      },
    ],
  },
];
