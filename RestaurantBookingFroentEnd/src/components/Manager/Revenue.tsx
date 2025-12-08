import { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import type { RootState } from '../../redux/store';
import { getManagerRestaurant, getOrderSummary, getDailyRevenue, getPaymentSummary } from '../../Service/manager.api';
import Toast, { type ToastType } from '../../Utils/Toast';
import type { OrderSummary, DailyRevenue } from '../../Types/Manager/Order';
import type { PaymentSummary } from '../../Types/Manager/Payment';
import type { AppError } from '../../Types/Dashboard/ApiError';
import '../../Styles/Manager/Revenue.css';

const Revenue = () => {
  const { user } = useSelector((state: RootState) => state.auth);
  const [loading, setLoading] = useState(true);
  const [orderData, setOrderData] = useState<OrderSummary | null>(null);
  const [paymentData, setPaymentData] = useState<PaymentSummary | null>(null);
  const [dailyRevenueData, setDailyRevenueData] = useState<DailyRevenue[]>([]);
  const [toast, setToast] = useState<{ message: string; type: ToastType } | null>(null);
  const [dateRange, setDateRange] = useState<{ startDate: string; endDate: string }>({
    startDate: '',
    endDate: ''
  });
  const [selectedPeriod, setSelectedPeriod] = useState<string>('all');

  useEffect(() => {
    if (user?.userId) {
      fetchRevenueData();
    }
  }, [user?.userId]);

  const fetchRevenueData = async (startDate?: string, endDate?: string) => {
    try {
      setLoading(true);
      
      const restaurantResponse = await getManagerRestaurant(user?.userId!);
      const restaurantId = restaurantResponse.data.restaurantId;
      
      const [orders, payments, dailyRevenue] = await Promise.all([
        getOrderSummary(restaurantId, startDate, endDate),
        getPaymentSummary(restaurantId, startDate, endDate),
        getDailyRevenue(restaurantId)
      ]);

      setOrderData(orders.data as OrderSummary);
      setPaymentData(payments.data as PaymentSummary);
      setDailyRevenueData(dailyRevenue.data as DailyRevenue[]);
    } catch (err) {
      const error = err as AppError;
      setToast({ message: error.message || 'Failed to fetch revenue data', type: 'error' });
    } finally {
      setLoading(false);
    }
  };

  const handlePeriodChange = (period: string) => {
    setSelectedPeriod(period);
    
    const today = new Date();
    let startDate = '';
    let endDate = today.toISOString().split('T')[0];
    
    switch (period) {
      case 'today':
        startDate = endDate;
        break;
      case 'week':
        const weekAgo = new Date(today);
        weekAgo.setDate(today.getDate() - 7);
        startDate = weekAgo.toISOString().split('T')[0];
        break;
      case 'month':
        const monthAgo = new Date(today);
        monthAgo.setMonth(today.getMonth() - 1);
        startDate = monthAgo.toISOString().split('T')[0];
        break;
      case 'year':
        const yearAgo = new Date(today);
        yearAgo.setFullYear(today.getFullYear() - 1);
        startDate = yearAgo.toISOString().split('T')[0];
        break;
      case 'all':
      default:
        fetchRevenueData();
        return;
    }
    
    fetchRevenueData(startDate, endDate);
  };



  const handleCustomDateRange = () => {
    if (!dateRange.startDate || !dateRange.endDate) {
      setToast({ message: 'Please select both start and end dates', type: 'error' });
      return;
    }
    
    setSelectedPeriod('custom');
    fetchRevenueData(dateRange.startDate, dateRange.endDate);
  };

  const formatCurrency = (amount: number) => {
    return new Intl.NumberFormat('en-IN', {
      style: 'currency',
      currency: 'INR'
    }).format(amount);
  };

  const formatDate = (date: string) => {
    return new Date(date).toLocaleDateString('en-IN', {
      month: 'short',
      day: 'numeric'
    });
  };

  if (loading) {
    return (
      <div className="revenue-container">
        <div className="revenue-loading">Loading revenue data...</div>
      </div>
    );
  }

  return (
    <div className="revenue-container">
      <div className="revenue-header">
        <h1>Revenue Management</h1>
        <p>Track and analyze your restaurant's financial performance</p>
      </div>

      <div className="revenue-filters">
        <div className="period-filters">
          <button 
            className={selectedPeriod === 'all' ? 'active' : ''}
            onClick={() => handlePeriodChange('all')}
          >
            All Time
          </button>
          <button 
            className={selectedPeriod === 'today' ? 'active' : ''}
            onClick={() => handlePeriodChange('today')}
          >
            Today
          </button>
          <button 
            className={selectedPeriod === 'week' ? 'active' : ''}
            onClick={() => handlePeriodChange('week')}
          >
            Last 7 Days
          </button>
          <button 
            className={selectedPeriod === 'month' ? 'active' : ''}
            onClick={() => handlePeriodChange('month')}
          >
            Last Month
          </button>
          <button 
            className={selectedPeriod === 'year' ? 'active' : ''}
            onClick={() => handlePeriodChange('year')}
          >
            Last Year
          </button>
        </div>
        
        <div className="custom-date-range">
          <input
            type="date"
            value={dateRange.startDate}
            onChange={(e) => setDateRange(prev => ({ ...prev, startDate: e.target.value }))}
            placeholder="Start Date"
          />
          <input
            type="date"
            value={dateRange.endDate}
            onChange={(e) => setDateRange(prev => ({ ...prev, endDate: e.target.value }))}
            placeholder="End Date"
          />
          <button onClick={handleCustomDateRange}>Apply Range</button>
        </div>
      </div>

      <div className="revenue-summary-grid">
        <div className="revenue-card total-revenue">
          <div className="revenue-card-header">
            <h3>💰 Total Revenue</h3>
          </div>
          <div className="revenue-card-content">
            <div className="revenue-main-value">{formatCurrency((orderData as any)?.totalRevenue || 0)}</div>
            <div className="revenue-label">Total Earnings</div>
          </div>
        </div>
        
        <div className="revenue-card today-revenue">
          <div className="revenue-card-header">
            <h3>📅 Today's Revenue</h3>
          </div>
          <div className="revenue-card-content">
            <div className="revenue-main-value">{formatCurrency((orderData as any)?.todayRevenue || 0)}</div>
            <div className="revenue-label">Today's Earnings</div>
          </div>
        </div>
        
        <div className="revenue-card total-orders">
          <div className="revenue-card-header">
            <h3>📊 Total Orders</h3>
          </div>
          <div className="revenue-card-content">
            <div className="revenue-main-value">{(orderData as any)?.totalOrders || 0}</div>
            <div className="revenue-label">Orders Processed</div>
          </div>
        </div>
        
        <div className="revenue-card avg-order">
          <div className="revenue-card-header">
            <h3>💳 Average Order Value</h3>
          </div>
          <div className="revenue-card-content">
            <div className="revenue-main-value">
              {formatCurrency(
                ((orderData as any)?.totalOrders > 0) 
                  ? ((orderData as any)?.totalRevenue / (orderData as any)?.totalOrders) 
                  : 0
              )}
            </div>
            <div className="revenue-label">Per Order</div>
          </div>
        </div>
      </div>

      <div className="revenue-charts-section">
        <div className="revenue-chart-card">
          <div className="chart-header">
            <h3>Daily Revenue Trend</h3>
          </div>
          <div className="daily-revenue-chart">
            {dailyRevenueData.length > 0 ? (
              <>
                <div className="y-axis">
                  {(() => {
                    const maxRevenue = Math.max(...dailyRevenueData.map(d => d.revenue), 1000);
                    const step = Math.ceil(maxRevenue / 5 / 100) * 100; // Round to nearest 100
                    const maxStep = Math.ceil(maxRevenue / step) * step;
                    
                    return [
                      <div key="5" className="y-axis-label">{formatCurrency(maxStep)}</div>,
                      <div key="4" className="y-axis-label">{formatCurrency(maxStep * 0.8)}</div>,
                      <div key="3" className="y-axis-label">{formatCurrency(maxStep * 0.6)}</div>,
                      <div key="2" className="y-axis-label">{formatCurrency(maxStep * 0.4)}</div>,
                      <div key="1" className="y-axis-label">{formatCurrency(maxStep * 0.2)}</div>,
                      <div key="0" className="y-axis-label">₹0</div>
                    ];
                  })()
                }
                </div>
                <div className="chart-container">
                  <div className="chart-bars">
                    {dailyRevenueData
                      .sort((a, b) => new Date(a.date).getTime() - new Date(b.date).getTime())
                      .slice(-14)
                      .map((dayData, index) => {
                      const maxRevenue = Math.max(...dailyRevenueData.map(d => d.revenue), 1000);
                      const height = (dayData.revenue / maxRevenue) * 100;
                      
                      // Color coding based on revenue amount
                      let barColor = '#dc3545'; // Red for < 600
                      if (dayData.revenue >= 3000) {
                        barColor = '#007bff'; // Blue for >= 3000
                      } else if (dayData.revenue >= 2000) {
                        barColor = '#28a745'; // Green for 2000-2999
                      } else if (dayData.revenue >= 1200) {
                        barColor = '#ffc107'; // Yellow for 1200-1999
                      } else if (dayData.revenue >= 600) {
                        barColor = '#fd7e14'; // Orange for 600-1199
                      }
                      
                      return (
                        <div key={index} className="chart-bar-container">
                          <div 
                            className="chart-bar"
                            style={{ 
                              height: `${Math.max(height, 5)}%`,
                              background: barColor
                            }}
                            title={`${formatDate(dayData.date)}: ${formatCurrency(dayData.revenue)}`}
                          ></div>
                          <div className="chart-label">{formatDate(dayData.date)}</div>
                        </div>
                      );
                    })
                  }
                  </div>
                </div>
              </>
            ) : (
              <div className="no-data">No daily revenue data available</div>
            )}
          </div>
        </div>

        <div className="revenue-breakdown-card">
          <div className="chart-header">
            <h3>Payment Method Breakdown</h3>
          </div>
          <div className="payment-breakdown">
            <div className="payment-item">
              <span className="payment-label cash">💵 Cash Payments</span>
              <span className="payment-value">{(paymentData as any)?.cashPayments || 0}</span>
            </div>
            <div className="payment-item">
              <span className="payment-label upi">📱 UPI Payments</span>
              <span className="payment-value">{(paymentData as any)?.upiPayments || 0}</span>
            </div>
          </div>
        </div>
      </div>

      <div className="revenue-details-section">
        <div className="revenue-stats-grid">
          <div className="stat-item">
            <span className="stat-label">Completed Orders</span>
            <span className="stat-value">{(orderData as any)?.completedOrders || 0}</span>
          </div>
          <div className="stat-item">
            <span className="stat-label">Pending Orders</span>
            <span className="stat-value">{(orderData as any)?.pendingOrders || 0}</span>
          </div>
          <div className="stat-item">
            <span className="stat-label">Today's Orders</span>
            <span className="stat-value">{(orderData as any)?.todayOrders || 0}</span>
          </div>
        </div>
      </div>



      <div className="admin-payout-section">
        <div className="payout-header">
          <h2>💳 Platform Settlement Management</h2>
          <p>Revenue sharing and settlement details for your restaurant</p>
        </div>
        
        <div className="payout-content">
          <div className="settlement-summary">
            <div className="settlement-card">
              <h3>📊 Revenue Breakdown</h3>
              <div className="settlement-details">
                <div className="settlement-item">
                  <div className="settlement-info">
                    <span className="settlement-title">Total Revenue Generated</span>
                    <span className="settlement-desc">All orders from {(orderData as any)?.totalOrders || 0} orders</span>
                  </div>
                  <span className="settlement-amount">{formatCurrency((orderData as any)?.totalRevenue || 0)}</span>
                </div>
                
                <div className="settlement-item commission">
                  <div className="settlement-info">
                    <span className="settlement-title">Platform Commission (30%)</span>
                    <span className="settlement-desc">Service fee for platform usage</span>
                  </div>
                  <span className="settlement-amount">-{formatCurrency(((orderData as any)?.totalRevenue || 0) * 0.3)}</span>
                </div>
                
                <div className="settlement-item pending">
                  <div className="settlement-info">
                    <span className="settlement-title">Restaurant Settlement (70%)</span>
                    <span className="settlement-desc">Amount payable to restaurant</span>
                  </div>
                  <span className="settlement-amount">{formatCurrency(((orderData as any)?.totalRevenue || 0) * 0.7)}</span>
                </div>
              </div>
            </div>
            
            <div className="settlement-card">
              <h3>📈 Settlement Analytics</h3>
              <div className="analytics-grid">
                <div className="analytics-item">
                  <span className="analytics-label">Today's Settlement</span>
                  <span className="analytics-value">{formatCurrency(((orderData as any)?.todayRevenue || 0) * 0.7)}</span>
                </div>
                <div className="analytics-item">
                  <span className="analytics-label">Avg. Order Commission</span>
                  <span className="analytics-value">{formatCurrency(
                    ((orderData as any)?.totalOrders > 0) 
                      ? (((orderData as any)?.totalRevenue / (orderData as any)?.totalOrders) * 0.3)
                      : 0
                  )}</span>
                </div>
                <div className="analytics-item">
                  <span className="analytics-label">Completed Orders</span>
                  <span className="analytics-value">{(orderData as any)?.completedOrders || 0}</span>
                </div>
                <div className="analytics-item">
                  <span className="analytics-label">Pending Orders</span>
                  <span className="analytics-value">{(orderData as any)?.pendingOrders || 0}</span>
                </div>
              </div>
            </div>
          </div>
          
          <div className="settlement-info-card">
            <h4>ℹ️ Settlement Information</h4>
            <div className="info-grid">
              <div className="info-item">
                <strong>Settlement Cycle:</strong> Weekly (Every Monday)
              </div>
              <div className="info-item">
                <strong>Commission Rate:</strong> 30% of order value
              </div>
              <div className="info-item">
                <strong>Payment Method:</strong> Bank Transfer
              </div>
              <div className="info-item">
                <strong>Processing Time:</strong> 2-3 business days
              </div>
            </div>
          </div>
        </div>
      </div>

      <div className="revenue-actions">
        <button className="refresh-btn" onClick={() => fetchRevenueData()}>
          🔄 Refresh Data
        </button>
        <button className="export-btn">
          📊 Export Report
        </button>
      </div>

      {toast && (
        <Toast
          message={toast.message}
          type={toast.type}
          onClose={() => setToast(null)}
        />
      )}
    </div>
  );
};

export default Revenue;