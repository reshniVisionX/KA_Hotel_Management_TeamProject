import { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import {
  Box,
  Typography,
  Card,
  CardContent,
  Grid,
  Rating,
  LinearProgress,
  Tabs,
  Tab,
  TextField,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Button,
  Chip,
  Avatar,
  Divider,
  CircularProgress,
  Paper
} from '@mui/material';
import {
  Star as StarIcon,
  StarBorder as StarBorderIcon,
  Refresh as RefreshIcon,
  Person as PersonIcon,
  RateReview as ReviewIcon
} from '@mui/icons-material';
import type { RootState } from '../../redux/store';
import { getManagerRestaurant, getRestaurantReviews, getRestaurantRating, getLatestReviews, getTopRatedReviews, getMenuItemsByRestaurant } from '../../Service/manager.api';
import Toast, { type ToastType } from '../../Utils/Toast';
import type { AppError } from '../../Types/Dashboard/ApiError';

const Reviews = () => {
  const { user } = useSelector((state: RootState) => state.auth);
  const [loading, setLoading] = useState(true);
  const [reviews, setReviews] = useState<any[]>([]);
  const [ratingData, setRatingData] = useState<any>(null);
  const [filteredReviews, setFilteredReviews] = useState<any[]>([]);
  const [latestReviews, setLatestReviews] = useState<any[]>([]);
  const [topRatedReviews, setTopRatedReviews] = useState<any[]>([]);

  const [toast, setToast] = useState<{ message: string; type: ToastType } | null>(null);
  const [filterRating, setFilterRating] = useState<string>('all');
  const [sortBy, setSortBy] = useState<string>('latest');
  const [searchTerm, setSearchTerm] = useState<string>('');
  const [activeTab, setActiveTab] = useState<string>('all');

  useEffect(() => {
    if (user?.userId) {
      fetchReviewsData();
    }
  }, [user?.userId]);

  useEffect(() => {
    applyFilters();
  }, [reviews, filterRating, sortBy, searchTerm]);

  const fetchReviewsData = async () => {
    try {
      setLoading(true);
      
      const restaurantResponse = await getManagerRestaurant(user?.userId!);
      const restaurantId = restaurantResponse.data.restaurantId;
      
      const [reviewsResponse, ratingResponse, latestResponse, topRatedResponse, menuItemsResponse] = await Promise.all([
        getRestaurantReviews(restaurantId),
        getRestaurantRating(restaurantId),
        getLatestReviews(restaurantId),
        getTopRatedReviews(restaurantId),
        getMenuItemsByRestaurant(restaurantId)
      ]);

      setReviews(Array.isArray(reviewsResponse.data) ? reviewsResponse.data : []);
      setRatingData(ratingResponse.data);
      setLatestReviews(Array.isArray(latestResponse.data) ? latestResponse.data : []);
      setTopRatedReviews(Array.isArray(topRatedResponse.data) ? topRatedResponse.data : []);
      
      // Menu item reviews not supported by backend
    } catch (err) {
      const error = err as AppError;
      setToast({ message: error.message || 'Failed to fetch reviews data', type: 'error' });
    } finally {
      setLoading(false);
    }
  };

  const applyFilters = () => {
    let sourceReviews = [...reviews];
    
    // Switch data source based on active tab
    switch (activeTab) {
      case 'latest':
        // Get latest 10 reviews from all reviews
        sourceReviews = [...reviews]
          .sort((a, b) => new Date(b.reviewDate).getTime() - new Date(a.reviewDate).getTime())
          .slice(0, 10);
        break;
      case 'topRated':
        // Get top 10 highest rated reviews from all reviews
        sourceReviews = [...reviews]
          .sort((a, b) => b.rating - a.rating)
          .slice(0, 10);
        break;
      default:
        // All reviews
        sourceReviews = reviews;
    }
    
    let filtered = [...sourceReviews];

    // Filter by rating (convert 5-point filter to 10-point range)
    if (filterRating !== 'all') {
      const fivePointRating = parseInt(filterRating);
      const minTenPoint = (fivePointRating - 1) * 2;
      const maxTenPoint = fivePointRating * 2;
      filtered = filtered.filter(review => 
        review.rating >= minTenPoint && review.rating <= maxTenPoint
      );
    }

    // Search filter - searches in review comments and customer names
    if (searchTerm) {
      filtered = filtered.filter(review => 
        review.comments?.toLowerCase().includes(searchTerm.toLowerCase()) ||
        review.userName?.toLowerCase().includes(searchTerm.toLowerCase())
      );
    }

    // Sort reviews (only apply if not already sorted by tab)
    if (activeTab === 'all') {
      switch (sortBy) {
        case 'latest':
          filtered.sort((a, b) => new Date(b.reviewDate).getTime() - new Date(a.reviewDate).getTime());
          break;
        case 'oldest':
          filtered.sort((a, b) => new Date(a.reviewDate).getTime() - new Date(b.reviewDate).getTime());
          break;
        case 'highest':
          filtered.sort((a, b) => b.rating - a.rating);
          break;
        case 'lowest':
          filtered.sort((a, b) => a.rating - b.rating);
          break;
      }
    }

    setFilteredReviews(filtered);
  };

  const convertTo5Point = (rating: number) => {
    // Convert 10-point scale to 5-point scale
    return (rating / 10) * 5;
  };



  const formatDate = (date: string) => {
    return new Date(date).toLocaleDateString('en-IN', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  };

  if (loading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: 400 }}>
        <CircularProgress sx={{ color: 'var(--primary-color)' }} />
        <Typography sx={{ ml: 2, color: 'var(--text-color)' }}>Loading reviews data...</Typography>
      </Box>
    );
  }

  return (
    <Box sx={{ p: 3, maxWidth: 1200, mx: 'auto', width: '100%' }}>
      {/* Header */}
      <Box sx={{ display: 'flex', alignItems: 'center', gap: 2, mb: 3 }}>
        <ReviewIcon sx={{ color: 'var(--primary-color)', fontSize: 32 }} />
        <Box>
          <Typography variant="h4" sx={{ fontWeight: 600, color: 'var(--text-color)' }}>
            Customer Reviews
          </Typography>
          <Typography variant="body1" sx={{ color: '#666' }}>
            Monitor and analyze customer feedback for your restaurant
          </Typography>
        </Box>
      </Box>

      {/* Rating Overview */}
      <Card sx={{ mb: 3, bgcolor: 'var(--card-bg)', border: '1px solid var(--card-border)' }}>
        <CardContent>
          <Grid container spacing={4} alignItems="center">
            <Grid item xs={12} md={4}>
              <Box sx={{ textAlign: 'center' }}>
                <Typography variant="h2" sx={{ fontWeight: 700, color: 'var(--primary-color)', mb: 1 }}>
                  {convertTo5Point(ratingData?.averageRating || 0).toFixed(1)}
                </Typography>
                <Rating
                  value={convertTo5Point(ratingData?.averageRating || 0)}
                  precision={0.1}
                  readOnly
                  size="large"
                  sx={{ mb: 1 }}
                />
                <Typography variant="body1" sx={{ color: '#666' }}>
                  Based on {ratingData?.totalReviews || 0} reviews
                </Typography>
              </Box>
            </Grid>
            <Grid item xs={12} md={8}>
              <Box sx={{ display: 'flex', flexDirection: 'column', gap: 1.5 }}>
                {[5, 4, 3, 2, 1].map((star) => {
                  const countKey = `${['', 'one', 'two', 'three', 'four', 'five'][star]}StarCount`;
                  const count = ratingData?.[countKey] || 0;
                  const percentage = ((count / (ratingData?.totalReviews || 1)) * 100);
                  
                  return (
                    <Box key={star} sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
                      <Typography variant="body1" sx={{ minWidth: 40 }}>{star} ★</Typography>
                      <LinearProgress
                        variant="determinate"
                        value={percentage}
                        sx={{ flex: 1, height: 10, borderRadius: 5 }}
                      />
                      <Typography variant="body1" sx={{ minWidth: 40, textAlign: 'right' }}>
                        {count}
                      </Typography>
                    </Box>
                  );
                })}
              </Box>
            </Grid>
          </Grid>
        </CardContent>
      </Card>

      {/* Tabs - All Reviews, Latest 10, Top 10 */}
      <Paper sx={{ mb: 3, bgcolor: 'var(--card-bg)', border: '1px solid var(--card-border)' }}>
        <Tabs
          value={activeTab}
          onChange={(_, newValue) => setActiveTab(newValue)}
          sx={{ borderBottom: 1, borderColor: 'divider' }}
        >
          <Tab 
            label={`All Reviews (${reviews.length})`} 
            value="all" 
          />
          <Tab 
            label={`Latest 10 Reviews`} 
            value="latest" 
          />
          <Tab 
            label={`Top 10 Rated Reviews`} 
            value="topRated" 
          />
        </Tabs>
      </Paper>

      {/* Filters */}
      <Card sx={{ mb: 3, bgcolor: 'var(--card-bg)', border: '1px solid var(--card-border)' }}>
        <CardContent>
          <Grid container spacing={3} alignItems="center">
            <Grid item xs={12} sm={6} md={3}>
              <FormControl fullWidth>
                <InputLabel>Filter by Rating</InputLabel>
                <Select
                  value={filterRating}
                  label="Filter by Rating"
                  onChange={(e) => setFilterRating(e.target.value)}
                >
                  <MenuItem value="all">All Ratings</MenuItem>
                  <MenuItem value="5">5 Stars Only</MenuItem>
                  <MenuItem value="4">4 Stars Only</MenuItem>
                  <MenuItem value="3">3 Stars Only</MenuItem>
                  <MenuItem value="2">2 Stars Only</MenuItem>
                  <MenuItem value="1">1 Star Only</MenuItem>
                </Select>
              </FormControl>
            </Grid>
            <Grid item xs={12} sm={6} md={3}>
              <FormControl fullWidth>
                <InputLabel>Sort By</InputLabel>
                <Select
                  value={sortBy}
                  label="Sort By"
                  onChange={(e) => setSortBy(e.target.value)}
                >
                  <MenuItem value="latest">Newest First</MenuItem>
                  <MenuItem value="oldest">Oldest First</MenuItem>
                  <MenuItem value="highest">Highest Rating First</MenuItem>
                  <MenuItem value="lowest">Lowest Rating First</MenuItem>
                </Select>
              </FormControl>
            </Grid>
            <Grid item xs={12} sm={8} md={4}>
              <TextField
                fullWidth
                label="Search Reviews"
                placeholder="Search by customer name or review content..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
              />
            </Grid>
            <Grid item xs={12} sm={4} md={2}>
              <Button
                fullWidth
                variant="contained"
                startIcon={<RefreshIcon />}
                onClick={fetchReviewsData}
                sx={{ 
                  bgcolor: 'var(--primary-color)', 
                  '&:hover': { bgcolor: 'var(--primary-hover)' },
                  height: 56
                }}
              >
                Refresh
              </Button>
            </Grid>
          </Grid>
        </CardContent>
      </Card>

      {/* Reviews List */}
      <Typography variant="h5" sx={{ fontWeight: 600, color: 'var(--text-color)', mb: 2 }}>
        {activeTab === 'all' && `All Customer Reviews (${filteredReviews.length})`}
        {activeTab === 'latest' && `Latest 10 Reviews`}
        {activeTab === 'topRated' && `Top 10 Rated Reviews`}
      </Typography>

      {filteredReviews.length === 0 ? (
        <Card sx={{ bgcolor: 'var(--card-bg)', border: '1px solid var(--card-border)' }}>
          <CardContent sx={{ textAlign: 'center', py: 8 }}>
            <ReviewIcon sx={{ fontSize: 64, color: '#ccc', mb: 2 }} />
            <Typography variant="h6" sx={{ color: '#666', mb: 1 }}>
              No reviews found
            </Typography>
            <Typography variant="body2" sx={{ color: '#999' }}>
              {activeTab === 'all' && 'No reviews match your current filters'}
              {activeTab === 'latest' && 'No recent reviews available'}
              {activeTab === 'topRated' && 'No top-rated reviews available'}
            </Typography>
          </CardContent>
        </Card>
      ) : (
        <Grid container spacing={3}>
          {filteredReviews.map((review, index) => (
            <Grid item xs={12} key={review.reviewId || index}>
              <Card sx={{ 
                bgcolor: 'var(--card-bg)', 
                border: '1px solid var(--card-border)', 
                '&:hover': { 
                  boxShadow: 3,
                  transform: 'translateY(-2px)',
                  transition: 'all 0.2s ease'
                }
              }}>
                <CardContent>
                  <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', mb: 2 }}>
                    <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
                      <Avatar sx={{ bgcolor: 'var(--primary-color)', width: 48, height: 48 }}>
                        <PersonIcon />
                      </Avatar>
                      <Box>
                        <Typography variant="h6" sx={{ fontWeight: 600, color: 'var(--text-color)' }}>
                          {review.userName || 'Anonymous Customer'}
                        </Typography>
                        <Typography variant="body2" sx={{ color: '#666' }}>
                          {formatDate(review.reviewDate)}
                        </Typography>
                      </Box>
                    </Box>
                    <Box sx={{ textAlign: 'right' }}>
                      <Rating
                        value={convertTo5Point(review.rating)}
                        precision={0.1}
                        readOnly
                        size="medium"
                      />
                      <Typography variant="body2" sx={{ color: '#666', mt: 0.5 }}>
                        {convertTo5Point(review.rating).toFixed(1)} out of 5
                      </Typography>
                    </Box>
                  </Box>
                  
                  {review.comments && (
                    <>
                      <Divider sx={{ my: 2 }} />
                      <Typography variant="body1" sx={{ 
                        fontStyle: 'italic', 
                        color: 'var(--text-color)',
                        lineHeight: 1.6,
                        pl: 2,
                        borderLeft: '3px solid var(--primary-color)',
                        bgcolor: '#f9f9f9',
                        p: 2,
                        borderRadius: 1
                      }}>
                        "{review.comments}"
                      </Typography>
                    </>
                  )}
                  
                  <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mt: 2 }}>
                    <Chip
                      label={`Review ID: ${review.reviewId}`}
                      size="small"
                      variant="outlined"
                      sx={{ color: '#666' }}
                    />
                    <Typography variant="caption" sx={{ color: '#999' }}>
                      Helpful for improving service quality
                    </Typography>
                  </Box>
                </CardContent>
              </Card>
            </Grid>
          ))}
        </Grid>
      )}

      {toast && (
        <Toast
          message={toast.message}
          type={toast.type}
          onClose={() => setToast(null)}
        />
      )}
    </Box>
  );
};

export default Reviews;