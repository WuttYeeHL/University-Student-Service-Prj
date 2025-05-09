import { Routes } from '@angular/router';
import { EnrolmentInfoComponent } from './components/enrolment-info/enrolment-info.component';
import { StudentInfoComponent } from './components/student-info/student-info.component';
import { HomeComponent } from './components/home/home.component';
import { StudentDocumentsComponent } from './components/student-documents/student-documents.component';
import { CourseInfoComponent } from './components/course-info/course-info.component';
import { CourseDetailComponent } from './components/course-detail/course-detail.component';

export const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'student-info', component: StudentInfoComponent },
  { path: 'enrolment-info', component: EnrolmentInfoComponent },
  { path: 'student-documents', component: StudentDocumentsComponent },
  { path: 'course-info', component: CourseInfoComponent },
  { path: 'course/:id', component: CourseDetailComponent }
];
