export interface Student {
    name: string;
    email: string;
    about: string;
    address: string;
    profileImage: string | ArrayBuffer | null;
  
    permanentAddress: {
      streetNumber: string;
      streetName: string;
      suburb: string;
      city: string;
      country: string;
      postcode: string;
      phone: string;
    };
  
    studyAddress: {
      streetNumber: string;
      streetName: string;
      suburb: string;
      city: string;
      country: string;
      postcode: string;
      phone: string;
    };
  
    contactEmail: string;
    confirmEmail: string;
  
    emergencyContact: {
      name: string;
      relationship: string;
    };
  }
  