export interface AdminManager {
  managerId: number;
  managerName: string;
  userId: number;
  email: string;
  phoneNumber: string;
  passwordHash: string;
  isActive: boolean;
  createdAt: string;
  updatedAt: string;
  verification: number;
}

export const IsVerified ={
  Unverified : 0,
  Verified : 1,
  Rejected : 2
}
