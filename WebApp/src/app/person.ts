export interface Person {
  id: string;
  firstName: string;
  lastName: string;
  fullName: string;
  initials: string;
  hasPicture: boolean;
  pictureUrl: string | null;
  email?: string;
  phone?: string;
  address?: string;
  dateOfBirth?: string;
  company?: string;
  jobTitle?: string;
  notes?: string;
}
