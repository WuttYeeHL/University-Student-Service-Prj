import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Student } from '../../model/interface/student';
import { Education } from '../../model/interface/education';
import { StudentService } from '../../services/student.service';
import { StudentEducationComponent } from "./student-education/student-education.component";
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-student-info',
  templateUrl: './student-info.component.html',
  styleUrls: ['./student-info.component.scss'],
  imports: [
    FormsModule,
    CommonModule,
    StudentEducationComponent
],
})

export class StudentInfoComponent implements OnInit {

  student: Student = this.getEmptyStudent();
  newEducation: Education = this.getEmptyEducation();

  activeTab: string = 'personal';
  defaultProfileImage = 'https://cdn-icons-png.flaticon.com/512/847/847969.png';  // or any placeholder image

  constructor(private studentService: StudentService, private authService: AuthService) {}

  ngOnInit(): void {
    this.loadStudent();
  }
 
  loadStudent(): void {
    const userId = 1; // You can change this if dynamic
    const user = this.authService.currentUser;
    const userId1 = user?.userId;
    console.log(user?.userId)
    this.studentService.getStudentByUserId(Number(user?.userId)).subscribe({
      next: (data: Student) => {
        this.student = data;  
        if (this.student.dateOfBirth) {
          this.student.dateOfBirth = this.student.dateOfBirth.slice(0, 10);
        }
      },
      error: (err) => {
        console.error('Failed to load student data:', err);
      }
    });
  }  

  showSuccessMessage = false;
  saveStudent(): void {    
    this.studentService.updateStudent(this.student).subscribe({
      next: () => {
        this.showSuccessMessage = true;
        setTimeout(() => {
          this.showSuccessMessage = false;
        }, 3000);
      },
      error: (err) => alert('Failed to save student data: ' + err.message)
    });
  }  

  addEducation(): void {
    if (!this.student.tertiaryRecords) {
      this.student.tertiaryRecords = [];
    }
    this.student.tertiaryRecords.push({...this.newEducation});
    this.newEducation = this.getEmptyEducation();
  }

  removeEducation(index: number): void {
    this.student.tertiaryRecords.splice(index, 1);
  }

  resetForm(): void {
    this.student = this.getEmptyStudent();
    this.newEducation = this.getEmptyEducation();
  }

  onProfileImageSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      const formData = new FormData();
      formData.append('file', file);
  
      this.studentService.uploadProfileImage(formData).subscribe({
        next: (res) => {
          this.student.profileImage = res.url; // Save the URL returned by backend
        },
        error: (err) => console.error('Failed to upload image:', err)
      });
    }
  }
  
  getProfileImageUrl(key: string): string {
    key = "profiles/" + key;
    return `https://uss-storage-bucket.s3.ap-southeast-2.amazonaws.com/${key}`;
  }  

  isFormInvalid(): boolean {
    return !this.student.preferred_First_Name || 
           !this.student.preferred_Last_Name || 
           !this.student.gender ||
           !this.student.dateOfBirth ||
           !this.student.country ||
           !this.student.permanent_streetNumber ||
           !this.student.permanent_streetName ||
           !this.student.permanent_city ||
           !this.student.contact_Email ||
           !this.student.confirm_Email ||
           !this.student.emergencyContact_Name;
  }
  

  // Helpers

  private getEmptyStudent(): Student {
    return {
      student_Id: 0,
      title: '',
      preferred_First_Name: '',
      preferred_Last_Name: '',
      gender: '',
      name: '',
      passport: '',
      country: '',
      dateOfBirth: '',
      profileImage: '',
      permanent_streetNumber: '',
      permanent_streetName: '',
      permanent_suburb: '',
      permanent_city: '',
      permanent_country: '',
      permanent_postcode: '',
      permanent_phone: '',
      study_streetNumber: '',
      study_streetName: '',
      study_suburb: '',
      study_city: '',
      study_country: '',
      study_postcode: '',
      study_phone: '',
      contact_Email: '',
      confirm_Email: '',
      emergencyContact_Name: '',
      emergencyContact_Relationship: '',
      user_Id: null,
      student_Code: '',
      tertiaryRecords: []
    };
  }

  private getEmptyEducation(): Education {
    return {
      yearFrom: '',
      yearTo: '',
      institution: '',
      qualification: '',
      location: '',
      completed: false,
      awaiting: false
    };
  }

}
