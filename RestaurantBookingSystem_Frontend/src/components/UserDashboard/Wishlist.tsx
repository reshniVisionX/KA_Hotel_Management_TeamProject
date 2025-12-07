import { useEffect, useState } from 'react';
import { useAppSelector } from '../../redux/hooks';
import { Navbar } from '../Customer/Navbar';
import '../../Styles/UserDashboard/UserDashboard.css';
import '../../Styles/UserDashboard/Wishlist.css';
import type { AppError } from '../../Types/Dashboard/ApiError';
import type { WishlistItem } from '../../Types/UserDashboard/Wishlist';
import { getUserWishlist, removeFromWishlist } from '../../Service/dashboard.api';
import wishlistImage from '../../assets/Wishlist.jpeg.jpg';

const Wishlist = () => {
  const [wishlistItems, setWishlistItems] = useState<WishlistItem[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string>('');
  const user = useAppSelector((state) => state.auth.user);

  useEffect(() => {
    fetchWishlist();
  }, [user?.userId]);

  const fetchWishlist = async () => {
    try {
      setLoading(true);
      setError('');
      if (user?.userId) {
        const response = await getUserWishlist(user.userId);
        const userWishlist = response.data;
        if (Array.isArray(userWishlist)) {
          setWishlistItems(userWishlist);
        } else {
          setWishlistItems([]);
        }
      } else {
        setWishlistItems([]);
      }
    } catch (err) {
      const error = err as AppError;
      setError(error.message || 'Failed to fetch wishlist');
      setWishlistItems([]);
    } finally {
      setLoading(false);
    }
  };

  const handleRemoveFromWishlist = async (wishlistId: number) => {
    try {
      await removeFromWishlist(wishlistId);
      fetchWishlist();
    } catch (err) {
      const error = err as AppError;
      console.error('Failed to remove from wishlist:', error.message);
    }
  };

  if (loading) {
    return (
      <>
        <Navbar />
        <div className="wishlist-container">
          <div className="wishlist-loading">Loading wishlist...</div>
        </div>
      </>
    );
  }

  if (error) {
    return (
      <>
        <Navbar />
        <div className="wishlist-container">
          <div className="wishlist-error">{error}</div>
        </div>
      </>
    );
  }

  return (
    <>
      <Navbar />
      <div className="wishlist-container" style={{ paddingTop: '64px' }}>
        <div className="wishlist-header">
          <h1 className="wishlist-title">My Wishlist</h1>
        </div>
      <div className="user-dashboard-content">
        {wishlistItems.length === 0 ? (
          <div className="wishlist-empty">
            <p>Your wishlist is empty. Start adding your favorite restaurants!</p>
          </div>
        ) : (
          <div className="wishlist-grid">
            {wishlistItems.map((item) => (
              <div key={item.wishlistId} className="wishlist-card">
                <img 
                  src={(() => {
                    const image = item.itemImage || item.restaurantImage;
                    if (image && !image.startsWith('data:') && !image.startsWith('http')) {
                      return `data:image/jpeg;base64,${image}`;
                    }
                    return image || wishlistImage;
                  })()} 
                  alt={item.itemName}
                  className="wishlist-image"
                  onError={(e) => {
                    e.currentTarget.src = wishlistImage;
                  }}
                />
                <div className="wishlist-details">
                  <div className="wishlist-info-row">
                    <span className="wishlist-label">Restaurant:</span>
                    <span className="wishlist-value">{item.restaurantName}</span>
                  </div>
                  <div className="wishlist-info-row">
                    <span className="wishlist-label">Location:</span>
                    <span className="wishlist-value">{item.location}</span>
                  </div>
                  <div className="wishlist-info-row">
                    <span className="wishlist-label">Item:</span>
                    <span className="wishlist-value">{item.itemName} {item.isVegetarian ? '🌱' : '🍖'}</span>
                  </div>
                  <div className="wishlist-info-row">
                    <span className="wishlist-label">Description:</span>
                    <span className="wishlist-value">{item.itemDescription}</span>
                  </div>
                  <div className="wishlist-info-row">
                    <span className="wishlist-label">Price:</span>
                    <span className="wishlist-value wishlist-item-price">₹{item.itemPrice}</span>
                  </div>
                  <div className="wishlist-info-row">
                    <span className="wishlist-label">Rating:</span>
                    <span className="wishlist-value">★ {item.rating}/5</span>
                  </div>
                  <div className="wishlist-info-row">
                    <span className="wishlist-label">Cuisine:</span>
                    <span className="wishlist-value">{item.cuisine}</span>
                  </div>
                </div>
                <div className="wishlist-actions">
                  <button 
                    onClick={() => handleRemoveFromWishlist(item.wishlistId)}
                    className="wishlist-remove-btn"
                  >
                    Remove from Wishlist
                  </button>
                </div>
              </div>
            ))}
          </div>
        )}
        </div>
      </div>
    </>

);
};

export default Wishlist;