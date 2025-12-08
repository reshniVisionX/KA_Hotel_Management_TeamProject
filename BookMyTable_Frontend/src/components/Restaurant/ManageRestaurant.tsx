import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { registerManagerWithRestaurant } from "../../Service/restaurant.api";
import type { ManagerRegisterRequest } from "../../Types/Restaurant/ManageRestaurant";
import Toast, { type ToastType } from "../../Utils/Toast";
import type { AppError } from "../../Types/Dashboard/ApiError";
import "../../Styles/Restaurant/ManageRestaurant.css";
import { useAppSelector } from "../../redux/hooks";

export const ManageRestaurant = () => {
    const navigate = useNavigate();
    const user = useAppSelector((state) => state.auth.user);
    
    // All hooks must be at the top
    const [formData, setFormData] = useState<ManagerRegisterRequest>({
        managerName: "",
        userId: 0,
        email: "",
        phoneNumber: "",
        password: "",
        restaurantName: "",
        description: "",
        ratings: 0,
        restaurantCategory: 0,
        restaurantType: 0,
        location: "",
        city: "",
        contactNo: "",
        deliveryCharge: 0,
        isActive: true,
        image: undefined
    });
    const [loading, setLoading] = useState(false);
    const [toast, setToast] = useState<{ message: string; type: ToastType } | null>(null);

    const cities = [
        "Chennai",
        "Coimbatore",
        "Madurai",
        "Bangalore",
        "Mysore",
        "Mangalore",
        "Kochi",
        "Thiruvananthapuram",
        "Kozhikode",
        "Hyderabad",
        "Visakhapatnam",
        "Vijayawada",
        "Warangal",
        "Nizamabad"
    ];

    useEffect(() => {
        if (user?.roleName === "Manager") {
            navigate("/manager-dashboard");
        }
    }, [user, navigate]);

    const showToast = (message: string, type: ToastType) => {
        setToast({ message, type });
    };

    // Conditional rendering after all hooks
    if (user?.roleName === "Manager") {
        return null;
    }

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
        const { name, value, type } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: type === 'number' ? parseFloat(value) || 0 : value
        }));
    };

    const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const file = e.target.files?.[0];
        setFormData(prev => ({ ...prev, image: file }));
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        if (!formData.managerName || !formData.email || !formData.restaurantName) {
            showToast("Please fill in all required fields", "error");
            return;
        }
        if (!user?.userId) {
            showToast("Please login to open a restaurant", "error");
            return;
        }

        try {
            setLoading(true);
            const updatedPayload = {
                ...formData,
                userId: user.userId 
            };

            await registerManagerWithRestaurant(updatedPayload);
            showToast("Manager and Restaurant registered successfully!", "success");
          
            // Reset form
            setFormData({
                managerName: "",
                userId: 0,
                email: "",
                phoneNumber: "",
                password: "",
                restaurantName: "",
                description: "",
                ratings: 0,
                restaurantCategory: 0,
                restaurantType: 0,
                location: "",
                city: "",
                contactNo: "",
                deliveryCharge: 0,
                isActive: true,
                image: undefined
            });
        } catch (err) {
            const error = err as AppError;
            showToast(error.message, "error");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="manage-restaurant-container">
            {toast && (
                <Toast
                    message={toast.message}
                    type={toast.type}
                    onClose={() => setToast(null)}
                />
            )}

            <div className="form-header">
                <h2>Register Manager & Restaurant</h2>
                <p>Create a new manager account with restaurant details</p>
            </div>

            <form onSubmit={handleSubmit} className="manager-form">
                {/* Manager Details */}
                <div className="form-section">
                    <h3>Manager Details</h3>
                    <div className="form-grid">
                        <div className="form-group">
                            <label>Manager Name *</label>
                            <input
                                type="text"
                                name="managerName"
                                value={formData.managerName}
                                onChange={handleInputChange}
                                required
                            />
                        </div>

                        <div className="form-group">
                            <label>Email *</label>
                            <input
                                type="email"
                                name="email"
                                value={formData.email}
                                onChange={handleInputChange}
                                required
                            />
                        </div>

                        <div className="form-group">
                            <label>Phone Number *</label>
                            <input
                                type="tel"
                                name="phoneNumber"
                                value={formData.phoneNumber}
                                onChange={handleInputChange}
                                required
                            />
                        </div>

                        <div className="form-group">
                            <label>Password *</label>
                            <input
                                type="password"
                                name="password"
                                value={formData.password}
                                onChange={handleInputChange}
                                required
                            />
                        </div>
                    </div>
                </div>

                {/* Restaurant Details */}
                <div className="form-section">
                    <h3>Restaurant Details</h3>
                    <div className="form-grid">
                        <div className="form-group">
                            <label>Restaurant Name *</label>
                            <input
                                type="text"
                                name="restaurantName"
                                value={formData.restaurantName}
                                onChange={handleInputChange}
                                required
                            />
                        </div>

                        <div className="form-group">
                            <label>Location *</label>
                            <input
                                type="text"
                                name="location"
                                value={formData.location}
                                onChange={handleInputChange}
                                required
                            />
                        </div>

                        <div className="form-group">
                            <label>City *</label>
                            <select
                                name="city"
                                value={formData.city}
                                onChange={handleInputChange}
                                required
                            >
                                <option value="">Select City</option>
                                {cities.map((city) => (
                                    <option key={city} value={city}>
                                        {city}
                                    </option>
                                ))}
                            </select>
                        </div>

                        <div className="form-group">
                            <label>Contact Number *</label>
                            <input
                                type="tel"
                                name="contactNo"
                                value={formData.contactNo}
                                onChange={handleInputChange}
                                required
                            />
                        </div>

                        <div className="form-group">
                            <label>Restaurant Category</label>
                            <select
                                name="restaurantCategory"
                                value={formData.restaurantCategory}
                                onChange={handleInputChange}
                            >
                                <option value={0}>Fast Food</option>
                                <option value={1}>Fine Dining</option>
                                <option value={2}>Casual Dining</option>
                                <option value={3}>Cafe</option>
                            </select>
                        </div>

                        <div className="form-group">
                            <label>Restaurant Type</label>
                            <select
                                name="restaurantType"
                                value={formData.restaurantType}
                                onChange={handleInputChange}
                            >
                                <option value={0}>Vegetarian</option>
                                <option value={1}>Non-Vegetarian</option>
                                <option value={2}>Both</option>
                            </select>
                        </div>

                        <div className="form-group">
                            <label>Delivery Charge</label>
                            <input
                                type="number"
                                name="deliveryCharge"
                                value={formData.deliveryCharge}
                                onChange={handleInputChange}
                                min="0"
                                step="0.01"
                            />
                        </div>

                        <div className="form-group">
                            <label>Restaurant Image</label>
                            <input
                                type="file"
                                accept="image/*"
                                onChange={handleFileChange}
                            />
                        </div>
                    </div>

                    <div className="form-group full-width">
                        <label>Description</label>
                        <textarea
                            name="description"
                            value={formData.description}
                            onChange={handleInputChange}
                            rows={4}
                            placeholder="Describe your restaurant..."
                        />
                    </div>
                </div>

                <button 
                    type="submit" 
                    className="submit-btn"
                    disabled={loading}
                >
                    {loading ? "Registering..." : "Register Manager & Restaurant"}
                </button>
            </form>
        </div>
    );
};
