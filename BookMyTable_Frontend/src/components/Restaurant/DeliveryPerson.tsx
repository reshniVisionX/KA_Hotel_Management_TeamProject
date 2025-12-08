import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { getAllDeliveryPersons } from "../../Service/restaurant.api";
import type { DeliveryPerson } from "../../Types/Restaurant/DeliveryPerson";
import Toast, { type ToastType } from "../../Utils/Toast";
import type { AppError } from "../../Types/Dashboard/ApiError";
import "../../Styles/Restaurant/DeliveryPerson.css";

export const DeliveryPersonList = () => {
  const navigate = useNavigate();
  const [deliveryPersons, setDeliveryPersons] = useState<DeliveryPerson[]>([]);
  const [loading, setLoading] = useState(true);
  const [toast, setToast] = useState<{ message: string; type: ToastType } | null>(null);

  const showToast = (message: string, type: ToastType) => {
    setToast({ message, type });
  };

  const fetchDeliveryPersons = async () => {
    try {
      setLoading(true);
      const response = await getAllDeliveryPersons();
      setDeliveryPersons(response.data);
    } catch (err) {
      const error = err as AppError;
      showToast(error.message, "error");
    } finally {
      setLoading(false);
    }
  };

  const handlePersonClick = (deliveryPersonId: number) => {
    navigate(`/delivery-history/${deliveryPersonId}`);
  };

  useEffect(() => {
    fetchDeliveryPersons();
  }, []);

  if (loading) {
    return (
      <div className="delivery-loading">
        <div className="loading-spinner"></div>
        <p>Loading delivery persons...</p>
      </div>
    );
  }

  return (
    <div className="delivery-container">
      {toast && (
        <Toast
          message={toast.message}
          type={toast.type}
          onClose={() => setToast(null)}
        />
      )}

      <div className="delivery-header">
        <h2>Delivery Team</h2>
        <p>Click on any delivery person to view their history</p>
      </div>

      <div className="delivery-grid">
        {deliveryPersons.map((person) => (
          <div
            key={person.deliveryPersonId}
            className="delivery-card"
            onClick={() => handlePersonClick(person.deliveryPersonId)}
          >
            <div className="person-avatar">
              {person.deliveryName.charAt(0).toUpperCase()}
            </div>
            
            <div className="person-info">
              <h3 className="person-name">{person.deliveryName}</h3>
              <p className="person-contact">{person.mobileNo}</p>
              <p className="person-email">{person.email}</p>
            </div>

            <div className="person-stats">
              <div className="stat-item">
                <span className="stat-label">License</span>
                <span className="stat-value">{person.licenseNumber}</span>
              </div>
              
              <div className="stat-item">
                <span className="stat-label">Rating</span>
                <span className="stat-value">
                  ⭐ {person.averageRating.toFixed(1)}
                </span>
              </div>
              
              <div className="stat-item">
                <span className="stat-label">Deliveries</span>
                <span className="stat-value">{person.totalDeliveries}</span>
              </div>
            </div>

            <div className={`availability-badge ${person.status === 1 ? 'available' : 'unavailable'}`}>
              {person.status === 0 ? 'Available' : 'OnDelivery'}
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}; 