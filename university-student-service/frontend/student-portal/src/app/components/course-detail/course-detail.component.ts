import { Component } from '@angular/core';
import { ActivatedRoute, RouterModule, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { CourseInfoService } from '../../services/course-info.service';
import { AuthService } from '../../services/guards/auth-guard.service';
import { firstValueFrom } from 'rxjs';
import { iQualificationInfo } from '../../model/interface/courseinfo';
import { COURSE_INFO_TABLE_HEADERS, COURSE_API_URL } from '../../constant/Constant';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-course-detail',
  imports: [CommonModule, RouterModule],
  templateUrl: './course-detail.component.html',
  styleUrl: './course-detail.component.scss'
})
export class CourseDetailComponent {
  tableHeaders = COURSE_INFO_TABLE_HEADERS;
  qualification?: iQualificationInfo;
  downloadurl = `${COURSE_API_URL}/download?key=courseinfo-documents/`;
  
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private http: HttpClient,
    private authService: AuthService,
    private courseInfoService: CourseInfoService
  ) {}

  ngOnInit(): void {

    const id = Number(this.route.snapshot.paramMap.get('id'));
    const user = this.authService.currentUser;

    if (user?.userId) {
      if (!isNaN(id)) {
        this.courseInfoService.getQualificationById(id).subscribe((data: iQualificationInfo) => {
          this.qualification = data;
          }
          , (error) => {
            console.error('Error fetching qualification data:', error);
          });
      } else {
        console.error('Invalid course ID:', id);
      }
    } else {
      this.router.navigate(['/login']);
    }

  }

} 
