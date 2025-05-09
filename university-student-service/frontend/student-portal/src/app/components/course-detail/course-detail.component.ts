import { Component } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { CourseInfoService } from '../../services/course-info.service';
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
  downloadurl = `${COURSE_API_URL}/download?key=`;
  
  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private courseInfoService: CourseInfoService
  ) {}

  ngOnInit(): void {

    const id = Number(this.route.snapshot.paramMap.get('id'));
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
  }

  // async downloadFile(s3LocName: string, outfileName: string): Promise<void> {
  //   try {
  //     const blob = await firstValueFrom(
  //       this.http.get(`${COURSE_API_URL}/download?key=${encodeURIComponent(s3LocName)}`, {
  //         responseType: 'blob'
  //       })
  //     );
  
  //     const link = document.createElement('a');
  //     link.href = URL.createObjectURL(blob);
  //     link.download = outfileName;
  //     link.click();
  //     URL.revokeObjectURL(link.href);
  //   } catch (err) {
  //     console.error('Download failed:', err);
  //   }
  // }
} 
