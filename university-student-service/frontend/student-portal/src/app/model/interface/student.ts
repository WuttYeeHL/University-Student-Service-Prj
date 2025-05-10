import { Education  } from './education';

export interface Student {
  student_Id: number;
  title: string;
  preferred_First_Name: string;
  preferred_Last_Name: string;
  gender: string;
  name: string;
  passport: string;
  country: string;
  dateOfBirth: string | null;
  profileImage: string;

  permanent_streetNumber: string;
  permanent_streetName: string;
  permanent_suburb: string;
  permanent_city: string;
  permanent_country: string;
  permanent_postcode: string;
  permanent_phone: string;

  study_streetNumber: string;
  study_streetName: string;
  study_suburb: string;
  study_city: string;
  study_country: string;
  study_postcode: string;
  study_phone: string;

  contact_Email: string;
  confirm_Email: string;

  emergencyContact_Name: string;
  emergencyContact_Relationship: string;

  user_Id: number | null;
  student_Code: string;

  tertiaryRecords: Education[];

  }
  