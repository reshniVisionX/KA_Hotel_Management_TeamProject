import { useEffect, useState } from 'react';
import { Navbar } from '../Customer/Navbar';
import '../../Styles/UserDashboard/UserDashboard.css';
import type { Restaurant } from '../../Types/UserDashboard/Restaurant';
import type { AppError } from '../../Types/Dashboard/ApiError';

const ManageRestaurant = () => {
  const [restaurants, setRestaurants] = useState<Restaurant[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string>('');



  useEffect(() => {
    fetchRestaurants();
  }, []);

  const fetchRestaurants = async () => {
    try {
      setLoading(true);
      // API call will be implemented later
      setRestaurants([]);
    } catch (err) {
      const error = err as AppError;
      setError(error.message || 'Failed to fetch restaurants');
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return (
      <>
        <Navbar />
        <div className="user-dashboard-container">
          <div className="user-dashboard-loading">Loading restaurants...</div>
        </div>
      </>
    );
  }

  if (error) {
    return (
      <>
        <Navbar />
        <div className="user-dashboard-container">
          <div className="user-dashboard-error">{error}</div>
        </div>
      </>
    );
  }

  return (
    <>
      <Navbar />
      <div className="user-dashboard-container">
      <div className="user-dashboard-header">
        <h1 className="user-dashboard-title">Manage Restaurant</h1>
      </div>
      <div className="user-dashboard-content">
        {restaurants.length === 0 ? (
          <div className="user-dashboard-empty">
            <p>No restaurants found. Register your restaurant to get started!</p>
          </div>
        ) : (
          <div className="user-dashboard-grid">
            {restaurants.map((restaurant) => (
              <div key={restaurant.id} className="user-dashboard-card">
                <h3>{restaurant.name}</h3>
                <p>{restaurant.description}</p>
                <p>Cuisine: {restaurant.cuisine}</p>
                <p>Status: {restaurant.status}</p>
              </div>
            ))}
          </div>
        )}
        </div>
      </div>
    </>
  );
};

export default ManageRestaurant;