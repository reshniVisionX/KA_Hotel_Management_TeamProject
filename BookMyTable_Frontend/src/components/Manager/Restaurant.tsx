import { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import {
  Box,
  Card,
  CardContent,
  Typography,
  Button,
  TextField,
  Chip,
  CircularProgress,
  Divider
} from '@mui/material';
import {
  Restaurant as RestaurantIcon,
  Edit as EditIcon,
  Save as SaveIcon,
  Cancel as CancelIcon,
  CheckCircle as ActiveIcon,
  Cancel as InactiveIcon
} from '@mui/icons-material';
import type { RootState } from '../../redux/store';
import { getManagerRestaurant, updateRestaurant, updateRestaurantStatus } from '../../Service/manager.api';
import Toast, { type ToastType } from '../../Utils/Toast';
import type { AppError } from '../../Types/Dashboard/ApiError';
import type { Restaurant, UpdateRestaurant } from '../../Types/Manager/Restaurant';
import '../../Styles/Manager/Restaurant.css';

const RestaurantManagement = () => {
  const { user } = useSelector((state: RootState) => state.auth);
  const [loading, setLoading] = useState(true);
  const [restaurant, setRestaurant] = useState<Restaurant | null>(null);
  const [isEditing, setIsEditing] = useState(false);
  const [formData, setFormData] = useState<UpdateRestaurant>({
    restaurantName: '',
    description: '',
    location: '',
    contactNo: '',
    deliveryCharge: 0
  });
  const [toast, setToast] = useState<{ message: string; type: ToastType } | null>(null);

  useEffect(() => {
    if (user?.userId) {
      fetchRestaurantData();
    }
  }, [user?.userId]);

  const fetchRestaurantData = async () => {
    try {
      setLoading(true);
      const response = await getManagerRestaurant(user?.userId!);
      const restaurantData = response.data;
      setRestaurant(restaurantData);
      setFormData({
        restaurantName: restaurantData.restaurantName || '',
        description: restaurantData.description || '',
        location: restaurantData.location || '',
        contactNo: restaurantData.contactNo || '',
        deliveryCharge: restaurantData.deliveryCharge || 0
      });
    } catch (err) {
      const error = err as AppError;
      setToast({ message: error.message || 'Failed to fetch restaurant data', type: 'error' });
    } finally {
      setLoading(false);
    }
  };

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  const handleSave = async () => {
    try {
      setLoading(true);
      await updateRestaurant(restaurant?.restaurantId!, formData);
      setToast({ message: 'Restaurant updated successfully!', type: 'success' });
      setIsEditing(false);
      await fetchRestaurantData();
    } catch (err) {
      const error = err as AppError;
      setToast({ message: error.message || 'Failed to update restaurant', type: 'error' });
    } finally {
      setLoading(false);
    }
  };

  const handleStatusToggle = async () => {
    try {
      setLoading(true);
      const newStatus = !restaurant?.isActive;
      await updateRestaurantStatus(restaurant?.restaurantId!, newStatus);
      setToast({ 
        message: `Restaurant ${newStatus ? 'activated' : 'deactivated'} successfully!`, 
        type: 'success' 
      });
      await fetchRestaurantData();
    } catch (err) {
      const error = err as AppError;
      setToast({ message: error.message || 'Failed to update restaurant status', type: 'error' });
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: 400 }}>
        <CircularProgress sx={{ color: 'var(--primary-color)' }} />
        <Typography sx={{ ml: 2, color: 'var(--text-color)' }}>Loading restaurant data...</Typography>
      </Box>
    );
  }

  if (!restaurant) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: 400 }}>
        <Typography variant="h6" sx={{ color: 'var(--text-color)' }}>
          No restaurant found for this manager.
        </Typography>
      </Box>
    );
  }

  return (
    <Box sx={{ p: 3, width: '100%', maxWidth: '100%', boxSizing: 'border-box' }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
          <RestaurantIcon sx={{ color: 'var(--primary-color)', fontSize: 32 }} />
          <Typography variant="h4" sx={{ fontWeight: 600, color: 'var(--text-color)' }}>
            My Restaurant
          </Typography>
        </Box>
        
        <Box sx={{ display: 'flex', gap: 1 }}>
          <Chip
            icon={restaurant.isActive ? <ActiveIcon /> : <InactiveIcon />}
            label={restaurant.isActive ? 'Active' : 'Inactive'}
            color={restaurant.isActive ? 'success' : 'error'}
            onClick={handleStatusToggle}
            disabled={loading}
            sx={{ cursor: 'pointer' }}
          />
          {!isEditing ? (
            <Button
              variant="contained"
              startIcon={<EditIcon />}
              onClick={() => setIsEditing(true)}
              sx={{ bgcolor: 'var(--primary-color)', '&:hover': { bgcolor: 'var(--primary-hover)' } }}
            >
              Edit Details
            </Button>
          ) : (
            <Box sx={{ display: 'flex', gap: 1 }}>
              <Button
                variant="contained"
                startIcon={<SaveIcon />}
                onClick={handleSave}
                disabled={loading}
                color="success"
              >
                Save
              </Button>
              <Button
                variant="outlined"
                startIcon={<CancelIcon />}
                onClick={() => setIsEditing(false)}
                color="error"
              >
                Cancel
              </Button>
            </Box>
          )}
        </Box>
      </Box>

      <Card sx={{ 
        background: 'var(--card-bg)', 
        border: '1px solid var(--card-border)',
        maxWidth: 800,
        mx: 'auto'
      }}>
        <CardContent sx={{ p: 4 }}>
          <Box sx={{ 
            display: 'flex', 
            justifyContent: 'center', 
            alignItems: 'center', 
            height: 120, 
            bgcolor: 'rgba(255, 122, 0, 0.1)', 
            borderRadius: 2, 
            mb: 3 
          }}>
            <RestaurantIcon sx={{ fontSize: 48, color: 'var(--primary-color)', mr: 2 }} />
            <Typography variant="h5" sx={{ color: 'var(--text-color)', fontWeight: 600 }}>
              {restaurant.restaurantName}
            </Typography>
          </Box>

          <Box sx={{ display: 'grid', gap: 3 }}>
            <Box>
              <Typography variant="body2" sx={{ color: '#666', mb: 1, fontWeight: 500 }}>
                Restaurant Name
              </Typography>
              {isEditing ? (
                <TextField
                  fullWidth
                  name="restaurantName"
                  value={formData.restaurantName}
                  onChange={handleInputChange}
                  placeholder="Enter restaurant name"
                  size="small"
                />
              ) : (
                <Typography variant="body1" sx={{ 
                  p: 1.5, 
                  bgcolor: 'rgba(255, 122, 0, 0.05)', 
                  borderRadius: 1, 
                  color: 'var(--text-color)' 
                }}>
                  {restaurant.restaurantName}
                </Typography>
              )}
            </Box>

            <Box>
              <Typography variant="body2" sx={{ color: '#666', mb: 1, fontWeight: 500 }}>
                Description
              </Typography>
              {isEditing ? (
                <TextField
                  fullWidth
                  multiline
                  rows={3}
                  name="description"
                  value={formData.description}
                  onChange={handleInputChange}
                  placeholder="Enter restaurant description"
                  size="small"
                />
              ) : (
                <Typography variant="body1" sx={{ 
                  p: 1.5, 
                  bgcolor: 'rgba(255, 122, 0, 0.05)', 
                  borderRadius: 1, 
                  color: 'var(--text-color)' 
                }}>
                  {restaurant.description || 'No description available'}
                </Typography>
              )}
            </Box>

            <Box>
              <Typography variant="body2" sx={{ color: '#666', mb: 1, fontWeight: 500 }}>
                Location
              </Typography>
              {isEditing ? (
                <TextField
                  fullWidth
                  multiline
                  rows={2}
                  name="location"
                  value={formData.location}
                  onChange={handleInputChange}
                  placeholder="Enter restaurant location"
                  size="small"
                />
              ) : (
                <Typography variant="body1" sx={{ 
                  p: 1.5, 
                  bgcolor: 'rgba(255, 122, 0, 0.05)', 
                  borderRadius: 1, 
                  color: 'var(--text-color)' 
                }}>
                  {restaurant.location}
                </Typography>
              )}
            </Box>

            <Box sx={{ display: 'grid', gridTemplateColumns: { xs: '1fr', sm: '1fr 1fr' }, gap: 2 }}>
              <Box>
                <Typography variant="body2" sx={{ color: '#666', mb: 1, fontWeight: 500 }}>
                  Contact Number
                </Typography>
                {isEditing ? (
                  <TextField
                    fullWidth
                    type="tel"
                    name="contactNo"
                    value={formData.contactNo}
                    onChange={handleInputChange}
                    placeholder="Enter contact number"
                    size="small"
                  />
                ) : (
                  <Typography variant="body1" sx={{ 
                    p: 1.5, 
                    bgcolor: 'rgba(255, 122, 0, 0.05)', 
                    borderRadius: 1, 
                    color: 'var(--text-color)' 
                  }}>
                    {restaurant.contactNo}
                  </Typography>
                )}
              </Box>

              <Box>
                <Typography variant="body2" sx={{ color: '#666', mb: 1, fontWeight: 500 }}>
                  City
                </Typography>
                <Typography variant="body1" sx={{ 
                  p: 1.5, 
                  bgcolor: 'rgba(255, 122, 0, 0.05)', 
                  borderRadius: 1, 
                  color: 'var(--text-color)' 
                }}>
                  {restaurant.city}
                </Typography>
              </Box>
            </Box>

            <Box sx={{ display: 'grid', gridTemplateColumns: { xs: '1fr', sm: '1fr 1fr' }, gap: 2 }}>
              <Box>
                <Typography variant="body2" sx={{ color: '#666', mb: 1, fontWeight: 500 }}>
                  Restaurant Category
                </Typography>
                <Chip
                  label={
                    restaurant.restaurantCategory === 0 ? 'Fast Food' :
                    restaurant.restaurantCategory === 1 ? 'Casual' :
                    restaurant.restaurantCategory === 2 ? 'Fine Dining' :
                    restaurant.restaurantCategory === 3 ? 'Cafe' :
                    restaurant.restaurantCategory === 4 ? 'Buffet' : 'Unknown'
                  }
                  sx={{ bgcolor: 'var(--secondary-color)', color: 'white' }}
                />
              </Box>

              <Box>
                <Typography variant="body2" sx={{ color: '#666', mb: 1, fontWeight: 500 }}>
                  Food Type
                </Typography>
                <Chip
                  label={
                    restaurant.restaurantType === 0 ? 'Vegetarian' :
                    restaurant.restaurantType === 1 ? 'Non-Vegetarian' :
                    restaurant.restaurantType === 2 ? 'Both' : 'Unknown'
                  }
                  sx={{ bgcolor: 'var(--primary-color)', color: 'white' }}
                />
              </Box>
            </Box>

            <Box>
              <Typography variant="body2" sx={{ color: '#666', mb: 1, fontWeight: 500 }}>
                Delivery Charge
              </Typography>
              {isEditing ? (
                <TextField
                  fullWidth
                  type="number"
                  name="deliveryCharge"
                  value={formData.deliveryCharge}
                  onChange={handleInputChange}
                  placeholder="Enter delivery charge"
                  size="small"
                  inputProps={{ min: 0, step: 0.01 }}
                />
              ) : (
                <Typography variant="h6" sx={{ 
                  p: 1.5, 
                  bgcolor: 'rgba(255, 122, 0, 0.1)', 
                  borderRadius: 1, 
                  color: 'var(--primary-color)',
                  fontWeight: 600
                }}>
                  ₹{restaurant.deliveryCharge || 0}
                </Typography>
              )}
            </Box>

            <Divider sx={{ my: 2 }} />
            
            <Box sx={{ display: 'grid', gridTemplateColumns: { xs: '1fr', sm: 'repeat(3, 1fr)' }, gap: 2 }}>
              <Box sx={{ textAlign: 'center', p: 2, bgcolor: 'rgba(33, 150, 243, 0.1)', borderRadius: 2 }}>
                <Typography variant="body2" sx={{ color: '#666', mb: 0.5 }}>Restaurant ID</Typography>
                <Typography variant="h6" sx={{ color: '#2196f3', fontWeight: 600 }}>#{restaurant.restaurantId}</Typography>
              </Box>
              
              <Box sx={{ textAlign: 'center', p: 2, bgcolor: 'rgba(76, 175, 80, 0.1)', borderRadius: 2 }}>
                <Typography variant="body2" sx={{ color: '#666', mb: 0.5 }}>Created At</Typography>
                <Typography variant="h6" sx={{ color: '#4caf50', fontWeight: 600 }}>
                  {new Date(restaurant.createdAt).toLocaleDateString()}
                </Typography>
              </Box>
              
              <Box sx={{ textAlign: 'center', p: 2, bgcolor: restaurant.isActive ? 'rgba(76, 175, 80, 0.1)' : 'rgba(244, 67, 54, 0.1)', borderRadius: 2 }}>
                <Typography variant="body2" sx={{ color: '#666', mb: 0.5 }}>Status</Typography>
                <Typography variant="h6" sx={{ color: restaurant.isActive ? '#4caf50' : '#f44336', fontWeight: 600 }}>
                  {restaurant.isActive ? 'Active' : 'Inactive'}
                </Typography>
              </Box>
            </Box>
          </Box>
        </CardContent>
      </Card>

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

export default RestaurantManagement;