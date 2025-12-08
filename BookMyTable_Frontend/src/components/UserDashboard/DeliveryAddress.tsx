import { useEffect, useState } from 'react';
import { useAppSelector } from '../../redux/hooks';
import { Navbar } from '../Customer/Navbar';
import '../../Styles/UserDashboard/UserDashboard.css';
import '../../Styles/UserDashboard/DeliveryAddress.css';
import type { AppError } from '../../Types/Dashboard/ApiError';
import type { UserDeliveryAddress } from '../../Types/UserDashboard/UserDeliveryAddress';
import { getUserAddresses } from '../../Service/dashboard.api';
import { changeDefaultAddress } from '../../Service/restaurant.api';
import type { ChangeDeliveryAddress } from '../../Types/Restaurant/ChangeDeliveryAddress';
import Toast, { type ToastType } from '../../Utils/Toast';
import { http } from '../../Service/https';
import deliveryAddressImage from '../../assets/DeliveryAddress.jpeg.jpg';

const DeliveryAddress = () => {
  const [addresses, setAddresses] = useState<UserDeliveryAddress[]>([]);
  const [profile, setProfile] = useState<any>(null);
  const [loading, setLoading] = useState(true);
  const [profileLoading, setProfileLoading] = useState(true);
  const [error, setError] = useState<string>('');
  const [profileError, setProfileError] = useState<string>('');
  const [editingProfile, setEditingProfile] = useState(false);
  const [editedProfile, setEditedProfile] = useState<any>({});
  const [toast, setToast] = useState<{ message: string; type: ToastType } | null>(null);

  const showToast = (message: string, type: ToastType) => {
    setToast({ message, type });
  };
  const user = useAppSelector((state) => state.auth.user);

  useEffect(() => {
    fetchAddresses();
    fetchProfile();
  }, [user?.userId]);

  const fetchProfile = async () => {
    try {
      setProfileLoading(true);
      setProfileError('');
      if (user?.userId) {
        const response = await http.get(`/UserProfile/${user.userId}`);
        setProfile(response.data.data);
        setEditedProfile(response.data.data);
      }
    } catch (err) {
      const error = err as AppError;
      setProfileError(error.message || 'Failed to fetch profile');
    } finally {
      setProfileLoading(false);
    }
  };

  const updateProfile = async () => {
    try {
      if (user?.userId) {
        await http.put(`/UserProfile/${user.userId}`, editedProfile);
        setProfile(editedProfile);
        setEditingProfile(false);
      }
    } catch (err) {
      const error = err as AppError;
      setProfileError(error.message || 'Failed to update profile');
    }
  };

  const handleProfileEdit = () => {
    setEditingProfile(true);
    setEditedProfile({ ...profile });
  };

  const handleProfileCancel = () => {
    setEditingProfile(false);
    setEditedProfile({ ...profile });
  };

  const handleProfileChange = (field: string, value: string) => {
    setEditedProfile({ ...editedProfile, [field]: value });
  };

  const fetchAddresses = async () => {
    try {
      setLoading(true);
      setError('');
      if (user?.userId) {
        const response = await getUserAddresses(user.userId);
        const userAddresses = response.data;
        if (Array.isArray(userAddresses)) {
          setAddresses(userAddresses);
        } else {
          setAddresses([]);
        }
      } else {
        setAddresses([]);
      }
    } catch (err) {
      const error = err as AppError;
      setError(error.message || 'Failed to fetch addresses');
      setAddresses([]);
    } finally {
      setLoading(false);
    }
  };

  const handleMakeDefault = async (addressId: number) => {
    try {
      if (!user?.userId) return;
      
      const payload: ChangeDeliveryAddress = {
        userId: user.userId,
        deliveryAddressId: addressId
      };
      
      await changeDefaultAddress(payload);
      showToast('Default address updated successfully!', 'success');
      fetchAddresses();
    } catch (err) {
      const error = err as AppError;
      showToast(error.message || 'Failed to set default address', 'error');
    }
  };

  if (loading) {
    return (
      <>
        <Navbar />
        <div className="delivery-address-container">
          <div className="delivery-address-loading">Loading addresses...</div>
        </div>
      </>
    );
  }

  if (error) {
    return (
      <>
        <Navbar />
        <div className="delivery-address-container">
          <div className="delivery-address-error">{error}</div>
        </div>
      </>
    );
  }

  return (
    <>
      {toast && (
        <Toast
          message={toast.message}
          type={toast.type}
          onClose={() => setToast(null)}
        />
      )}
      <Navbar />
      <div className="delivery-address-container" style={{ paddingTop: '64px' }}>
        <div className="delivery-address-header">
          <h1 className="delivery-address-title">Profile & Delivery Addresses</h1>
        </div>
        
        {/* User Profile Section */}
        <div className="profile-section" style={{ marginBottom: '2rem' }}>
          <h2 style={{ color: 'var(--primary-color)', marginBottom: '1rem' }}>User Profile</h2>
          {profileLoading ? (
            <div className="delivery-address-loading">Loading profile...</div>
          ) : profileError ? (
            <div className="delivery-address-error">{profileError}</div>
          ) : profile ? (
            <div className="delivery-address-card" style={{ maxWidth: '600px' }}>
              <div className="delivery-address-details">
                {editingProfile ? (
                  <>
                    <div className="delivery-address-info-row">
                      <span className="delivery-address-label">First Name:</span>
                      <input 
                        type="text" 
                        value={editedProfile.firstName || ''}
                        onChange={(e) => handleProfileChange('firstName', e.target.value)}
                        style={{ flex: 1, padding: '8px', border: '1px solid var(--border-color)', borderRadius: '4px' }}
                      />
                    </div>
                    <div className="delivery-address-info-row">
                      <span className="delivery-address-label">Last Name:</span>
                      <input 
                        type="text" 
                        value={editedProfile.lastName || ''}
                        onChange={(e) => handleProfileChange('lastName', e.target.value)}
                        style={{ flex: 1, padding: '8px', border: '1px solid var(--border-color)', borderRadius: '4px' }}
                      />
                    </div>
                    <div className="delivery-address-info-row">
                      <span className="delivery-address-label">Email:</span>
                      <input 
                        type="email" 
                        value={editedProfile.email || ''}
                        onChange={(e) => handleProfileChange('email', e.target.value)}
                        style={{ flex: 1, padding: '8px', border: '1px solid var(--border-color)', borderRadius: '4px' }}
                      />
                    </div>
                    <div className="delivery-address-info-row">
                      <span className="delivery-address-label">Mobile:</span>
                      <input 
                        type="tel" 
                        value={editedProfile.mobile || ''}
                        onChange={(e) => handleProfileChange('mobile', e.target.value)}
                        style={{ flex: 1, padding: '8px', border: '1px solid var(--border-color)', borderRadius: '4px' }}
                      />
                    </div>
                  </>
                ) : (
                  <>
                    <div className="delivery-address-info-row">
                      <span className="delivery-address-label">Name:</span>
                      <span className="delivery-address-value">{profile.firstName} {profile.lastName}</span>
                    </div>
                    <div className="delivery-address-info-row">
                      <span className="delivery-address-label">Email:</span>
                      <span className="delivery-address-value">{profile.email}</span>
                    </div>
                    <div className="delivery-address-info-row">
                      <span className="delivery-address-label">Mobile:</span>
                      <span className="delivery-address-value">{profile.mobile}</span>
                    </div>
                  </>
                )}
              </div>
              <div className="delivery-address-actions">
                {editingProfile ? (
                  <>
                    <button 
                      onClick={updateProfile}
                      className="delivery-address-remove-btn"
                      style={{ backgroundColor: '#e07a1aff', marginRight: '10px', color: 'white' }}
                    >
                      Save Changes
                    </button>
                    <button 
                      onClick={handleProfileCancel}
                      className="delivery-address-remove-btn"
                      style={{ backgroundColor: '#6c757d', color: 'white' }}
                    >
                      Cancel
                    </button>
                  </>
                ) : (
                  <button 
                    onClick={handleProfileEdit}
                    className="delivery-address-remove-btn"
                    style={{ backgroundColor: '#e47b0bff', color: 'white', padding:'10px',borderRadius:'15px', borderColor:'white'  }}
                  >
                    Edit Profile
                  </button>
                )}
              </div>
            </div>
          ) : (
            <div className="delivery-address-empty">
              <p>Profile not found.</p>
            </div>
          )}
        </div>
        
        {/* Delivery Addresses Section */}
        <div className="addresses-section">
          <h2 style={{ color: 'var(--primary-color)', marginBottom: '1rem' }}>Delivery Addresses</h2>
        <div className="user-dashboard-content">
          {addresses.length === 0 ? (
            <div className="delivery-address-empty">
              <p>No delivery addresses found. Add your first address!</p>
            </div>
          ) : (
            <div className="delivery-address-grid">
              {addresses.map((address: any) => (
                <div key={address.addressId} className={`delivery-address-card ${address.isDefault ? 'default-address' : ''}`}>
                  {address.isDefault && <div className="delivery-address-default">Default Address</div>}
                  <div className="delivery-address-icon">
                    <img src={deliveryAddressImage} alt="Delivery Address" className="delivery-address-image" />
                  </div>
                  <div className="delivery-address-details">
                    <div className="delivery-address-info-row">
                      <span className="delivery-address-label">Address:</span>
                      <span className="delivery-address-value">{address.address}</span>
                    </div>
                    <div className="delivery-address-info-row">
                      <span className="delivery-address-label">City:</span>
                      <span className="delivery-address-value">{address.city}</span>
                    </div>
                    <div className="delivery-address-info-row">
                      <span className="delivery-address-label">State:</span>
                      <span className="delivery-address-value">{address.state}</span>
                    </div>
                    <div className="delivery-address-info-row">
                      <span className="delivery-address-label">Pincode:</span>
                      <span className="delivery-address-value">{address.pincode}</span>
                    </div>
                    <div className="delivery-address-info-row">
                      <span className="delivery-address-label">Mobile:</span>
                      <span className="delivery-address-value">{address.mobile}</span>
                    </div>
                  </div>
                  <div className="delivery-address-actions">
                    {!address.isDefault && (
                      <button 
                        onClick={() => handleMakeDefault(address.addressId)}
                        className="delivery-address-default-btn"
                      >
                        Make Default Address
                      </button>
                    )}
                  </div>
                </div>
              ))}
            </div>
          )}
        </div>
        </div>
      </div>
    </>
  );
};

export default DeliveryAddress;