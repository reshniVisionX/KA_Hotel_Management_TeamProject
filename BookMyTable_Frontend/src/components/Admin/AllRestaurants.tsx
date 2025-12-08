import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { getAllRestaurants, toggleRestaurantStatus } from '../../Service/admin.api';
import type { AdminRestaurant } from '../../Types/Admin/Manager';
import type { AppError } from '../../Types/Dashboard/ApiError';
import Toast, { type ToastType } from '../../Utils/Toast';
import '../../Styles/Admin/AllRestaurants.css';

const AllRestaurants = () => {
  const [restaurants, setRestaurants] = useState<AdminRestaurant[]>([]);
  const [loading, setLoading] = useState(true);
  const [toast, setToast] = useState<{ message: string; type: ToastType } | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    fetchRestaurants();
  }, []);

  const fetchRestaurants = async () => {
    try {
      setLoading(true);
      const response = await getAllRestaurants();
      setRestaurants(response.data);
    } catch (err) {
      const error = err as AppError;
      setToast({ message: error.message, type: 'error' });
    } finally {
      setLoading(false);
    }
  };

  const handleToggleStatus = async (restaurantId: number, e: React.MouseEvent) => {
    e.stopPropagation();
    try {
      const response = await toggleRestaurantStatus(restaurantId);
      setToast({ message: response.message, type: 'success' });
      fetchRestaurants();
    } catch (err) {
      const error = err as AppError;
      setToast({ message: error.message, type: 'error' });
    }
  };

  const handleRestaurantClick = (restaurantId: number) => {
    navigate(`/admin/restaurant-analytics/${restaurantId}`);
  };

  if (loading) {
    return <div className="restaurants-loading">Loading restaurants...</div>;
  }

  return (
    <div className="all-restaurants-container">
      <h1 className="restaurants-title">All Restaurants</h1>
      <div className="restaurants-grid">
        {restaurants.map((restaurant) => (
          <div 
            key={restaurant.restaurantId} 
            className="restaurant-card"
            onClick={() => handleRestaurantClick(restaurant.restaurantId)}
          >
            {restaurant.images && (
              <img 
                src={restaurant.images.startsWith('data:') ? restaurant.images : `data:image/jpeg;base64,${restaurant.images}`}
                alt={restaurant.restaurantName}
                className="restaurant-image"
              />
            )}
            <div className="restaurant-info">
              <h3 className="restaurant-name">{restaurant.restaurantName}</h3>
              <p className="restaurant-description">{restaurant.description}</p>
              <div className="restaurant-details">
                <span className="restaurant-rating">★ {restaurant.ratings}</span>
                <span className="restaurant-city">{restaurant.city}</span>
              </div>
              <p className="restaurant-location">{restaurant.location}</p>
              <p className="restaurant-manager">Manager: {restaurant.manager.managerName}</p>
              <div className="restaurant-actions">
                <button
                  className={`status-toggle ${restaurant.isActive ? 'active' : 'inactive'}`}
                  onClick={(e) => handleToggleStatus(restaurant.restaurantId, e)}
                >
                  {restaurant.isActive ? 'Active' : 'Inactive'}
                </button>
              </div>
            </div>
          </div>
        ))}
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

export default AllRestaurants;
