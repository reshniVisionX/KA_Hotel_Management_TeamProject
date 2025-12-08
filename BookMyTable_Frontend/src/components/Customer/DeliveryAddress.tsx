import { useEffect, useState } from "react";
import { useAppSelector, useAppDispatch } from "../../redux/hooks";
import { setCity } from "../../redux/slices/authSlice";

import {
  getDeliveryAddresses,
  createDeliveryAddress,
  changeDefaultAddress
} from "../../Service/restaurant.api";

import type { DeliveryAddress } from "../../Types/Restaurant/DeliveryAddress";
import type { CreateDeliveryAddress } from "../../Types/Restaurant/CreateDeliveryAddress";
import type { ChangeDeliveryAddress } from "../../Types/Restaurant/ChangeDeliveryAddress";

import Toast, {type ToastType } from "../../Utils/Toast";
import "../../Styles/Restaurant/DeliveryAddress.css";
import type { AppError } from "../../Types/Dashboard/ApiError";

interface DeliveryAddressProps {
  isOpen: boolean;
  onClose: () => void;
}

export function DeliveryAddress({ isOpen, onClose }: DeliveryAddressProps) {
  const user = useAppSelector((state) => state.auth.user);
  const userId = user?.userId ?? 0;
  const dispatch = useAppDispatch();

  const [addresses, setAddresses] = useState<DeliveryAddress[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [showAddForm, setShowAddForm] = useState<boolean>(false);
  const [toast, setToast] = useState<{ message: string; type: ToastType } | null>(null);

  const statesCities = {
    "Tamil Nadu": ["Chennai", "Coimbatore", "Madurai"],
    "Karnataka": ["Bangalore", "Mysore", "Mangalore"],
    "Kerala": ["Kochi", "Thiruvananthapuram", "Kozhikode"],
    "Andhra Pradesh": ["Hyderabad", "Visakhapatnam", "Vijayawada"],
    "Telangana": ["Hyderabad", "Warangal", "Nizamabad"]
  };

  
  const showToast = (message: string, type: ToastType) => {
    setToast({ message, type });
  };

  const [form, setForm] = useState<CreateDeliveryAddress>({
    userId,
    mobile: "",
    address: "",
    city: "",
    state: "",
    pincode: "",
    landmark: "",
    contactNo: "",
    isDefault: true
  });
 const availableCities = form.state ? statesCities[form.state as keyof typeof statesCities] || [] : [];

 const loadAddresses = async () => {
  try {
    setLoading(true);
    const res = await getDeliveryAddresses(userId);
    
    if (!res.data || res.data.length === 0) {
      showToast("No existing address found", "error");
      setShowAddForm(true);
    } else {
      setAddresses(res.data);
    }
  } catch (err) {
    const error = err as AppError;
    showToast(error.message, "error");
  } finally {
    setLoading(false);
  }
};

const handleClose = () => {
  if (addresses.length === 0 && !showAddForm) {
    showToast("Please add an address to continue", "error");
    return;
  }
  onClose();
};
  useEffect(() => {
    if (userId > 0 && isOpen) {
      loadAddresses();
    }
  }, [userId, isOpen]);

  const submitNewAddress = async (e: React.FormEvent) => {
    e.preventDefault();

    const payload: CreateDeliveryAddress = {
      ...form,
      userId,
      isDefault: true
    };

    try {
      const res = await createDeliveryAddress(payload);
      setAddresses((prev) => [...prev, res.data]);
      dispatch(setCity(res.data.city));
      showToast("Address added successfully!", "success");
      setShowAddForm(false);
      onClose();
    } catch (err) {
      const error = err as AppError;
      showToast(error.message, "error");
    }
  };

  const setDefaultAddress = async (addressId: number) => {
    const payload: ChangeDeliveryAddress = {
      userId,
      deliveryAddressId: addressId
    };

    try {
      await changeDefaultAddress(payload);
      
      const selectedAddress = addresses.find(addr => addr.addressId === addressId);
      if (selectedAddress) {
        dispatch(setCity(selectedAddress.city));
      }
      
      showToast("Default address updated!", "success");
      loadAddresses();
      onClose();
    } catch (err) {
      const error = err as AppError;
      showToast(error.message, "error");
    }
  };

  if (!isOpen) return null;

  return (
    <div className="dm-overlay-backdrop">
      {toast && (
        <Toast
          message={toast.message}
          type={toast.type}
          onClose={() => setToast(null)}
        />
      )}

      <div className="dm-modal-container">
        <div className="dm-modal-header">
          <h2 className="dm-modal-title">Select Delivery Address</h2>
        
          <button className="dm-close-btn" onClick={handleClose}>×</button>

        </div>

        {loading ? (
          <div className="dm-loading">Loading addresses...</div>
        ) : (
          <div className="dm-modal-content">
            {!showAddForm ? (
              <>
                <div className="dm-address-grid">
                  {addresses.map((addr) => (
                    <div
                      key={addr.addressId}
                      className={`dm-address-card ${addr.isDefault ? "dm-selected" : ""}`}
                      onClick={() => setDefaultAddress(addr.addressId)}
                    >
                      <div className="dm-address-info">
                        <p className="dm-address-text">{addr.address}</p>
                        <p className="dm-address-location">
                          {addr.city}, {addr.state} - {addr.pincode}
                        </p>
                      </div>
                      {addr.isDefault && <span className="dm-default-badge">✓</span>}
                    </div>
                  ))}
                </div>
                
                <button 
                  className="dm-add-new-btn" 
                  onClick={() => setShowAddForm(true)}
                >
                  + Add New Address
                </button>



              </>
            ) : (
              <form onSubmit={submitNewAddress} className="dm-add-form">
                <div className="dm-form-grid">
                  <input
                    className="dm-input"
                    placeholder="Mobile Number"
                    value={form.mobile}
                    onChange={(e) => setForm({ ...form, mobile: e.target.value })}
                    required
                  />
                  <input
                    className="dm-input"
                    placeholder="Full Address"
                    value={form.address}
                    onChange={(e) => setForm({ ...form, address: e.target.value })}
                    required
                  />
                  <select
                    className="dm-input"
                    value={form.state}
                    onChange={(e) => setForm({ ...form, state: e.target.value, city: "" })}
                    required
                  >
                    <option value="">Select State</option>
                    {Object.keys(statesCities).map(state => (
                      <option key={state} value={state}>{state}</option>
                    ))}
                  </select>
                  <select
                    className="dm-input"
                    value={form.city}
                    onChange={(e) => setForm({ ...form, city: e.target.value })}
                    required
                    disabled={!form.state}
                  >
                    <option value="">Select City</option>
                    {availableCities.map(city => (
                      <option key={city} value={city}>{city}</option>
                    ))}
                  </select>
                  <input
                    className="dm-input"
                    placeholder="Pincode"
                    value={form.pincode}
                    onChange={(e) => setForm({ ...form, pincode: e.target.value })}
                    required
                  />
                </div>
                
                <div className="dm-form-actions">
                  <button type="submit" className="dm-save-btn">Save Address</button>
                  <button 
                    type="button" 
                    className="dm-cancel-btn" 
                    onClick={() => setShowAddForm(false)}
                  >
                    Cancel
                  </button>
                </div>
              </form>
            )}
          </div>
        )}
      </div>
    </div>
  );
}
