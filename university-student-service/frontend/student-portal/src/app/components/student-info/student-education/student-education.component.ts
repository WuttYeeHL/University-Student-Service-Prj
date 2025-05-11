import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Education } from '../../../model/interface/education';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-student-education',
  templateUrl: './student-education.component.html',
  imports: [CommonModule, FormsModule],
})

export class StudentEducationComponent {

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
    awaiting: false
  };

  addRecord() {
    if (this.newRecord.yearFrom && this.newRecord.yearTo && this.newRecord.institution) {
      this.educationAdded.emit({ ...this.newRecord });
      this.clearForm();
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
      awaiting: false
    };
  }
}
