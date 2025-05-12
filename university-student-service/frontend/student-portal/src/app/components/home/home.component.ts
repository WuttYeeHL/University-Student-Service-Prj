import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'app-home',
  imports: [CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent {
  currentDate: string = '';
  nextWeekday: string = '';
  nextDate: string = '';
  weekDays = ['M', 'T', 'W', 'T', 'F', 'S', 'S'];
  visibleDates: Date[] = [];
  currentMonthName = '';
  currentYear = 0;

  ngOnInit(): void {
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
    startOfCalendar.setDate(
      referenceDate.getDate() - referenceDate.getDay() + 1
    ); // Monday start

    this.visibleDates = [];
    for (let i = 0; i < 7; i++) {
      const date = new Date(startOfCalendar);
      date.setDate(startOfCalendar.getDate() + i);
      this.visibleDates.push(date);
    }
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
