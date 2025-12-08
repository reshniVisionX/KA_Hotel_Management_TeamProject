import { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import type { RootState } from '../../redux/store';
import { getManagerRestaurant, getOrdersByRestaurant, getTodayOrders, getOrderHistoryByRestaurant } from '../../Service/manager.api';
import Toast, { type ToastType } from '../../Utils/Toast';
import type { Order } from '../../Types/Manager/Order';
import type { AppError } from '../../Types/Dashboard/ApiError';
import '../../Styles/Manager/OrderManagement.css';

const OrderManagement = () => {
  const { user } = useSelector((state: RootState) => state.auth);
  const [loading, setLoading] = useState(true);
  const [orders, setOrders] = useState<Order[]>([]);
  const [filteredOrders, setFilteredOrders] = useState<Order[]>([]);
  const [toast, setToast] = useState<{ message: string; type: ToastType } | null>(null);
  const [restaurantId, setRestaurantId] = useState<number | null>(null);
  const [statusFilter, setStatusFilter] = useState<string>('all');
  const [dateFilter, setDateFilter] = useState<string>('all');
  const [startDate, setStartDate] = useState<string>('');
  const [endDate, setEndDate] = useState<string>('');

  useEffect(() => {
    if (user?.userId) {
      fetchOrders();
    }
  }, [user?.userId]);

  useEffect(() => {
    applyFilters();
  }, [orders, statusFilter, dateFilter, startDate, endDate]);

  const fetchOrders = async () => {
    try {
      setLoading(true);
      
      const restaurant = await getManagerRestaurant(user?.userId!);
      const restId = restaurant.data.restaurantId;
      setRestaurantId(restId);
      
      const ordersResponse = await getOrdersByRestaurant(restId);
      setOrders((ordersResponse.data as Order[]) || []);
    } catch (err) {
      const error = err as AppError;
      setToast({ message: error.message || 'Failed to fetch orders', type: 'error' });
    } finally {
      setLoading(false);
    }
  };

  const fetchTodayOrders = async () => {
    if (!restaurantId) return;
    
    try {
      setLoading(true);
      const ordersResponse = await getTodayOrders(restaurantId);
      setOrders((ordersResponse.data as Order[]) || []);
    } catch (err) {
      const error = err as AppError;
      setToast({ message: error.message || 'Failed to fetch today\'s orders', type: 'error' });
    } finally {
      setLoading(false);
    }
  };

  const fetchOrderHistory = async () => {
    if (!restaurantId) return;
    
    try {
      setLoading(true);
      const historyResponse = await getOrderHistoryByRestaurant(restaurantId);
      setOrders((historyResponse.data as Order[]) || []);
    } catch (err) {
      const error = err as AppError;
      setToast({ message: error.message || 'Failed to fetch order history', type: 'error' });
    } finally {
      setLoading(false);
    }
  };

  const fetchOrdersByDateRange = async () => {
    if (!restaurantId || !startDate || !endDate) {
      setToast({ message: 'Please select both start and end dates', type: 'error' });
      return;
    }
    
    try {
      setLoading(true);
      const ordersResponse = await getOrdersByRestaurant(restaurantId, startDate, endDate);
      setOrders((ordersResponse.data as Order[]) || []);
      setDateFilter('range');
    } catch (err) {
      const error = err as AppError;
      setToast({ message: error.message || 'Failed to fetch orders for date range', type: 'error' });
    } finally {
      setLoading(false);
    }
  };

  const applyFilters = () => {
    let filtered = [...orders];

    // Status filter
    if (statusFilter !== 'all') {
      const statusValue = getStatusValue(statusFilter);
      if (statusValue !== -1) {
        filtered = filtered.filter(order => order.status === statusValue);
      }
    }

    // Date filter
    if (dateFilter === 'today') {
      const today = new Date().toDateString();
      filtered = filtered.filter(order => 
        new Date(order.orderDate).toDateString() === today
      );
    } else if (dateFilter === 'range' && startDate && endDate) {
      const start = new Date(startDate);
      const end = new Date(endDate);
      end.setHours(23, 59, 59, 999); // Include the entire end date
      
      filtered = filtered.filter(order => {
        const orderDate = new Date(order.orderDate);
        return orderDate >= start && orderDate <= end;
      });
    }

    // Sort by order date in descending order (newest first)
    filtered.sort((a, b) => new Date(b.orderDate).getTime() - new Date(a.orderDate).getTime());
    
    setFilteredOrders(filtered);
  };

  const getStatusValue = (statusName: string): number => {
    switch (statusName.toLowerCase()) {
      case 'pending': return 0;
      case 'inprogress': return 1;
      case 'completed': return 2;
      case 'cancelled': return 3;
      default: return -1;
    }
  };

  const getStatusColor = (status: number) => {
    switch (status) {
      case 0: return 'pending'; // Pending
      case 1: return 'in-progress'; // InProgress
      case 2: return 'completed'; // Completed
      case 3: return 'cancelled'; // Cancelled
      default: return 'pending';
    }
  };

  const getStatusText = (status: number) => {
    switch (status) {
      case 0: return 'Pending';
      case 1: return 'In Progress';
      case 2: return 'Completed';
      case 3: return 'Cancelled';
      default: return 'Unknown';
    }
  };

  const getOrderTypeText = (type: number) => {
    switch (type) {
      case 0: return 'Dine In';
      case 1: return 'Dine Out';
      case 2: return 'Take Away';
      default: return 'Unknown';
    }
  };

  const formatCurrency = (amount: number) => {
    return new Intl.NumberFormat('en-IN', {
      style: 'currency',
      currency: 'INR'
    }).format(amount);
  };

  const formatDate = (date: string) => {
    return new Date(date).toLocaleString('en-IN', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  };

  if (loading) {
    return (
      <div className="order-management-container">
        <div className="order-loading">Loading orders...</div>
      </div>
    );
  }

  return (
    <div className="order-management-container">
      <div className="order-header">
        <h1>Order Management</h1>
        <div className="order-actions">
          <button 
            className="refresh-btn"
            onClick={fetchOrders}
          >
            🔄 Refresh
          </button>
          <button 
            className="today-btn"
            onClick={fetchTodayOrders}
          >
            📅 Today's Orders
          </button>
          <button 
            className="history-btn"
            onClick={fetchOrderHistory}
          >
            📋 Order History
          </button>
        </div>
      </div>

      <div className="order-filters">
        <div className="filter-group">
          <label>Status:</label>
          <select 
            value={statusFilter} 
            onChange={(e) => setStatusFilter(e.target.value)}
          >
            <option value="all">All Status</option>
            <option value="pending">Pending</option>
            <option value="inprogress">In Progress</option>
            <option value="completed">Completed</option>
            <option value="cancelled">Cancelled</option>
          </select>
        </div>
        
        <div className="filter-group">
          <label>Date:</label>
          <select 
            value={dateFilter} 
            onChange={(e) => setDateFilter(e.target.value)}
          >
            <option value="all">All Time</option>
            <option value="today">Today</option>
            <option value="range">Date Range</option>
          </select>
        </div>
        
        {dateFilter === 'range' && (
          <>
            <div className="filter-group">
              <label>Start Date:</label>
              <input
                type="date"
                value={startDate}
                onChange={(e) => setStartDate(e.target.value)}
              />
            </div>
            
            <div className="filter-group">
              <label>End Date:</label>
              <input
                type="date"
                value={endDate}
                onChange={(e) => setEndDate(e.target.value)}
              />
            </div>
            
            <div className="filter-group">
              <button 
                className="date-filter-btn"
                onClick={fetchOrdersByDateRange}
                disabled={!startDate || !endDate}
              >
                Apply Date Filter
              </button>
            </div>
          </>
        )}
      </div>

      <div className="orders-section">
        <h2>Orders (Count: {filteredOrders.length})</h2>
        
        {filteredOrders.length === 0 ? (
          <div className="no-orders">No orders found with current filters.</div>
        ) : (
          <div className="orders-grid">
            {filteredOrders.map((order) => (
              <div key={order.orderId} className="order-card">
                <div className="order-card-header">
                  <div className="order-id">Order #{order.orderId}</div>
                  <div className={`order-status ${getStatusColor(order.status)}`}>
                    {getStatusText(order.status)}
                  </div>
                </div>
                
                <div className="order-details">
                  <div className="order-info">
                    <span className="label">Type:</span>
                    <span className="value">{getOrderTypeText(order.orderType)}</span>
                  </div>
                  
                  <div className="order-info">
                    <span className="label">Date:</span>
                    <span className="value">{formatDate(order.orderDate)}</span>
                  </div>
                  
                  <div className="order-info">
                    <span className="label">Items:</span>
                    <span className="value">{order.qtyOrdered} items</span>
                  </div>
                  
                  <div className="order-info">
                    <span className="label">Total:</span>
                    <span className="value amount">{formatCurrency(order.totalAmount)}</span>
                  </div>
                  
                  <div className="order-info">
                    <span className="label">Customer ID:</span>
                    <span className="value">{order.userId}</span>
                  </div>
                </div>

                {order.items && order.items.length > 0 && (
                  <div className="order-items">
                    <h4>Items:</h4>
                    <ul>
                      {order.items.map((item, index) => (
                        <li key={index}>
                          Item #{item.itemId} - Qty: {item.quantity}
                        </li>
                      ))}
                    </ul>
                  </div>
                )}
              </div>
            ))}
          </div>
        )}
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

export default OrderManagement;