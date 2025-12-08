import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAppSelector } from "../../redux/hooks";
import { getOrderSummary, processPayment } from "../../Service/restaurant.api";
import type { OrderSummary  } from "../../Types/Restaurant/CartItems";
import { PayMethod } from "../../Types/Restaurant/CartItems";
import Toast, { type ToastType } from "../../Utils/Toast";
import type { AppError } from "../../Types/Dashboard/ApiError";
import "../../Styles/Restaurant/Orders.css";

export const Order = () => {
  const user = useAppSelector((state) => state.auth.user);
  const navigate = useNavigate();
  const [orderData, setOrderData] = useState<OrderSummary | null>(null);
  const [loading, setLoading] = useState(true);
  const [toast, setToast] = useState<{ message: string; type: ToastType } | null>(null);
  const [showPaymentOverlay, setShowPaymentOverlay] = useState(false);
  const [showSuccessOverlay, setShowSuccessOverlay] = useState(false);
  const [paymentForm, setPaymentForm] = useState({
    amount: '',
    payMethod: PayMethod.Card,
    pin: ''
  });
  const [errors, setErrors] = useState({ amount: '', pin: '' });

  const showToast = (message: string, type: ToastType) => {
    setToast({ message, type });
  };

  const fetchOrderSummary = async () => {
    if (!user?.userId) return;

    try {
      setLoading(true);
      const response = await getOrderSummary(user.userId);
      setOrderData(response.data);
    } catch (err) {
      const error = err as AppError;
      showToast(error.message, "error");
    } finally {
      setLoading(false);
    }
  };

  const handlePayment = () => {
    if (!orderData) return;
    setPaymentForm({
      amount: orderData.grandTotal.toString(),
      payMethod: PayMethod.Card,
      pin: ''
    });
    setShowPaymentOverlay(true);
  };

  const validateForm = () => {
    const newErrors = { amount: '', pin: '' };
    
    if (!paymentForm.amount || parseFloat(paymentForm.amount) <= 0) {
      newErrors.amount = 'Please enter a valid amount';
    }
    
    if (!paymentForm.pin || paymentForm.pin.length !== 4) {
      newErrors.pin = 'PIN must be 4 digits';
    }
    
    setErrors(newErrors);
    return !newErrors.amount && !newErrors.pin;
  };

  const processPaymentHandler = async () => {
    if (!validateForm() || !orderData) return;

    try {
      const paymentData = {
        orderId: orderData.orderId,
        payMethod: paymentForm.payMethod
      };
      
      const response = await processPayment(paymentData);
      setShowPaymentOverlay(false);
      setShowSuccessOverlay(true);
      
      // Auto redirect after 3 seconds
      setTimeout(() => {
        setShowSuccessOverlay(false);
        navigate('/orderTrack');
      }, 3000);
    } catch (err) {
      const error = err as AppError;
      showToast(error.message, "error");
    }
  };

  useEffect(() => {
    fetchOrderSummary();
  }, [user?.userId]);

  if (loading) {
    return (
      <div className="order-loading">
        <div className="loading-spinner"></div>
        <p>Loading your order...</p>
      </div>
    );
  }

  if (!orderData) {
    return (
      <div className="order-container">
        <h2>No active order found</h2>
      </div>
    );
  }

  return (
    <div className="order-container">
      {toast && (
        <Toast
          message={toast.message}
          type={toast.type}
          onClose={() => setToast(null)}
        />
      )}

      <div className="order-header">
        <h2>Order Summary</h2>
        <div className="order-info">
          <p>Order ID: #{orderData.orderId}</p>
          <p>Restaurant: {orderData.restaurantName}</p>
          <p>Date: {new Date(orderData.orderDate).toLocaleDateString()}</p>
        </div>
      </div>

      <div className="order-items">
        {orderData.items.map((item) => (
          <div className="order-item" key={item.itemId}>
            <div className="item-details">
              <h4>{item.itemName}</h4>
              <p>Qty: {item.quantity}</p>
            </div>
            <div className="item-pricing">
              <p>₹{item.originalPrice} × {item.quantity}</p>
              <p className="discount">-₹{item.itemDiscount} (discount)</p>
              <p className="tax">+₹{item.itemTax} (tax)</p>
              <p className="total">₹{item.itemTotal}</p>
            </div>
          </div>
        ))}
      </div>

      <div className="order-summary">
        <div className="summary-row">
          <span>Subtotal:</span>
          <span>₹{orderData.subtotal}</span>
        </div>
        <div className="summary-row">
          <span>Discount:</span>
          <span>-₹{orderData.totalDiscount}</span>
        </div>
        <div className="summary-row">
          <span>Tax:</span>
          <span>₹{orderData.totalTax}</span>
        </div>
        <div className="summary-row total">
          <span>Grand Total:</span>
          <span>₹{orderData.grandTotal}</span>
        </div>
      </div>
      <button type="button" className="pay-btn" onClick={handlePayment}>
        Proceed to Payment
      </button>

      {showPaymentOverlay && (
        <div className="payment-overlay">
          <div className="payment-modal">
            <div className="payment-header">
              <h3>Payment Details</h3>
              <button 
                className="close-btn" 
                onClick={() => setShowPaymentOverlay(false)}
              >
                ×
              </button>
            </div>
            
            <div className="payment-form">
              <div className="form-group">
                <label>Amount</label>
                <input
                  type="number"
                  className={`form-input ${errors.amount ? 'error' : ''}`}
                  value={paymentForm.amount}
                  onChange={(e) => setPaymentForm({...paymentForm, amount: e.target.value})}
                  placeholder="Enter amount"
                />
                {errors.amount && <span className="error-text">{errors.amount}</span>}
              </div>
              
              <div className="form-group">
                <label>Payment Method</label>
                <select
                  className="form-input"
                  value={paymentForm.payMethod}
                  onChange={(e) => setPaymentForm({...paymentForm, payMethod: parseInt(e.target.value)})}
                >
                  <option value={PayMethod.Card}>Card</option>
                  <option value={PayMethod.UPI}>UPI</option>
                  <option value={PayMethod.Wallet}>Wallet</option>
                </select>
              </div>
              
              <div className="form-group">
                <label>PIN</label>
                <input
                  type="password"
                  className={`form-input ${errors.pin ? 'error' : ''}`}
                  value={paymentForm.pin}
                  onChange={(e) => setPaymentForm({...paymentForm, pin: e.target.value})}
                  placeholder="Enter 4-digit PIN"
                  maxLength={4}
                />
                {errors.pin && <span className="error-text">{errors.pin}</span>}
              </div>
              
              <div className="payment-actions">
                <button 
                  className="cancel-payment-btn" 
                  onClick={() => setShowPaymentOverlay(false)}
                >
                  Cancel
                </button>
                <button 
                  className="confirm-payment-btn" 
                  onClick={processPaymentHandler}
                >
                  Pay ₹{paymentForm.amount}
                </button>
              </div>
            </div>
          </div>
        </div>
      )}

      {showSuccessOverlay && (
        <div className="success-overlay">
          <div className="success-modal">
            <div className="success-icon">✅</div>
            <h2>Payment Successful!</h2>
            <p>Order Confirmed</p>
            <div className="success-details">
              <p>Order ID: #{orderData.orderId}</p>
              <p>Amount: ₹{orderData.grandTotal}</p>
            </div>
            <div className="success-message">
              <p>Thank you for your order!</p>
              <p>Redirecting to order tracking...</p>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};
