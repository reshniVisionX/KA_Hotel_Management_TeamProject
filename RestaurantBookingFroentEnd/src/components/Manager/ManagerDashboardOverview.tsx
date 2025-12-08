import React, { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import { 
  Box, 
  Card, 
  CardContent, 
  Typography, 
  CircularProgress,
  Chip
} from "@mui/material";
import { 
  Assignment as OrdersIcon,
  AttachMoney as RevenueIcon,
  Payment as PaymentIcon,
  People as CustomersIcon
} from "@mui/icons-material";
import type { RootState } from "../../redux/store";
import { getManagerRestaurant, getOrderSummary, getPaymentSummary, getCustomerSummary } from "../../Service/manager.api";
import Toast, { type ToastType } from "../../Utils/Toast";
import type { OrderSummary } from "../../Types/Manager/Order";
import type { PaymentSummary } from "../../Types/Manager/Payment";
import type { CustomerSummary } from "../../Types/Manager/Customer";
import type { AppError } from "../../Types/Dashboard/ApiError";
import "../../Styles/Manager/ManagerDashboardOverview.css";

const ManagerDashboardOverview: React.FC = () => {
  const { user } = useSelector((state: RootState) => state.auth);
  const [loading, setLoading] = useState(true);
  const [orderSummary, setOrderSummary] = useState<OrderSummary | null>(null);
  const [paymentSummary, setPaymentSummary] = useState<PaymentSummary | null>(null);
  const [customerSummary, setCustomerSummary] = useState<CustomerSummary | null>(null);
  const [toast, setToast] = useState<{ message: string; type: ToastType } | null>(null);

  useEffect(() => {
    if (user?.userId) {
      loadDashboardData();
    }
  }, [user?.userId]);

  const loadDashboardData = async () => {
    try {
      setLoading(true);
      
      const restaurant = await getManagerRestaurant(user?.userId!);
      const restaurantId = restaurant.data.restaurantId;
      
      const [orders, payments, customers] = await Promise.all([
        getOrderSummary(restaurantId),
        getPaymentSummary(restaurantId),
        getCustomerSummary(restaurantId)
      ]);

      setOrderSummary(orders.data as OrderSummary);
      setPaymentSummary(payments.data as PaymentSummary);
      setCustomerSummary(customers.data as CustomerSummary);
    } catch (err) {
      const error = err as AppError;
      setToast({ message: error.message || 'Failed to load dashboard data', type: "error" });
    } finally {
      setLoading(false);
    }
  };

  const formatCurrency = (amount: number) => {
    return `₹${new Intl.NumberFormat('en-IN').format(amount)}`;
  };

  return (
    <Box sx={{ p: 3, width: '100%', maxWidth: '100%', boxSizing: 'border-box' }}>
      <Box sx={{ mb: 4 }}>
        <Typography variant="h4" sx={{ fontWeight: 600, color: 'var(--text-color)', mb: 1 }}>
          Dashboard Overview
        </Typography>
        <Typography variant="body1" sx={{ color: '#666' }}>
          Welcome back, {user?.firstName || 'Manager'}!
        </Typography>
      </Box>

      {loading ? (
        <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: 200 }}>
          <CircularProgress sx={{ color: 'var(--primary-color)' }} />
        </Box>
      ) : (
        <Box sx={{ 
          display: 'grid', 
          gridTemplateColumns: { xs: '1fr', sm: '1fr 1fr' },
          gap: 3 
        }}>
          <Card sx={{ 
            minHeight: 260, 
            background: 'var(--card-bg)', 
            border: '1px solid var(--card-border)',
            '&:hover': { transform: 'translateY(-4px)', boxShadow: 'var(--card-shadow-hover)' },
            transition: 'all 0.3s ease'
          }}>
            <CardContent sx={{ p: 3 }}>
              <Box sx={{ display: 'flex', alignItems: 'center', mb: 3 }}>
                <OrdersIcon sx={{ color: 'var(--primary-color)', fontSize: 40, mr: 2 }} />
                <Typography variant="h4" sx={{ fontWeight: 600, color: 'var(--text-color)' }}>
                  Orders Management
                </Typography>
              </Box>
              
              <Box sx={{ mb: 3 }}>
                <Typography variant="h2" sx={{ fontWeight: 700, color: 'var(--text-color)', mb: 1 }}>
                  {(orderSummary as any)?.totalOrders || 0}
                </Typography>
                <Typography variant="h6" sx={{ color: '#666', mb: 2 }}>
                  Total Orders
                </Typography>
              </Box>

              <Box sx={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 2, mb: 3 }}>
                <Box sx={{ textAlign: 'center', p: 2, bgcolor: 'rgba(255, 152, 0, 0.1)', borderRadius: 2 }}>
                  <Typography variant="h4" sx={{ fontWeight: 700, color: '#ff9800' }}>
                    {(orderSummary as any)?.pendingOrders || 0}
                  </Typography>
                  <Typography variant="body2" sx={{ color: '#666' }}>Pending</Typography>
                </Box>
                <Box sx={{ textAlign: 'center', p: 2, bgcolor: 'rgba(76, 175, 80, 0.1)', borderRadius: 2 }}>
                  <Typography variant="h4" sx={{ fontWeight: 700, color: '#4caf50' }}>
                    {(orderSummary as any)?.completedOrders || 0}
                  </Typography>
                  <Typography variant="body2" sx={{ color: '#666' }}>Completed</Typography>
                </Box>
              </Box>

              <Box sx={{ p: 2, bgcolor: 'rgba(255, 122, 0, 0.05)', borderRadius: 2 }}>
                <Typography variant="h5" sx={{ fontWeight: 600, color: 'var(--primary-color)' }}>
                  {(orderSummary as any)?.todayOrders || 0}
                </Typography>
                <Typography variant="body2" sx={{ color: '#666' }}>Today's Orders</Typography>
              </Box>
            </CardContent>
          </Card>

          <Card sx={{ 
            minHeight: 320, 
            background: 'var(--card-bg)', 
            border: '1px solid var(--card-border)',
            '&:hover': { transform: 'translateY(-4px)', boxShadow: 'var(--card-shadow-hover)' },
            transition: 'all 0.3s ease'
          }}>
            <CardContent sx={{ p: 4 }}>
              <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
                <RevenueIcon sx={{ color: 'var(--primary-color)', fontSize: 32, mr: 1.5 }} />
                <Typography variant="h5" sx={{ fontWeight: 600, color: 'var(--text-color)' }}>
                  Revenue Analytics
                </Typography>
              </Box>
              
              <Box sx={{ mb: 2 }}>
                <Typography variant="h3" sx={{ fontWeight: 700, color: 'var(--primary-color)', mb: 0.5 }}>
                  {formatCurrency((orderSummary as any)?.totalRevenue || 0)}
                </Typography>
                <Typography variant="body1" sx={{ color: '#666', mb: 1.5 }}>
                  Total Revenue
                </Typography>
              </Box>

              <Box sx={{ p: 2, bgcolor: 'rgba(255, 122, 0, 0.1)', borderRadius: 2, mb: 1.5 }}>
                <Typography variant="h5" sx={{ fontWeight: 700, color: 'var(--primary-color)' }}>
                  {formatCurrency((orderSummary as any)?.todayRevenue || 0)}
                </Typography>
                <Typography variant="body1" sx={{ color: '#666' }}>Today's Revenue</Typography>
              </Box>

              <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                <Typography variant="body2" sx={{ color: '#888' }}>Daily Average</Typography>
                <Typography variant="body1" sx={{ fontWeight: 600, color: 'var(--text-color)' }}>
                  {formatCurrency(((orderSummary as any)?.totalRevenue || 0) / 30)}
                </Typography>
              </Box>
            </CardContent>
          </Card>

          <Card sx={{ 
            minHeight: 260, 
            background: 'var(--card-bg)', 
            border: '1px solid var(--card-border)',
            '&:hover': { transform: 'translateY(-4px)', boxShadow: 'var(--card-shadow-hover)' },
            transition: 'all 0.3s ease'
          }}>
            <CardContent sx={{ p: 3 }}>
              <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
                <PaymentIcon sx={{ color: 'var(--primary-color)', fontSize: 32, mr: 1.5 }} />
                <Typography variant="h5" sx={{ fontWeight: 600, color: 'var(--text-color)' }}>
                  Payment Methods
                </Typography>
              </Box>
              
              <Box sx={{ mb: 3 }}>
                <Typography variant="h2" sx={{ fontWeight: 700, color: 'var(--text-color)', mb: 1 }}>
                  {formatCurrency((paymentSummary as any)?.totalAmount || 0)}
                </Typography>
                <Typography variant="h6" sx={{ color: '#666', mb: 2 }}>
                  Total Payments
                </Typography>
              </Box>

              <Box sx={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 2, mb: 3 }}>
                <Box sx={{ textAlign: 'center', p: 2, bgcolor: 'rgba(33, 150, 243, 0.1)', borderRadius: 2 }}>
                  <Typography variant="h4" sx={{ fontWeight: 700, color: '#2196f3' }}>
                    {(paymentSummary as any)?.cashPayments || 0}
                  </Typography>
                  <Typography variant="body2" sx={{ color: '#666' }}>Cash</Typography>
                </Box>
                <Box sx={{ textAlign: 'center', p: 2, bgcolor: 'rgba(156, 39, 176, 0.1)', borderRadius: 2 }}>
                  <Typography variant="h4" sx={{ fontWeight: 700, color: '#9c27b0' }}>
                    {(paymentSummary as any)?.upiPayments || 0}
                  </Typography>
                  <Typography variant="body2" sx={{ color: '#666' }}>UPI</Typography>
                </Box>
              </Box>

              <Box sx={{ p: 2, bgcolor: 'rgba(255, 122, 0, 0.05)', borderRadius: 2 }}>
                <Typography variant="h5" sx={{ fontWeight: 600, color: 'var(--primary-color)' }}>
                  {(paymentSummary as any)?.todayPayments || 0}
                </Typography>
                <Typography variant="body2" sx={{ color: '#666' }}>Today's Payments</Typography>
              </Box>
            </CardContent>
          </Card>

          <Card sx={{ 
            minHeight: 260, 
            background: 'var(--card-bg)', 
            border: '1px solid var(--card-border)',
            '&:hover': { transform: 'translateY(-4px)', boxShadow: 'var(--card-shadow-hover)' },
            transition: 'all 0.3s ease'
          }}>
            <CardContent sx={{ p: 3 }}>
              <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
                <CustomersIcon sx={{ color: 'var(--primary-color)', fontSize: 32, mr: 1.5 }} />
                <Typography variant="h5" sx={{ fontWeight: 600, color: 'var(--text-color)' }}>
                  Customer Base
                </Typography>
              </Box>
              
              <Box sx={{ mb: 3 }}>
                <Typography variant="h2" sx={{ fontWeight: 700, color: 'var(--text-color)', mb: 1 }}>
                  {(customerSummary as any)?.totalCustomers || 0}
                </Typography>
                <Typography variant="h6" sx={{ color: '#666', mb: 2 }}>
                  Total Customers
                </Typography>
              </Box>

              <Box sx={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 2, mb: 3 }}>
                <Box sx={{ textAlign: 'center', p: 2, bgcolor: 'rgba(76, 175, 80, 0.1)', borderRadius: 2 }}>
                  <Typography variant="h4" sx={{ fontWeight: 700, color: '#4caf50' }}>
                    {(customerSummary as any)?.recentCustomers || 0}
                  </Typography>
                  <Typography variant="body2" sx={{ color: '#666' }}>Recent</Typography>
                </Box>
                <Box sx={{ textAlign: 'center', p: 2, bgcolor: 'rgba(255, 193, 7, 0.1)', borderRadius: 2 }}>
                  <Typography variant="h4" sx={{ fontWeight: 700, color: '#ffc107' }}>
                    {(customerSummary as any)?.frequentCustomers || 0}
                  </Typography>
                  <Typography variant="body2" sx={{ color: '#666' }}>Frequent</Typography>
                </Box>
              </Box>

              <Box sx={{ p: 2, bgcolor: 'rgba(255, 184, 12, 0.1)', borderRadius: 2 }}>
                <Typography variant="h5" sx={{ fontWeight: 600, color: 'var(--secondary-color)' }}>
                  {(customerSummary as any)?.newCustomersThisMonth || 0}
                </Typography>
                <Typography variant="body2" sx={{ color: '#666' }}>New This Month</Typography>
              </Box>
            </CardContent>
          </Card>
        </Box>
      )}

      {toast && (
        <Toast
          message={toast.message}
          type={toast.type}
          onClose={() => setToast(null)}
        />
      )}
    </Box>
  );
};

export default ManagerDashboardOverview;