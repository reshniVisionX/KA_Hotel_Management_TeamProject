import { useEffect, useState } from "react";
import { useAppSelector } from "../../redux/hooks";
import { 
  getCartItems, 
  clearCart, 
  incrementCartItem, 
  decrementCartItem, 
  removeFromCart 
} from "../../Service/restaurant.api";
import type { CartResponse } from "../../Types/Restaurant/CartItems";
import Toast, { type ToastType } from "../../Utils/Toast";
import type { AppError } from "../../Types/Dashboard/ApiError";
import "../../Styles/Restaurant/Cart.css";
import { useNavigate } from "react-router-dom";
import { placeOrder } from "../../Service/restaurant.api";

export const Cart = () => {
  const user = useAppSelector((state) => state.auth.user);
  const [cartData, setCartData] = useState<CartResponse | null>(null);
  const [loading, setLoading] = useState(true);
  const [toast, setToast] = useState<{ message: string; type: ToastType } | null>(null);

  const navigate = useNavigate();
  const showToast = (message: string, type: ToastType) => {
    setToast({ message, type });
  };

  const fetchCart = async () => {
    if (!user?.userId) return;

    try {
      setLoading(true);
      const response = await getCartItems(user.userId);
      setCartData(response.data);
    } catch (err) {
      const error = err as AppError;
      showToast(error.message, "error");
    } finally {
      setLoading(false);
    }
  };

  const handleClearCart = async () => {
    if (!user?.userId) return;

    try {
      const response = await clearCart(user.userId);
      showToast(response.message, "success");
      fetchCart();
    } catch (err) {
      const error = err as AppError;
      showToast(error.message, "error");
    }
  };

  const handleIncrement = async (cartId: number) => {
    try {
      const response = await incrementCartItem(cartId);
      showToast(response.message, "success");
      fetchCart();
    } catch (err) {
      const error = err as AppError;
      showToast(error.message, "error");
    }
  };

  const handleDecrement = async (cartId: number) => {
    try {
      const response = await decrementCartItem(cartId);
      showToast(response.message, "success");
      fetchCart();
    } catch (err) {
      const error = err as AppError;
      showToast(error.message, "error");
    }
  };

  const handleRemoveItem = async (cartId: number) => {
    try {
      const response = await removeFromCart(cartId);
      showToast(response.message, "success");
      fetchCart();
    } catch (err) {
      const error = err as AppError;
      showToast(error.message, "error");
    }
  };

const handlePlaceOrder = async () => {
  if (!user?.userId) return;

  try {
    const response = await placeOrder(user.userId);
    showToast(response.message, "success");
    navigate("/orders");
  } catch (err) {
    const error = err as AppError;
    showToast(error.message, "error");
  }
};
  useEffect(() => {
    fetchCart();
  }, [user?.userId]);

  if (loading) {
    return (
      <div className="cart-loading">
        <div className="loading-spinner"></div>
        <p>Loading your cart...</p>
      </div>
    );
  }

  if (!cartData || cartData.items.length === 0) {
    return (
      <div className="cart-container">
        <div className="cart-header">
          <h2 className="cart-title">Your Cart</h2>
        </div>
        <div className="empty-cart">
          <h3>Your cart is empty</h3>
          <p>Add some delicious items to get started!</p>
        </div>
      </div>
    );
  }

  const totalQuantity = cartData.items.reduce((sum, item) => sum + item.quantity, 0);
  const totalAmount = cartData.grandTotal ;

  return (
    <div className="cart-container">
      {toast && (
        <Toast
          message={toast.message}
          type={toast.type}
          onClose={() => setToast(null)}
        />
      )}

      <div className="cart-header">
        <h2 className="cart-title">Your Cart ({cartData.items.length} items)</h2>
        <button className="clear-cart-btn" onClick={handleClearCart}>
          Clear Cart
        </button>
      </div>

      <div className="cart-content">
        <div className="cart-items">
          {cartData.items.map((item) => (
            <div className="cart-item" key={item.cartId}>
              <div className="item-info">
                <div className="item-header">
                  <div className={`veg-indicator ${item.isVegetarian ? 'veg' : 'non-veg'}`}></div>
                  <h4 className="item-name">{item.itemName}</h4>
                </div>
                <p className="restaurant-name">{item.restaurantName}</p>
                <p className="item-price">₹{item.price} × {item.quantity} = ₹{item.totalPrice}</p>
              </div>

              <div className="item-controls">
                <div className="quantity-controls">
                  <button 
                    className="quantity-btn decrement" 
                    onClick={() => handleDecrement(item.cartId)}
                  >
                    -
                  </button>
                  <span className="quantity">{item.quantity}</span>
                  <button 
                    className="quantity-btn increment" 
                    onClick={() => handleIncrement(item.cartId)}
                  >
                    +
                  </button>
                </div>
                <button 
                  className="delete-btn" 
                  onClick={() => handleRemoveItem(item.cartId)}
                >
                  🗑️
                </button>
              </div>
            </div>
          ))}
        </div>

        <div className="cart-summary">
          <div className="summary-card">
            <h3 className="summary-title">Order Summary</h3>
            <div className="summary-divider"></div>
            
            <div className="summary-row">
              <span>Total Items:</span>
              <span>{cartData.items.length}</span>
            </div>
            <div className="summary-row">
              <span>Total Quantity:</span>
              <span>{totalQuantity}</span>
            </div>
            
            
            <div className="summary-divider"></div>
            <div className="summary-row total-row">
              <span>Total Amount:</span>
              <span className="total-amount">₹{totalAmount}</span>
            </div>

            <button className="checkout-btn" onClick={handlePlaceOrder}>
              Proceed to Checkout
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};