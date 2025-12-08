import { useEffect, useState } from 'react';
import { getDashboardAnalytics } from '../../Service/admin.api';
import type { DashboardAnalytics } from '../../Types/Admin/Analytics';
import type { AppError } from '../../Types/Dashboard/ApiError';
import Toast, { type ToastType } from '../../Utils/Toast';
import '../../Styles/Admin/RevenueAnalysis.css';

const RevenueAnalysis = () => {
  const [analytics, setAnalytics] = useState<DashboardAnalytics | null>(null);
  const [loading, setLoading] = useState(true);
  const [toast, setToast] = useState<{ message: string; type: ToastType } | null>(null);

  useEffect(() => {
    fetchAnalytics();
  }, []);

  const fetchAnalytics = async () => {
    try {
      setLoading(true);
      const response = await getDashboardAnalytics();
      setAnalytics(response.data);
    } catch (err) {
      const error = err as AppError;
      setToast({ message: error.message, type: 'error' });
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return <div className="revenue-loading">Loading dashboard analytics...</div>;
  }

  if (!analytics) {
    return <div className="no-data">No analytics data available</div>;
  }

  return (
    <div className="revenue-analysis-container">
      <h1 className="revenue-title">Dashboard Analytics</h1>
      
      <div className="analytics-grid">
        <div className="analytics-card">
          <h3>Restaurants</h3>
          <p className="metric-value">{analytics.noOfRestaurants}</p>
        </div>
        
        <div className="analytics-card">
          <h3>Total Users</h3>
          <p className="metric-value">{analytics.noOfUsers}</p>
        </div>
        
        <div className="analytics-card">
          <h3>Active Users</h3>
          <p className="metric-value">{analytics.noOfActiveUsers}</p>
        </div>
        
        <div className="analytics-card">
          <h3>Total Managers</h3>
          <p className="metric-value">{analytics.noOfManagers}</p>
        </div>
        
        <div className="analytics-card">
          <h3>Active Managers</h3>
          <p className="metric-value">{analytics.noOfActiveManagers}</p>
        </div>
        
    
        
        
        
        <div className="analytics-card">
          <h3>Dine Out Orders</h3>
          <p className="metric-value orders">{analytics.dineOutOrders}</p>
        </div>
        
        <div className="analytics-card">
          <h3>Vegetarian Hotels</h3>
          <p className="metric-value">{analytics.noOfVegetarianHotels}</p>
        </div>
        
        <div className="analytics-card">
          <h3>Non-Vegetarian Hotels</h3>
          <p className="metric-value">{analytics.noOfNonVegetarianHotels}</p>
        </div>
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

export default RevenueAnalysis;
