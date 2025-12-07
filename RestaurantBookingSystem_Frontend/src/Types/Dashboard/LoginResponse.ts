export interface LoginResponse {
  userId: number;
  firstName: string;
  lastName: string;
  email: string;
  mobile: string;
  roleName:string;
  lastLogin: string | null;
  token: string;
}

