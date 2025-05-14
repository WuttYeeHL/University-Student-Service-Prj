import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Education } from '../../../model/interface/education';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-student-education',
  standalone: true,
  templateUrl: './student-education.component.html',
  imports: [CommonModule, FormsModule],
})
export class StudentEducationComponent {
  years: number[] = [];
  yearRangeError: boolean = false;

  @Input() records: Education[] = [];
  @Output() educationAdded = new EventEmitter<Education>();
  @Output() educationRemoved = new EventEmitter<number>();

  newRecord: Education = {
    yearFrom: '',
    yearTo: '',
    institution: '',
    qualification: '',
    location: 'NEW ZEALAND',
    completed: false,
    awaiting: false,
  };

  ngOnInit() {
    const currentYear = new Date().getFullYear();
    const startYear = currentYear - 40;
    for (let year = currentYear; year >= startYear; year--) {
      this.years.push(year);
    }
  }
  checkboxError: boolean = false;

addRecord(form: NgForm) {
  this.yearRangeError = false;
  this.checkboxError = false; // clear old error

  // Year range check
  if (this.newRecord.yearFrom && this.newRecord.yearTo &&
      +this.newRecord.yearFrom > +this.newRecord.yearTo) {
    this.yearRangeError = true;
    return;
  }

  // Checkbox validation
  if (!this.newRecord.completed && !this.newRecord.awaiting) {
    this.checkboxError = true;
    return;
  }

  // Standard validation
  if (form.valid) {
    this.records.push({ ...this.newRecord });
    this.clearForm();
    form.resetForm({ location: 'NEW ZEALAND' });
  } else {
    form.control.markAllAsTouched();
  }
}



  removeRecord(index: number) {
    this.educationRemoved.emit(index);
  }

  private clearForm() {
    this.newRecord = {
      yearFrom: '',
      yearTo: '',
      institution: '',
      qualification: '',
      location: 'NEW ZEALAND',
      completed: false,
      awaiting: false,
    };
  }
}
