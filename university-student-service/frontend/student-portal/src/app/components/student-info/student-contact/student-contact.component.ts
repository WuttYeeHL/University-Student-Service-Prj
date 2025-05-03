import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { StudentService } from '../../../services/student.service';
import { Student } from '../../../model/interface/student.interface';


@Component({
  selector: 'app-student-contact',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './student-contact.component.html'
})
export class StudentContactComponent implements OnInit {
  student!: Student;

  constructor(private studentService: StudentService) {}

  ngOnInit() {
    this.studentService.student$.subscribe(data => {
      this.student = data;
    });
  }
}
