import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Student } from '../model/interface/student.interface'; 

@Injectable({
  providedIn: 'root'
})
export class StudentService {
  private studentData = new BehaviorSubject<Student>({
    name: 'testerstest',
    email: 'test@email.com',
    about: '',
    address: '',
    profileImage: null,

    permanentAddress: {
      streetNumber: '',
      streetName: '',
      suburb: '',
      city: '',
      country: 'NEW ZEALAND',
      postcode: '',
      phone: ''
    },
    studyAddress: {
      streetNumber: '',
      streetName: '',
      suburb: '',
      city: '',
      country: 'NEW ZEALAND',
      postcode: '',
      phone: ''
    },
    contactEmail: '',
    confirmEmail: '',
    emergencyContact: {
      name: '',
      relationship: ''
    }
  });

  student$ = this.studentData.asObservable();

  updateStudent(newData: Partial<Student>) {
    const current = this.studentData.value;
    this.studentData.next({ ...current, ...newData });
  }

  /*
  getStudentById(id: number): Student | undefined {
    
  }*/
}
