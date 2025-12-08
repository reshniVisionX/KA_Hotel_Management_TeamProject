export interface Customer {
  userId: number;
  firstName: string;
  lastName: string;
  email: string;
  isActive: boolean;
  createdAt: string;
  lastLogin?: string;
}

export interface CustomerSummary {
  totalCustomers: number;
  recentCustomers: number;
  frequentCustomers: number;
  newCustomersThisMonth: number;
}