import { useEffect, useState } from 'react';
import { useAppSelector } from '../../redux/hooks';
import { Navbar } from '../Customer/Navbar';
import '../../Styles/UserDashboard/MyOrders.css';
import type { Order } from '../../Types/UserDashboard/Orders';
import type { AppError } from '../../Types/Dashboard/ApiError';
import { getUserOrders } from '../../Service/dashboard.api';
import myOrdersImage from '../../assets/Myorders.jpeg.jpg';

const MyOrders = () => {
  const [orders, setOrders] = useState<Order[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string>('');

  const user = useAppSelector((state) => state.auth.user);

  useEffect(() => {
    fetchOrders();
  }, [user?.userId]);

  const fetchOrders = async () => {
    try {
      setLoading(true);
      setError('');
      if (user?.userId) {
        const response = await getUserOrders(user.userId);
        const userOrders = response.data;
        if (Array.isArray(userOrders)) {
          setOrders(userOrders);
        } else {
          setOrders([]);
        }
      } else {
        setOrders([]);
      }
    } catch (err) {
      const error = err as AppError;
      setError(error.message || 'Failed to fetch orders');
      setOrders([]);
    } finally {
      setLoading(false);
    }
  };

  const getStatusColor = (status: number) => {
    switch (status) {
      case 0: return 'var(--warning-color)'; // Pending
      case 1: return 'var(--info-color)'; // Confirmed
      case 2: return 'var(--primary-color)'; // Preparing
      case 3: return 'var(--success-color)'; // Delivered
      case 4: return 'var(--error-color)'; // Cancelled
      default: return 'var(--text-secondary)';
    }
  };

  const getStatusText = (status: number) => {
    switch (status) {
      case 0: return 'PENDING';
      case 1: return 'CONFIRMED';
      case 2: return 'PREPARING';
      case 3: return 'DELIVERED';
      case 4: return 'CANCELLED';
      default: return 'UNKNOWN';
    }
  };

  if (loading) {
    return (
      <>
        <Navbar />
        <div className="myorders-container">
          <div className="myorders-loading">Loading orders...</div>
        </div>
      </>
    );
  }

  if (error) {
    return (
      <>
        <Navbar />
        <div className="myorders-container">
          <div className="myorders-error">{error}</div>
        </div>
      </>
    );
  }

  return (
    <>
      <Navbar />
      <div className="myorders-container" style={{ paddingTop: '64px' }}>
        <div className="myorders-header">
          <h1 className="myorders-title">My Orders</h1>
        </div>
        {orders.length === 0 ? (
          <div className="myorders-empty">
            <p>No orders found. Place your first order!</p>
          </div>
        ) : (
          <div className="myorders-grid">
            {orders.map((order: any) => (
              <div key={order.orderId} className="myorders-card">
                <img 
                  src={myOrdersImage} 
                  alt="Order" 
                  className="myorders-image"
                />
                <div className="myorders-details">
                  <div className="myorders-info-row">
                    <span className="myorders-label">Order ID:</span>
                    <span className="myorders-value">{order.orderId}</span>
                  </div>
                  <div className="myorders-info-row">
                    <span className="myorders-label">Restaurant:</span>
                    <span className="myorders-value">{order.restaurantName || `Restaurant #${order.restaurantId}`}</span>
                  </div>
                  <div className="myorders-info-row">
                    <span className="myorders-label">Customer:</span>
                    <span className="myorders-value">{order.userName}</span>
                  </div>
                  <div className="myorders-info-row">
                    <span className="myorders-label">Quantity:</span>
                    <span className="myorders-value">{order.qtyOrdered}</span>
                  </div>
                  <div className="myorders-info-row">
                    <span className="myorders-label">Status:</span>
                    <span className="myorders-value" style={{ color: getStatusColor(order.status) }}>
                      {getStatusText(order.status)}
                    </span>
                  </div>
                  <div className="myorders-info-row">
                    <span className="myorders-label">Date:</span>
                    <span className="myorders-value">{new Date(order.orderDate).toLocaleDateString()}</span>
                  </div>
                  <div className="myorders-info-row">
                    <span className="myorders-label">Total:</span>
                    <span className="myorders-value myorders-amount">${order.totalAmount}</span>
                  </div>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </>
  );
};

export default MyOrders;