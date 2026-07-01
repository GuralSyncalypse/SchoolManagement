export interface AcademicProfile {
  className: string;
  facultyName: string;
  majorName: string;
}

export interface FamilyRelationship {
  relativeName: string;
  relationshipType: string;
  phoneNumber: string;
}

export interface StudentResponse {
  studentID: string;
  studentName: string;
  gender: string;
  ethnicity: string;
  permanentAddress: string;

  academicProfile?: AcademicProfile;

  familyRelationships: FamilyRelationship[];
}

export interface StudentRequest {
  studentID: string;
  userID: string;

  studentName: string;
  gender: string;
  birthDate: Date;

  ethnicity: string;
  religion: string;
  nationality: string;

  birthPlace: string;

  citizenID: string;
  citizenIDIssueDate: Date;
  citizenIDIssuePlace: string;

  permanentAddress: string;
  temporaryAddress: string;
}
