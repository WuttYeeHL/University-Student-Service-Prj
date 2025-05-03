import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { StudentContactComponent } from './student-contact/student-contact.component';
import { StudentService } from '../../services/student.service';
import { Student } from '../../model/interface/student.interface';


@Component({
  selector: 'app-student-info',
  standalone: true,
  imports: [
    FormsModule,
    CommonModule,
    StudentContactComponent
  ],
  templateUrl: './student-info.component.html',
  styleUrl: './student-info.component.scss'
})
export class StudentInfoComponent implements OnInit {
  student!: Student;
  activeTab: string = 'contact';

  constructor(private studentService: StudentService) {}

  ngOnInit() {
    this.studentService.student$.subscribe(data => {
      this.student = data;
    });
  }

  submitForm() {
    alert('Changes saved for ' + this.student.name);
    this.studentService.updateStudent(this.student);
  }

  onImageSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = () => {
        this.studentService.updateStudent({ profileImage: reader.result });
      };
      reader.readAsDataURL(file);
    }
  }
}
