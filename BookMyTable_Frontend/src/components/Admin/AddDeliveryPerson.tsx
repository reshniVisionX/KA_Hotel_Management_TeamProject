import { useState } from 'react';
import { addDeliveryPerson } from '../../Service/admin.api';
import type { CreateDeliveryPersonRequest } from '../../Types/Admin/DeliveryPerson';
import type { AppError } from '../../Types/Dashboard/ApiError';
import Toast, { type ToastType } from '../../Utils/Toast';
import '../../Styles/Admin/AddDeliveryPerson.css';

interface ValidationErrors {
  deliveryName?: string;
  mobileNo?: string;
  email?: string;
  licenseNumber?: string;
}

const AddDeliveryPerson = () => {
  const [formData, setFormData] = useState<CreateDeliveryPersonRequest>({
    deliveryName: '',
    mobileNo: '',
    email: '',
    licenseNumber: ''
  });

  const [errors, setErrors] = useState<ValidationErrors>({});
  const [loading, setLoading] = useState(false);
  const [toast, setToast] = useState<{ message: string; type: ToastType } | null>(null);

  const validateField = (name: string, value: string): string => {
    switch (name) {
      case 'deliveryName':
        if (!value.trim()) return 'Name is required';
        if (!/^[A-Za-z\s]+$/.test(value)) return 'Name should contain only alphabets';
        return '';
      
      case 'mobileNo':
        if (!value) return 'Mobile number is required';
        if (!/^[6-9]\d{9}$/.test(value)) return 'Mobile number must start with 6-9 and be exactly 10 digits';
        return '';
      
      case 'email':
        if (!value) return 'Email is required';
        if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value)) return 'Please enter a valid email';
        return '';
      
      case 'licenseNumber':
        if (!value.trim()) return 'License number is required';
        if (!/^[A-Z]{2}\d{6}$/.test(value)) return 'License number should be in format: XX123456 (2 letters + 6 digits)';
        return '';
      
      default:
        return '';
    }
  };

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    
    // Restrict mobile number input to 10 digits
    if (name === 'mobileNo' && value.length > 10) return;
    
    setFormData(prev => ({ ...prev, [name]: value }));
    
    // Clear error when user starts typing
    if (errors[name as keyof ValidationErrors]) {
      setErrors(prev => ({ ...prev, [name]: '' }));
    }
  };

  const handleBlur = (e: React.FocusEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    const error = validateField(name, value);
    setErrors(prev => ({ ...prev, [name]: error }));
  };

  const validateForm = (): boolean => {
    const newErrors: ValidationErrors = {};
    
    Object.keys(formData).forEach(key => {
      const error = validateField(key, formData[key as keyof CreateDeliveryPersonRequest]);
      if (error) newErrors[key as keyof ValidationErrors] = error;
    });
    
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!validateForm()) return;
    
    try {
      setLoading(true);
      const response = await addDeliveryPerson(formData);
      setToast({ message: response.message, type: 'success' });
      
      // Reset form
      setFormData({
        deliveryName: '',
        mobileNo: '',
        email: '',
        licenseNumber: ''
      });
      setErrors({});
    } catch (err) {
      const error = err as AppError;
      setToast({ message: error.message, type: 'error' });
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="add-delivery-person-container">
      <div className="form-header">
        <h2>Add Delivery Person</h2>
        <p>Register a new delivery person to the system</p>
      </div>

      <form onSubmit={handleSubmit} className="delivery-person-form">
        <div className="form-group">
          <label htmlFor="deliveryName">
            Full Name <span className="required">*</span>
          </label>
          <input
            type="text"
            id="deliveryName"
            name="deliveryName"
            value={formData.deliveryName}
            onChange={handleInputChange}
            onBlur={handleBlur}
            className={errors.deliveryName ? 'error' : ''}
            placeholder="Enter full name"
          />
          {errors.deliveryName && <span className="error-message">{errors.deliveryName}</span>}
        </div>

        <div className="form-group">
          <label htmlFor="mobileNo">
            Mobile Number <span className="required">*</span>
          </label>
          <input
            type="tel"
            id="mobileNo"
            name="mobileNo"
            value={formData.mobileNo}
            onChange={handleInputChange}
            onBlur={handleBlur}
            className={errors.mobileNo ? 'error' : ''}
            placeholder="Enter 10-digit mobile number"
            maxLength={10}
          />
          {errors.mobileNo && <span className="error-message">{errors.mobileNo}</span>}
        </div>

        <div className="form-group">
          <label htmlFor="email">
            Email Address <span className="required">*</span>
          </label>
          <input
            type="email"
            id="email"
            name="email"
            value={formData.email}
            onChange={handleInputChange}
            onBlur={handleBlur}
            className={errors.email ? 'error' : ''}
            placeholder="Enter email address"
          />
          {errors.email && <span className="error-message">{errors.email}</span>}
        </div>

        <div className="form-group">
          <label htmlFor="licenseNumber">
            License Number <span className="required">*</span>
          </label>
          <input
            type="text"
            id="licenseNumber"
            name="licenseNumber"
            value={formData.licenseNumber}
            onChange={handleInputChange}
            onBlur={handleBlur}
            className={errors.licenseNumber ? 'error' : ''}
            placeholder="e.g., TN123456"
            style={{ textTransform: 'uppercase' }}
          />
          {errors.licenseNumber && <span className="error-message">{errors.licenseNumber}</span>}
        </div>

        <button type="submit" className="submit-btn" disabled={loading}>
          {loading ? 'Adding...' : 'Add Delivery Person'}
        </button>
      </form>

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

export default AddDeliveryPerson;
