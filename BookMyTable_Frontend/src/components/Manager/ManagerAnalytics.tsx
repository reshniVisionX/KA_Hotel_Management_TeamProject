import { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import {
  Box,
  Typography,
  Button,
  CircularProgress,
  IconButton
} from '@mui/material';
import {
  Analytics as AnalyticsIcon,
  Refresh as RefreshIcon
} from '@mui/icons-material';
import type { RootState } from '../../redux/store';
import { getManagerRestaurant, getOrderSummary, getPaymentSummary, getCustomerSummary, getReservationSummary, getDailyRevenue } from '../../Service/manager.api';
import Toast, { type ToastType } from '../../Utils/Toast';
import type { OrderSummary, DailyRevenue } from '../../Types/Manager/Order';
import type { PaymentSummary } from '../../Types/Manager/Payment';
import type { CustomerSummary } from '../../Types/Manager/Customer';
import type { ReservationSummary } from '../../Types/Manager/Reservation';
import type { AppError } from '../../Types/Dashboard/ApiError';
import '../../Styles/Manager/ManagerAnalytics.css';

const ManagerAnalytics = () => {
  const { user } = useSelector((state: RootState) => state.auth);
  const [loading, setLoading] = useState(true);
  const [orderData, setOrderData] = useState<OrderSummary | null>(null);
  const [paymentData, setPaymentData] = useState<PaymentSummary | null>(null);
  const [customerData, setCustomerData] = useState<CustomerSummary | null>(null);
  const [reservationData, setReservationData] = useState<ReservationSummary | null>(null);
  const [dailyRevenueData, setDailyRevenueData] = useState<DailyRevenue[]>([]);
  const [toast, setToast] = useState<{ message: string; type: ToastType } | null>(null);

  useEffect(() => {
    if (user?.userId) {
      fetchAnalyticsData();
    }
  }, [user?.userId]);

  const fetchAnalyticsData = async () => {
    if (!user?.userId) return;
    
    try {
      setLoading(true);
      
      const restaurantResponse = await getManagerRestaurant(user.userId);
      const restaurantId = restaurantResponse.data.restaurantId;
      
      const [orders, payments, customers, reservations, dailyRevenue] = await Promise.all([
        getOrderSummary(restaurantId),
        getPaymentSummary(restaurantId),
        getCustomerSummary(restaurantId),
        getReservationSummary(restaurantId),
        getDailyRevenue(restaurantId)
      ]);



      setOrderData(orders.data);
      setPaymentData(payments.data);
      setCustomerData(customers.data);
      setReservationData(reservations.data);
      setDailyRevenueData(dailyRevenue.data);
    } catch (err) {
      const error = err as AppError;
      setToast({ message: error.message || 'Failed to fetch analytics data', type: 'error' });
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: 400 }}>
        <CircularProgress sx={{ color: 'var(--primary-color)' }} />
        <Typography sx={{ ml: 2, color: 'var(--text-color)' }}>Loading analytics data...</Typography>
      </Box>
    );
  }

  return (
    <Box sx={{ p: 3, width: '100%', maxWidth: '100vw', boxSizing: 'border-box', overflowX: 'hidden' }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
          <AnalyticsIcon sx={{ color: 'var(--primary-color)', fontSize: 32 }} />
          <Box>
            <Typography variant="h4" sx={{ fontWeight: 600, color: 'var(--text-color)' }}>
              Analytics Dashboard
            </Typography>
            <Typography variant="body1" sx={{ color: '#666' }}>
              Real-time insights for your restaurant
            </Typography>
          </Box>
        </Box>
        
        <Button
          variant="contained"
          startIcon={<RefreshIcon />}
          onClick={fetchAnalyticsData}
          sx={{ bgcolor: 'var(--primary-color)', '&:hover': { bgcolor: 'var(--primary-hover)' } }}
        >
          Refresh Data
        </Button>
      </Box>

      <div className="analytics-summary-grid">
        <div className="analytics-card revenue-analytics">
          <div className="analytics-card-header">
            <h3>💰 Total Revenue</h3>
          </div>
          <div className="analytics-card-content">
            <div className="analytics-main-value">₹{orderData?.totalRevenue || 0}</div>
            <div className="analytics-label">Revenue</div>
          </div>
        </div>
        
        <div className="analytics-card orders-analytics">
          <div className="analytics-card-header">
            <h3>📊 Total Orders</h3>
          </div>
          <div className="analytics-card-content">
            <div className="analytics-main-value">{orderData?.totalOrders || 0}</div>
            <div className="analytics-label">Orders</div>
          </div>
        </div>
        
        <div className="analytics-card customers-analytics">
          <div className="analytics-card-header">
            <h3>👥 Total Customers</h3>
          </div>
          <div className="analytics-card-content">
            <div className="analytics-main-value">{customerData?.totalCustomers || 0}</div>
            <div className="analytics-label">Customers</div>
          </div>
        </div>
        
        <div className="analytics-card reservations-analytics">
          <div className="analytics-card-header">
            <h3>📅 Reservations</h3>
          </div>
          <div className="analytics-card-content">
            <div className="analytics-main-value">{reservationData?.totalReservations || 0}</div>
            <div className="analytics-label">Bookings</div>
          </div>
        </div>
      </div>

      <div className="analytics-charts-grid">
        <div className="analytics-chart-card">
          <div className="chart-header">
            <h3>Order Status Distribution</h3>
          </div>
          <div className="chart-content">
            <div className="chart-item">
              <span className="chart-label completed">Completed</span>
              <span className="chart-value">{orderData?.completedOrders || 0}</span>
            </div>
            <div className="chart-item">
              <span className="chart-label pending">Pending</span>
              <span className="chart-value">{orderData?.pendingOrders || 0}</span>
            </div>
          </div>
        </div>
        
        <div className="analytics-chart-card">
          <div className="chart-header">
            <h3>Payment Methods</h3>
          </div>
          <div className="chart-content">
            <div className="chart-item">
              <span className="chart-label cash">Cash</span>
              <span className="chart-value">{paymentData?.cashPayments || 0}</span>
            </div>
            <div className="chart-item">
              <span className="chart-label upi">UPI</span>
              <span className="chart-value">{paymentData?.upiPayments || 0}</span>
            </div>
          </div>
        </div>
      </div>
      
      <div className="analytics-revenue-card">
        <div className="chart-header">
          <h3>Revenue Analysis</h3>
        </div>
        <div className="revenue-content">
          <div className="revenue-item">
            <span className="revenue-label">Today Revenue</span>
            <span className="revenue-value">₹{orderData?.todayRevenue || 0}</span>
          </div>
          <div className="revenue-item">
            <span className="revenue-label">Total Revenue</span>
            <span className="revenue-value">₹{orderData?.totalRevenue || 0}</span>
          </div>
        </div>
        
        <div className="revenue-bar-chart">
          <h4>Last 7 Days Revenue</h4>
          <div className="chart-graph">
            <div className="y-axis">
              {(() => {
                const revenues = dailyRevenueData.length > 0 
                  ? dailyRevenueData.map(d => d.revenue) 
                  : [orderData?.todayRevenue || 1740];
                const maxRevenue = Math.max(...revenues, 3000);
                const step = 750;
                const maxStep = Math.ceil(maxRevenue / step) * step;
                return [
                  <div key="4" className="y-label">₹{maxStep}</div>,
                  <div key="3" className="y-label">₹{maxStep - step}</div>,
                  <div key="2" className="y-label">₹{maxStep - step * 2}</div>,
                  <div key="1" className="y-label">₹{step}</div>,
                  <div key="0" className="y-label">₹0</div>
                ];
              })()}
            </div>
            
            <div className="chart-area">
              <div className="grid-lines">
                <div className="horizontal-line"></div>
                <div className="horizontal-line"></div>
                <div className="horizontal-line"></div>
                <div className="horizontal-line"></div>
                <div className="horizontal-line"></div>
              </div>
              
              <div className="bars-container">
                {(() => {
                  if (dailyRevenueData.length > 0) {
                    const revenues = dailyRevenueData.map(d => d.revenue);
                    const maxRevenue = Math.max(...revenues, 3000);
                    const step = 750;
                    const maxStep = Math.ceil(maxRevenue / step) * step;
                    
                    return dailyRevenueData.map((dayData, index) => {
                      const date = new Date(dayData.date);
                      const dayName = date.toLocaleDateString('en-US', { weekday: 'short' });
                      const height = dayData.revenue === 0 ? 0 : Math.max((dayData.revenue / maxStep) * 100, 10);

                      let barColor = '#e0e0e0';
                      if (dayData.revenue > 2250) {
                        barColor = '#4caf50';
                      } else if (dayData.revenue > 1500 && dayData.revenue <= 2250) {
                        barColor = '#ffeb3b';
                      } else if (dayData.revenue > 750 && dayData.revenue <= 1500) {
                        barColor = '#ff9800';
                      } else if (dayData.revenue > 0 && dayData.revenue <= 750) {
                        barColor = '#f44336';
                      }
                      
                      return (
                        <div key={index} className="bar-column">
                          <div 
                            className="bar" 
                            style={{height: `${height}%`, backgroundColor: barColor}}
                          ></div>
                          <div className="x-label">{dayName}</div>
                          <div className="bar-amount">₹{dayData.revenue}</div>
                        </div>
                      );
                    });
                  }
                  return <div>No revenue data available</div>;
                })()}
              </div>
            </div>
          </div>
        </div>
        
        <div className="pie-charts-section">
          <div className="combined-pie-chart-card">
            <h4>Orders & Payments Distribution</h4>
            <div className="combined-pie-container">
              <div className="pie-chart">
                {(() => {
                  const completed = orderData?.completedOrders || 0;
                  const pending = orderData?.pendingOrders || 0;
                  const cash = paymentData?.cashPayments || 0;
                  const upi = paymentData?.upiPayments || 0;
                  const total = completed + pending + cash + upi;
                  
                  if (total === 0) return <div>No data available</div>;
                  
                  let currentAngle = 0;
                  const createPath = (value: number, color: string) => {
                    const percentage = value / total;
                    const angle = percentage * 360;
                    const startAngle = currentAngle;
                    const endAngle = currentAngle + angle;
                    currentAngle += angle;
                    
                    const startX = 100 + 90 * Math.cos((startAngle - 90) * Math.PI / 180);
                    const startY = 100 + 90 * Math.sin((startAngle - 90) * Math.PI / 180);
                    const endX = 100 + 90 * Math.cos((endAngle - 90) * Math.PI / 180);
                    const endY = 100 + 90 * Math.sin((endAngle - 90) * Math.PI / 180);
                    
                    const largeArc = angle > 180 ? 1 : 0;
                    
                    return (
                      <path
                        key={color}
                        d={`M 100,100 L ${startX},${startY} A 90,90 0 ${largeArc},1 ${endX},${endY} L 100,100 Z`}
                        fill={color}
                      />
                    );
                  };
                  
                  return (
                    <svg viewBox="0 0 200 200" className="pie-svg">
                      {completed > 0 && createPath(completed, '#4caf50')}
                      {pending > 0 && createPath(pending, '#ffeb3b')}
                      {cash > 0 && createPath(cash, '#2196f3')}
                      {upi > 0 && createPath(upi, '#424242')}
                    </svg>
                  );
                })()}
                <div className="pie-center">
                  <div className="pie-total">{(orderData?.completedOrders || 0) + (orderData?.pendingOrders || 0) + (paymentData?.cashPayments || 0) + (paymentData?.upiPayments || 0)}</div>
                  <div className="pie-label">Total</div>
                </div>
              </div>
              <div className="combined-legend">
                <div className="legend-section">
                  <h5>Orders</h5>
                  <div className="legend-item">
                    <div className="legend-dot" style={{backgroundColor: '#4caf50'}}></div>
                    <span>Completed ({orderData?.completedOrders || 0})</span>
                  </div>
                  <div className="legend-item">
                    <div className="legend-dot" style={{backgroundColor: '#ffeb3b'}}></div>
                    <span>Pending ({orderData?.pendingOrders || 0})</span>
                  </div>
                </div>
                <div className="legend-section">
                  <h5>Payments</h5>
                  <div className="legend-item">
                    <div className="legend-dot" style={{backgroundColor: '#2196f3'}}></div>
                    <span>Cash ({paymentData?.cashPayments || 0})</span>
                  </div>
                  <div className="legend-item">
                    <div className="legend-dot" style={{backgroundColor: '#424242'}}></div>
                    <span>UPI ({paymentData?.upiPayments || 0})</span>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

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

export default ManagerAnalytics;