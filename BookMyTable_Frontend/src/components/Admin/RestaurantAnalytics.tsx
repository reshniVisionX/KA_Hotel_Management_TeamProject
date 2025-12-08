import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { getRestaurantAnalytics } from '../../Service/admin.api';
import type { RestaurantAnalytics } from '../../Types/Admin/Analytics';
import type { AppError } from '../../Types/Dashboard/ApiError';
import Toast, { type ToastType } from '../../Utils/Toast';
import '../../Styles/Admin/RestaurantAnalytics.css';

const RestaurantAnalyticsComponent = () => {
  const { restaurantId } = useParams<{ restaurantId: string }>();
  const [analytics, setAnalytics] = useState<RestaurantAnalytics[]>([]);
  const [loading, setLoading] = useState(true);
  const [toast, setToast] = useState<{ message: string; type: ToastType } | null>(null);

  useEffect(() => {
    if (restaurantId) {
      fetchAnalytics(parseInt(restaurantId));
    }
  }, [restaurantId]);

  const fetchAnalytics = async (id: number) => {
    try {
      setLoading(true);
      const response = await getRestaurantAnalytics(id);
      setAnalytics(response.data);
    } catch (err) {
      const error = err as AppError;
      setToast({ message: error.message, type: 'error' });
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return <div className="analytics-loading">Loading analytics...</div>;
  }

  if (analytics.length === 0) {
    return <div className="analytics-empty">No analytics data available</div>;
  }

  const data = analytics[0];

  return (
    <div className="restaurant-analytics-container">
      <h1 className="analytics-title">{data.restaurantName} Analytics</h1>
      
      <div className="analytics-grid">
        <div className="analytics-card">
          <h3>Daily Revenue</h3>
          <p className="revenue-amount">₹{data.dailyRevenue}</p>
        </div>
        
        <div className="analytics-card">
          <h3>Weekly Revenue</h3>
          <p className="revenue-amount">₹{data.weeklyRevenue}</p>
        </div>
        
        <div className="analytics-card">
          <h3>Monthly Revenue</h3>
          <p className="revenue-amount">₹{data.monthlyRevenue}</p>
        </div>
        
        <div className="analytics-card">
          <h3>Daily Orders</h3>
          <p className="order-count">{data.noOfDailyOrders}</p>
        </div>
        
        <div className="analytics-card">
          <h3>Weekly Orders</h3>
          <p className="order-count">{data.weeklyOrders}</p>
        </div>
        
        <div className="analytics-card">
          <h3>Monthly Orders</h3>
          <p className="order-count">{data.monthlyOrders}</p>
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

export default RestaurantAnalyticsComponent;
