import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { EnrolmentService } from '../../services/enrolment.service';
import { Enrolment } from '../../model/interface/enrolment';
import { HOME_CONSTANTS } from '../../constant/Constant';

@Component({
  selector: 'app-home',
  imports: [CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent {
  readonly constants = HOME_CONSTANTS;

  currentDate: string = '';
  nextWeekday: string = '';
  nextDate: string = '';
  weekDays = this.constants.WEEK_DAYS;
  visibleDates: Date[] = [];
  currentMonthName = '';
  currentYear = 0;
  enrolments: Enrolment[] = [];
  isLoading = true;
  router = inject(Router);

  constructor(
    private enrolmentService: EnrolmentService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    const user = this.authService.currentUser;
    if (user?.userId) {
      this.enrolmentService.getEnrolments(user.userId).subscribe({
        next: (data) => {
          this.enrolments = data;
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

    const today = new Date();
    this.currentMonthName = today.toLocaleString('en-NZ', { month: 'long' });
    this.currentYear = today.getFullYear();
    this.generateVisibleDates(today);

    this.currentDate = today.toLocaleDateString('en-NZ', {
      day: 'numeric',
      month: 'long',
      year: 'numeric',
    });

    const next = new Date(today);
    do {
      next.setDate(next.getDate() + 1);
    } while (next.getDay() === 0 || next.getDay() === 6);

    this.nextWeekday = next.toLocaleDateString('en-NZ', { weekday: 'short' });
    this.nextDate = next.toLocaleDateString('en-NZ', {
      day: 'numeric',
      month: 'long',
      year: 'numeric',
    });
  }

  generateVisibleDates(referenceDate: Date): void {
    const startOfCalendar = new Date(referenceDate);
    const day = referenceDate.getDay();
    const diff = day === 0 ? -6 : 1 - day;
    startOfCalendar.setDate(referenceDate.getDate() + diff);

    this.visibleDates = Array.from({ length: 7 }, (_, i) => {
      const date = new Date(startOfCalendar);
      date.setDate(startOfCalendar.getDate() + i);
      return date;
    });
  }

  isToday(date: Date): boolean {
    const today = new Date();
    return (
      date.getDate() === today.getDate() &&
      date.getMonth() === today.getMonth() &&
      date.getFullYear() === today.getFullYear()
    );
  }
}
