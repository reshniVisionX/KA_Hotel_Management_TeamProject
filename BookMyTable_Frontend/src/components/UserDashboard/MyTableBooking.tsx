import { useEffect, useState } from 'react';
import { useAppSelector } from '../../redux/hooks';
import { Navbar } from '../Customer/Navbar';
import '../../Styles/UserDashboard/UserDashboard.css';
import type { TableBooking } from '../../Types/UserDashboard/TableBooking';
import type { AppError } from '../../Types/Dashboard/ApiError';
import { getUserTableBookings } from '../../Service/dashboard.api';

const MyTableBooking = () => {
  const [bookings, setBookings] = useState<TableBooking[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string>('');
  const user = useAppSelector((state) => state.auth.user);

  useEffect(() => {
    fetchBookings();
  }, [user?.userId]);
  const fetchBookings = async () => {
    try {
      setLoading(true);
      setError('');
      if (user?.userId) {
        const response = await getUserTableBookings(user.userId);
        const userBookings = response.data;
        if (Array.isArray(userBookings)) {
          setBookings(userBookings);
        } else {
          setBookings([]);
        }
      } else {
        setBookings([]);
      }
    } catch (err) {
      const error = err as AppError;
      setError(error.message || 'Failed to fetch table bookings');
      setBookings([]);
    } finally {
      setLoading(false);
    }
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'pending': return 'var(--warning-color)';
      case 'confirmed': return 'var(--success-color)';
      case 'cancelled': return 'var(--error-color)';
      case 'completed': return 'var(--info-color)';
      default: return 'var(--text-secondary)';
    }
  };

  if (loading) {
    return (
      <>
        <Navbar />
        <div className="user-dashboard-container">
          <div className="user-dashboard-loading">Loading table bookings...</div>
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
      <div className="user-dashboard-container" style={{ paddingTop: '64px' }}>
      <div className="user-dashboard-header">
        <h1 className="user-dashboard-title">My Table Bookings</h1>
      </div>
      <div className="user-dashboard-content">
        {bookings.length === 0 ? (
          <div className="user-dashboard-empty">
            <p>No table bookings found. Book your first table!</p>
          </div>
        ) : (
          <div className="user-dashboard-grid">
            {bookings.map((booking) => (
              <div key={booking.id} className="user-dashboard-card">
                <h3>{booking.restaurantName}</h3>
                <p>Table: {booking.tableNumber}</p>
                <p>Date: {new Date(booking.bookingDate).toLocaleDateString()}</p>
                <p>Time: {booking.bookingTime}</p>
                <p>Guests: {booking.numberOfGuests}</p>
                <p style={{ color: getStatusColor(booking.status) }}>
                  Status: {booking.status.toUpperCase()}
                </p>
                {booking.specialRequests && (
                  <p>Special Requests: {booking.specialRequests}</p>
                )}
              </div>
            ))}
          </div>
        )}
        </div>
      </div>
    </>
  );
};

export default MyTableBooking;