import { useEffect, useState } from "react";
import { useDispatch } from "react-redux";
import { useAppSelector } from "../../redux/hooks";
import { getOrderHistory, addReview } from "../../Service/restaurant.api";
import type { OrderHistory } from "../../Types/Restaurant/OrderHistory";
import type { MenuItem } from "../../Types/Restaurant/MenuItems";
import type { AddReviewRequest } from "../../Types/Restaurant/AddReview";
import Toast, { type ToastType } from "../../Utils/Toast";
import type { AppError } from "../../Types/Dashboard/ApiError";
import { fetchAllMenuItems } from "../../redux/thunks/menuThunk";
import type { AppDispatch } from "../../redux/store";
import "../../Styles/Restaurant/OrderTrack.css";

interface OrderItem {
  ItemId: number;
  Quantity: number;
}

export const OrderTrack: React.FC = () => {
  const dispatch = useDispatch<AppDispatch>();
  const user = useAppSelector((state) => state.auth.user);
  const menuItems = useAppSelector((state) => state.menu.menuItems);

  const [orders, setOrders] = useState<OrderHistory[]>([]);
  const [loading, setLoading] = useState(true);
  const [toast, setToast] = useState<{ message: string; type: ToastType } | null>(null);
  const [reviewModal, setReviewModal] = useState<{ orderId: number; restaurantId: number } | null>(null);
  const [reviewData, setReviewData] = useState({ rating: 5, comments: "" });

  const showToast = (message: string, type: ToastType) => {
    setToast({ message, type });
  };

  // 🔥 FETCH MENU ITEMS IF EMPTY
  useEffect(() => {
    if (menuItems.length === 0) {
      dispatch(fetchAllMenuItems());
    }
  }, [dispatch, menuItems.length]);

  // 🔥 FETCH ORDER HISTORY
  const fetchOrderHistory = async () => {
    if (!user?.userId) return;

    try {
      setLoading(true);
      const response = await getOrderHistory(user.userId);

      const sorted = response.data.sort(
        (a, b) => new Date(b.orderDate).getTime() - new Date(a.orderDate).getTime()
      );

      setOrders(sorted);
    } catch (err) {
      const error = err as AppError;
      showToast(error.message, "error");
    } finally {
      setLoading(false);
    }
  };

  // Handle Add Review
  const handleAddReview = (orderId: number) => {
    const order = orders.find(o => o.orderId === orderId);
    if (!order) return;

    // Get restaurant ID from the first item in the order
    const items = parseItems(order.itemsList);
    if (items.length === 0) {
      showToast("No items found in this order", "error");
      return;
    }

    const firstMenuItem = getMenuItem(items[0].ItemId);
    if (!firstMenuItem) {
      showToast("Restaurant information not available", "error");
      return;
    }

    setReviewModal({ orderId, restaurantId: firstMenuItem.restaurantId });
  };

  // Submit Review
  const submitReview = async () => {
    if (!user?.userId || !reviewModal) return;

    if (!reviewData.comments.trim()) {
      showToast("Please enter a comment", "error");
      return;
    }

    try {
      const reviewRequest: AddReviewRequest = {
        restaurantId: reviewModal.restaurantId,
        userId: user.userId,
        rating: reviewData.rating,
        comments: reviewData.comments.trim()
      };

      await addReview(reviewRequest);
      showToast("Review added successfully!", "success");
      setReviewModal(null);
      setReviewData({ rating: 5, comments: "" });
    } catch (err) {
      const error = err as AppError;
      showToast(error.message, "error");
    }
  };

  // Parse items list
  const parseItems = (itemsList: string): OrderItem[] => {
    try {
      return JSON.parse(itemsList);
    } catch {
      return [];
    }
  };

  // Get menu item from menuSlice
  const getMenuItem = (id: number): MenuItem | undefined =>
    menuItems.find((m) => m.itemId === id);

  // Calculate total per item
  const getItemTotal = (m: MenuItem, qty: number) => {
    const discounted = m.price - m.price * (m.discount / 100);
    const taxed = discounted + discounted * (m.tax / 100);
    return taxed * qty;
  };

  // Status mapping
  const getStatusText = (s: number) =>
    ["Pending", "In Progress", "Completed", "Cancelled"][s] ?? "Unknown";

  const getStatusColor = (s: number) =>
    ["warning", "info", "success", "error"][s] ?? "info";

  const getOrderTypeText = (t: number) =>
    ["Dine In", "Dine Out", "Take Away"][t] ?? "Unknown";

  useEffect(() => {
    fetchOrderHistory();
  }, [user?.userId]);

  if (loading) {
    return (
      <div className="order-track-loading">
        <div className="loading-spinner"></div>
        <p>Loading your orders...</p>
      </div>
    );
  }

  return (
    <div className="order-track-container">
      {/* Toast */}
      {toast && (
        <Toast
          message={toast.message}
          type={toast.type}
          onClose={() => setToast(null)}
        />
      )}

      {/* Review Modal */}
      {reviewModal && (
        <div className="review-modal-overlay">
          <div className="review-modal">
            <div className="review-modal-header">
              <h3>Add Review</h3>
              <button 
                className="close-btn" 
                onClick={() => setReviewModal(null)}
              >
                ×
              </button>
            </div>
            
            <div className="review-modal-content">
              <div className="rating-section">
                <label>Rating:</label>
                <div className="star-rating">
                  {[1, 2, 3, 4, 5].map((star) => (
                    <span
                      key={star}
                      className={`star ${star <= reviewData.rating ? 'filled' : ''}`}
                      onClick={() => setReviewData(prev => ({ ...prev, rating: star }))}
                    >
                      ★
                    </span>
                  ))}
                </div>
              </div>
              
              <div className="comment-section">
                <label>Comment:</label>
                <textarea
                  value={reviewData.comments}
                  onChange={(e) => setReviewData(prev => ({ ...prev, comments: e.target.value }))}
                  placeholder="Share your experience..."
                  rows={4}
                />
              </div>
              
              <div className="review-modal-actions">
                <button className="submit-review-btn" onClick={submitReview}>
                  Submit Review
                </button>
                <button 
                  className="cancel-btn" 
                  onClick={() => setReviewModal(null)}
                >
                  Cancel
                </button>
              </div>
            </div>
          </div>
        </div>
      )}

      <div className="order-track-header">
        <h2>Order History</h2>
        <p>Track all your past and ongoing orders</p>
      </div>

      {/* No Orders */}
      {orders.length === 0 ? (
        <div className="no-orders">
          <h3>No Orders Found</h3>
          <p>You haven't placed any orders yet.</p>
        </div>
      ) : (
        <div className="orders-list">
          {orders.map((order) => {
            const items = parseItems(order.itemsList);

            return (
              <div key={order.orderId} className="order-card">
                {/* HEADER */}
                <div className="order-header">
                  <div className="order-info">
                    <h4>Order #{order.orderId}</h4>
                    <span className={`status-badge ${getStatusColor(order.status)}`}>
                      {getStatusText(order.status)}
                    </span>
                  </div>

                  <div className="order-date">
                    {new Date(order.orderDate).toLocaleDateString()}
                  </div>
                </div>

                {/* DETAILS */}
                <div className="order-details">
                  <div className="detail-row">
                    <span className="label">Customer:</span>
                    <span className="value">{order.userName}</span>
                  </div>

                  <div className="detail-row">
                    <span className="label">Order Type:</span>
                    <span className="value">{getOrderTypeText(order.orderType)}</span>
                  </div>
                </div>

                {/* ITEMS */}
                <div className="order-items">
                  <h5>Items Ordered</h5>

                  {items.length === 0 ? (
                    <p className="no-items">No item details available</p>
                  ) : (
                    <div className="items-list">
                      {items.map((item, index) => {
                        const menuItem = getMenuItem(item.ItemId);

                        if (!menuItem) {
                          return (
                            <div key={index} className="item-row">
                              <span className="item-name">Unknown Item #{item.ItemId}</span>
                              <span className="item-quantity">Qty: {item.Quantity}</span>
                              <span className="item-price">N/A</span>
                            </div>
                          );
                        }

                        const total = getItemTotal(menuItem, item.Quantity);

                        return (
                          <div key={index} className="item-row">
                            <div className="item-info">
                              <span className="item-name">{menuItem.itemName}</span>
                              <span className="item-type">
                                {menuItem.isVegetarian ? "🟢 Veg" : "🔴 Non-Veg"}
                              </span>
                              {menuItem.discount > 0 && (
                                <span className="discount-badge">{menuItem.discount}% OFF</span>
                              )}
                            </div>

                            <div className="item-quantity">Qty: {item.Quantity}</div>

                            <div className="item-price">₹{total.toFixed(2)}</div>
                          </div>
                        );
                      })}
                    </div>
                  )}
                </div>

                {/* TOTAL */}
                <div className="order-total">
                  <div className="total-row">
                    <span className="total-label">Total Amount:</span>
                    <span className="total-amount">₹{order.totalAmount}</span>
                  </div>
                </div>

                {/* FOOTER */}
                <div className="order-footer">
                  <span>Ordered on: {new Date(order.orderDate).toLocaleString()}</span>
                  {order.status === 2 && ( // Only show for completed orders
                    <button 
                      type="button" 
                      className="add-review-btn"
                      onClick={() => handleAddReview(order.orderId)}
                    >
                      Add Review
                    </button>
                  )}
                </div>
              </div>
            );
          })}
        </div>
      )}
    </div>
  );
};
