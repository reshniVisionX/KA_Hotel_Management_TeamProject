import { useEffect, useState } from 'react';
import { useAppSelector } from '../../redux/hooks';
import { Navbar } from '../Customer/Navbar';
import '../../Styles/UserDashboard/UserDashboard.css';
import '../../Styles/UserDashboard/Reviews.css';
import type { Review } from '../../Types/UserDashboard/Reviews';
import type { AppError } from '../../Types/Dashboard/ApiError';
import { getUserReviews } from '../../Service/dashboard.api';
import reviewsImage from '../../assets/Reviews.jpg';

const ManageReviews = () => {
  const [reviews, setReviews] = useState<Review[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string>('');

  const user = useAppSelector((state) => state.auth.user);

  useEffect(() => {
    fetchReviews();
  }, [user?.userId]);

  const fetchReviews = async () => {
    try {
      setLoading(true);
      setError('');
      if (user?.userId) {
        const response = await getUserReviews(user.userId);
        const userReviews = response.data;
        if (Array.isArray(userReviews)) {
          setReviews(userReviews);
        } else {
          setReviews([]);
        }
      } else {
        setReviews([]);
      }
    } catch (err) {
      const error = err as AppError;
      setError(error.message || 'Failed to fetch reviews');
      setReviews([]);
    } finally {
      setLoading(false);
    }
  };

  const renderStars = (rating: number) => {
    // Convert 10-point scale to 5-point scale
    const convertedRating = Math.max(0, Math.min(5, Math.floor(rating / 2)));
    return '★'.repeat(convertedRating) + '☆'.repeat(5 - convertedRating);
  };

  if (loading) {
    return (
      <>
        <Navbar />
        <div className="reviews-container" style={{ paddingTop: '64px' }}>
          <div className="reviews-loading">Loading reviews...</div>
        </div>
      </>
    );
  }

  if (error) {
    return (
      <>
        <Navbar />
        <div className="reviews-container" style={{ paddingTop: '64px' }}>
          <div className="reviews-error">{error}</div>
        </div>
      </>
    );
  }

  return (
    <>
      <Navbar />
      <div className="reviews-container" style={{ paddingTop: '64px' }}>
        <div className="reviews-header">
          <h1 className="reviews-title">My Reviews</h1>
        </div>
        <div className="user-dashboard-content">
          {reviews.length === 0 ? (
            <div className="reviews-empty">
              <p>No reviews found. Leave your first review!</p>
            </div>
          ) : (
            <div className="reviews-grid">
              {reviews.map((review: any) => (
                <div key={review.reviewId || review.id} className="reviews-card">
                  <img 
                    src={reviewsImage} 
                    alt="Review" 
                    className="reviews-image"
                  />
                  <div className="reviews-details">
                    <div className="reviews-info-row">
                      <span className="reviews-label">Review ID:</span>
                      <span className="reviews-value">{review.reviewId || review.id}</span>
                    </div>
                    <div className="reviews-info-row">
                      <span className="reviews-label">Restaurant:</span>
                      <span className="reviews-value">{review.restaurantName}</span>
                    </div>
                    <div className="reviews-info-row">
                      <span className="reviews-label">Rating:</span>
                      <span className="reviews-value reviews-rating">
                        {renderStars(review.rating)}
                      </span>
                    </div>
                    <div className="reviews-info-row">
                      <span className="reviews-label">Comment:</span>
                      <span className="reviews-value">{review.comments}</span>
                    </div>
                    <div className="reviews-info-row">
                      <span className="reviews-label">Date:</span>
                      <span className="reviews-value">{new Date(review.reviewDate || review.createdAt).toLocaleDateString()}</span>
                    </div>
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

export default ManageReviews;