import { Component } from '@angular/core';

@Component({
  selector: 'app-home',
  imports: [],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent {
  currentDate: string = '';
  nextWeekday: string = '';
  nextDate: string = '';

  ngOnInit(): void {
    const today = new Date();

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
}
