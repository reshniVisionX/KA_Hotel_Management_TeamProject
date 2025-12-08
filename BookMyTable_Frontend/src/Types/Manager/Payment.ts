export interface Payment {
  paymentId: number;
  orderId: number;
  amount: number;
  paymentDate: string;
  payMethod: PayMethod;
  status: PaymentStatus;
}

export const PayMethod = {
  Cash: 0,
  UPI: 1,
  Card: 2
} as const;

export type PayMethod = typeof PayMethod[keyof typeof PayMethod];

export const PaymentStatus = {
  Pending: 0,
  Completed: 1,
  Failed: 2,
  Refunded: 3
} as const;

export type PaymentStatus = typeof PaymentStatus[keyof typeof PaymentStatus];

export interface PaymentSummary {
  totalPayments: number;
  todayPayments: number;
  completedPayments: number;
  pendingPayments: number;
  totalAmount: number;
  todayAmount: number;
  cashPayments: number;
  upiPayments: number;
}