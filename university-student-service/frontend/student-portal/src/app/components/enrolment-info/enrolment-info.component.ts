import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { Enrolment } from '../../model/interface/enrolment';
import { ENROLMENT_TABLE_HEADERS } from '../../constant/Constant';
import { EnrolmentService } from '../../services/enrolment.service';

@Component({
  selector: 'app-enrolment-info',
  imports: [CommonModule, RouterModule],
  templateUrl: './enrolment-info.component.html',
  styleUrl: './enrolment-info.component.scss',
})
export class EnrolmentInfoComponent implements OnInit {
  tableHeaders = ENROLMENT_TABLE_HEADERS;
  enrolments: Enrolment[] = [];

  constructor(private enrolmentService: EnrolmentService) {}

  ngOnInit(): void {
    const userId = '1';
    this.enrolmentService.getEnrolments(userId).subscribe({
      next: (data) => (this.enrolments = data),
      error: (err) => console.error('Failed to load enrolments', err),
    });
  }
}
