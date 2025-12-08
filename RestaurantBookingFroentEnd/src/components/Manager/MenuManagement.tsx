import { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import {
  Box,
  Typography,
  Button,
  Dialog,
  DialogTitle,
  DialogContent,
  CircularProgress,
  IconButton
} from '@mui/material';
import {
  Restaurant as MenuIcon,
  Add as AddIcon,
  Close as CloseIcon
} from '@mui/icons-material';
import type { RootState } from '../../redux/store';
import { getManagerRestaurant, getMenuItemsByRestaurant, createMenuItem, updateMenuItem, deleteMenuItem } from '../../Service/manager.api';
import Toast, { type ToastType } from '../../Utils/Toast';
import type { MenuItem, CreateMenuItem, UpdateMenuItem } from '../../Types/Manager/Menu';
import type { AppError } from '../../Types/Dashboard/ApiError';
import '../../Styles/Manager/MenuManagement.css';

const MenuManagement = () => {
  const { user } = useSelector((state: RootState) => state.auth);
  const [loading, setLoading] = useState(true);
  const [menuItems, setMenuItems] = useState<MenuItem[]>([]);
  const [showAddForm, setShowAddForm] = useState(false);
  const [editingItem, setEditingItem] = useState<MenuItem | null>(null);
  const [toast, setToast] = useState<{ message: string; type: ToastType } | null>(null);
  const [restaurantId, setRestaurantId] = useState<number | null>(null);

  const [formData, setFormData] = useState<CreateMenuItem>({
    restaurantId: 0,
    itemName: '',
    description: '',
    availableQty: 0,
    discount: 0,
    price: 0,
    isVegetarian: false,
    tax: 0,
    image: undefined,
  });

  const [formErrors, setFormErrors] = useState<Record<string, string>>({});



  useEffect(() => {
    if (user?.userId) {
      fetchMenuItems();
    }
  }, [user?.userId]);

  const fetchMenuItems = async () => {
    try {
      setLoading(true);
      
      const restaurantResponse = await getManagerRestaurant(user?.userId!);
      const restId = restaurantResponse.data.restaurantId;
      setRestaurantId(restId);
      
      const menuItems = await getMenuItemsByRestaurant(restId);
      setMenuItems(menuItems.data || []);
    } catch (err) {
      const error = err as AppError;

      setToast({ message: error.message || 'Failed to fetch menu items', type: 'error' });
    } finally {
      setLoading(false);
    }
  };

  const validateForm = (): boolean => {
    const errors: Record<string, string> = {};

    if (!formData.itemName.trim()) {
      errors.itemName = 'Item name is required';
    }
    if (!formData.description.trim()) {
      errors.description = 'Description is required';
    }
    if (formData.availableQty < 0) {
      errors.availableQty = 'Available quantity must be 0 or greater';
    }
    if (formData.price <= 0) {
      errors.price = 'Price must be greater than 0';
    }
    if (formData.discount < 0 || formData.discount > 100) {
      errors.discount = 'Discount must be between 0 and 100';
    }
    if (formData.tax < 0) {
      errors.tax = 'Tax must be 0 or greater';
    }

    setFormErrors(errors);
    return Object.keys(errors).length === 0;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!validateForm()) return;

    try {
      if (editingItem) {
        const updateData: UpdateMenuItem = {
          itemName: formData.itemName,
          description: formData.description,
          availableQty: formData.availableQty,
          discount: formData.discount,
          price: formData.price,
          isVegetarian: formData.isVegetarian,
          tax: formData.tax,
        };
        
        if (formData.image) {
          updateData.image = formData.image;
        }
        
        await updateMenuItem(editingItem.itemId, updateData);
        setToast({ message: 'Menu item updated successfully', type: 'success' });
        setEditingItem(null);
      } else {
        const createData: CreateMenuItem = {
          restaurantId: restaurantId!,
          itemName: formData.itemName,
          description: formData.description,
          availableQty: formData.availableQty,
          discount: formData.discount,
          price: formData.price,
          isVegetarian: formData.isVegetarian,
          tax: formData.tax,
        };
        
        if (formData.image) {
          createData.image = formData.image;
        }
        
        await createMenuItem(createData);
        setToast({ message: 'Menu item created successfully', type: 'success' });
        setShowAddForm(false);
      }
      
      resetForm();
      fetchMenuItems();
    } catch (err) {
      const error = err as AppError;
      setToast({ message: error.message || 'Failed to save menu item', type: 'error' });
    }
  };

  const handleDelete = async (id: number) => {
    if (!confirm('Are you sure you want to delete this menu item?')) return;

    try {
      await deleteMenuItem(id);
      setToast({ message: 'Menu item deleted successfully', type: 'success' });
      fetchMenuItems();
    } catch (err) {
      const error = err as AppError;
      setToast({ message: error.message || 'Failed to delete menu item', type: 'error' });
    }
  };

  const handleEdit = (item: MenuItem) => {
    setEditingItem(item);
    setFormData({
      restaurantId: item.restaurantId,
      itemName: item.itemName,
      description: item.description,
      availableQty: item.availableQty,
      discount: item.discount,
      price: item.price,
      isVegetarian: item.isVegetarian,
      tax: item.tax,
      image: undefined, // Reset image for editing
    });
    setShowAddForm(true);
  };

  const resetForm = () => {
    setFormData({
      restaurantId: restaurantId || 0,
      itemName: '',
      description: '',
      availableQty: 0,
      discount: 0,
      price: 0,
      isVegetarian: false,
      tax: 0,
      image: undefined,
    });
    setFormErrors({});
  };

  const handleCancel = () => {
    setShowAddForm(false);
    setEditingItem(null);
    resetForm();
  };

  const handleImageChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file) {
      setFormData({ ...formData, image: file });
    } else {
      setFormData({ ...formData, image: undefined });
    }
  };

  if (loading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: 400 }}>
        <CircularProgress sx={{ color: 'var(--primary-color)' }} />
        <Typography sx={{ ml: 2, color: 'var(--text-color)' }}>Loading menu items...</Typography>
      </Box>
    );
  }

  return (
    <Box sx={{ p: 3, width: '100%', maxWidth: '100vw', boxSizing: 'border-box', overflowX: 'hidden' }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
          <MenuIcon sx={{ color: 'var(--primary-color)', fontSize: 32 }} />
          <Typography variant="h4" sx={{ fontWeight: 600, color: 'var(--text-color)' }}>
            Menu Management
          </Typography>
        </Box>
        
        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={() => setShowAddForm(true)}
          sx={{ bgcolor: 'var(--primary-color)', '&:hover': { bgcolor: 'var(--primary-hover)' } }}
        >
          Add New Item
        </Button>
      </Box>

      <Dialog open={showAddForm} onClose={handleCancel} maxWidth="md" fullWidth>
        <DialogTitle sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <Typography variant="h5" sx={{ fontWeight: 600, color: 'var(--text-color)' }}>
            {editingItem ? 'Edit Menu Item' : 'Add New Menu Item'}
          </Typography>
          <IconButton onClick={handleCancel}>
            <CloseIcon />
          </IconButton>
        </DialogTitle>
        
        <DialogContent>
          <Box component="form" onSubmit={handleSubmit} sx={{ pt: 2 }}>
              <div className="form-row">
                <div className="form-group">
                  <label>Item Name *</label>
                  <input
                    type="text"
                    value={formData.itemName}
                    onChange={(e) => setFormData({ ...formData, itemName: e.target.value })}
                    className={formErrors.itemName ? 'error' : ''}
                  />
                  {formErrors.itemName && <span className="error-text">{formErrors.itemName}</span>}
                </div>
                
                <div className="form-group">
                  <label>Price *</label>
                  <input
                    type="number"
                    step="0.01"
                    value={formData.price}
                    onChange={(e) => setFormData({ ...formData, price: parseFloat(e.target.value) || 0 })}
                    className={formErrors.price ? 'error' : ''}
                  />
                  {formErrors.price && <span className="error-text">{formErrors.price}</span>}
                </div>
              </div>

              <div className="form-group">
                <label>Description *</label>
                <textarea
                  value={formData.description}
                  onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                  className={formErrors.description ? 'error' : ''}
                  rows={3}
                />
                {formErrors.description && <span className="error-text">{formErrors.description}</span>}
              </div>

              <div className="form-row">
                <div className="form-group">
                  <label>Available Quantity</label>
                  <input
                    type="number"
                    value={formData.availableQty}
                    onChange={(e) => setFormData({ ...formData, availableQty: parseInt(e.target.value) || 0 })}
                    className={formErrors.availableQty ? 'error' : ''}
                  />
                  {formErrors.availableQty && <span className="error-text">{formErrors.availableQty}</span>}
                </div>
                
                <div className="form-group">
                  <label>Discount (%)</label>
                  <input
                    type="number"
                    step="0.01"
                    value={formData.discount}
                    onChange={(e) => setFormData({ ...formData, discount: parseFloat(e.target.value) || 0 })}
                    className={formErrors.discount ? 'error' : ''}
                  />
                  {formErrors.discount && <span className="error-text">{formErrors.discount}</span>}
                </div>
              </div>

              <div className="form-row">
                <div className="form-group">
                  <label>Tax (%)</label>
                  <input
                    type="number"
                    step="0.01"
                    value={formData.tax}
                    onChange={(e) => setFormData({ ...formData, tax: parseFloat(e.target.value) || 0 })}
                    className={formErrors.tax ? 'error' : ''}
                  />
                  {formErrors.tax && <span className="error-text">{formErrors.tax}</span>}
                </div>
                
                <div className="form-group">
                  <label className="checkbox-label">
                    <input
                      type="checkbox"
                      checked={formData.isVegetarian}
                      onChange={(e) => setFormData({ ...formData, isVegetarian: e.target.checked })}
                    />
                    Vegetarian
                  </label>
                </div>
              </div>

              <div className="form-group">
                <label>Image {editingItem && '(Select new image to replace current one)'}</label>
                <input
                  type="file"
                  accept="image/*"
                  onChange={handleImageChange}
                  key={editingItem ? editingItem.itemId : 'new'}
                />
                {editingItem && editingItem.image && (
                  <div className="current-image-preview">
                    <small>Current image:</small>
                    <img 
                      src={`data:image/jpeg;base64,${editingItem.image}`} 
                      alt="Current" 
                      style={{ width: '50px', height: '50px', objectFit: 'cover', marginTop: '5px' }}
                    />
                  </div>
                )}
              </div>

              <div className="form-actions">
                <button type="button" className="cancel-btn" onClick={handleCancel}>
                  Cancel
                </button>
                <button type="submit" className="submit-btn">
                  {editingItem ? 'Update Item' : 'Add Item'}
                </button>
              </div>
            </Box>
        </DialogContent>
      </Dialog>

      <div className="menu-items-section">
        <h2>Menu Items (Count: {menuItems.length})</h2>
        <div className="menu-items-grid">
          {menuItems.length === 0 ? (
            <div className="no-items">No menu items found. Add your first item!</div>
          ) : (
            menuItems.map((item) => (
              <div key={item.itemId} className="menu-item-card">
                {item.image && (
                  <div className="item-image">
                    <img src={`data:image/jpeg;base64,${item.image}`} alt={item.itemName} />
                  </div>
                )}
                
                <div className="item-content">
                  <div className="item-header">
                    <h3>{item.itemName}</h3>
                    {item.isVegetarian && <span className="veg-badge">🌱</span>}
                  </div>
                  
                  <p className="item-description">{item.description}</p>
                  
                  <div className="item-details">
                    <div className="price-info">
                      <span className="price">₹{item.price}</span>
                      {item.discount > 0 && (
                        <span className="discount">{item.discount}% off</span>
                      )}
                    </div>
                    
                    <div className="quantity-info">
                      <span className={`qty ${item.availableQty === 0 ? 'out-of-stock' : ''}`}>
                        {item.availableQty === 0 ? 'Out of Stock' : `${item.availableQty} available`}
                      </span>
                    </div>
                  </div>
                  
                  <div className="item-actions">
                    <button className="edit-btn" onClick={() => handleEdit(item)}>
                      Edit
                    </button>
                    <button className="delete-btn" onClick={() => handleDelete(item.itemId)}>
                      Delete
                    </button>
                  </div>
                </div>
              </div>
            ))
          )}
        </div>
      </div>

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

export default MenuManagement;