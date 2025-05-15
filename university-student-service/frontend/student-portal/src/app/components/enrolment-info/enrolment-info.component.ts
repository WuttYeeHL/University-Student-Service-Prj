import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { Enrolment } from '../../model/interface/enrolment';
import { ENROLMENT_TABLE_HEADERS } from '../../constant/Constant';
import { EnrolmentService } from '../../services/enrolment.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-enrolment-info',
  imports: [CommonModule, RouterModule],
  templateUrl: './enrolment-info.component.html',
  styleUrl: './enrolment-info.component.scss',
})
export class EnrolmentInfoComponent implements OnInit {
  tableHeaders = ENROLMENT_TABLE_HEADERS;
  enrolments: Enrolment[] = [];
  router = inject(Router);
  qualification: string = '';
  isLoading = true;

  constructor(
    private enrolmentService: EnrolmentService,
    private authService: AuthService
  ) {}

  async ngOnInit(): Promise<void> {
    const user = this.authService.currentUser;

    if (user?.userId) {
      this.enrolmentService.getEnrolments(user.userId).subscribe({
        next: (data) => {
          this.enrolments = data;
          if (this.enrolments.length > 1) {
            this.qualification = this.enrolments[1].qualification;
          }
          this.isLoading = false;
        },
        error: (err) => {
          console.error('Failed to load enrolments', err);
          this.isLoading = false;
        },
      });
    } else {
      this.router.navigate(['/login']);
    }
  }
}
