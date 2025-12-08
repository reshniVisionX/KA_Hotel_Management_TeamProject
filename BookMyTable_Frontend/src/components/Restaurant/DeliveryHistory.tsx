import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { getDeliveryPersonHistory, completeDelivery } from "../../Service/restaurant.api";
import type { DeliveryPersonHistory } from "../../Types/Restaurant/DeliveryPersonHistory";
import Toast, { type ToastType } from "../../Utils/Toast";
import type { AppError } from "../../Types/Dashboard/ApiError";
import "../../Styles/Restaurant/DeliveryHistory.css";

export const DeliveryHistoryPage = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();

  const [history, setHistory] = useState<DeliveryPersonHistory[]>([]);
  const [loading, setLoading] = useState(true);
  const [toast, setToast] = useState<{ message: string; type: ToastType } | null>(null);

  const deliveryPersonId = parseInt(id || "0");

  const showToast = (msg: string, type: ToastType) => {
    setToast({ message: msg, type });
  };

  const fetchHistory = async () => {
    try {
      setLoading(true);
      const res = await getDeliveryPersonHistory(deliveryPersonId);
      setHistory(res.data);
    } catch (err) {
      const error = err as AppError;
      showToast(error.message, "error");
    } finally {
      setLoading(false);
    }
  };

  const handleMarkDelivered = async (deliveryId: number | null) => {
    if (!deliveryId) return;
    
    try {
      await completeDelivery(deliveryId);
      showToast("Delivery marked as completed!", "success");
      fetchHistory();
    } catch (err) {
      const error = err as AppError;
      showToast(error.message, "error");
    }
  };

  const getStatusText = (status: number | null) => {
    switch (status) {
      case 0: return "Pending";
      case 1: return "Out for Delivery";
      case 2: return "Delivered";
      case 3: return "Failed";
      default: return "Unknown";
    }
  };

  const getStatusColor = (status: number | null) => {
    switch (status) {
      case 0: return "warning";
      case 1: return "info";
      case 2: return "success";
      case 3: return "error";
      default: return "info";
    }
  };

  useEffect(() => {
    if (deliveryPersonId > 0) {
      fetchHistory();
    }
  }, [id]);

  if (loading) {
    return (
      <div className="history-loading">
        <div className="loading-spinner"></div>
        <p>Loading delivery history...</p>
      </div>
    );
  }

  const pendingDeliveries = history.filter(d => d.deliveryStatus === 0);
  const completedDeliveries = history.filter(d => d.deliveryStatus !== 0);

  const renderDeliveryCard = (d: DeliveryPersonHistory, showButton = false) => (
    <div key={d.deliveryId || Math.random()} className="history-card">
      <div className="card-header">
        <span className="delivery-id">#{d.deliveryId}</span>
        <span className={`status-badge ${getStatusColor(d.deliveryStatus)}`}>
          {getStatusText(d.deliveryStatus)}
        </span>
      </div>

      <div className="card-content">
        <div className="customer-info">
          <strong>{d.customerFirstName} {d.customerLastName}</strong>
          <span className="phone">{d.mobile}</span>
        </div>
        
        <div className="address-info">
          {d.address}, {d.city}, {d.state} - {d.pincode}
          {d.landmark && <span className="landmark"> (Near: {d.landmark})</span>}
        </div>

      


        {d.estimatedDeliveryTime && (
          <div className="eta">
            🕒 ETA: {new Date(d.estimatedDeliveryTime).toLocaleString()}
          </div>
        )}
      </div>

      {showButton && (
        <div className="card-actions">
          <button 
            className="delivered-btn"
            onClick={() => handleMarkDelivered(d.deliveryId)}
          >
            Mark as Delivered
          </button>
        </div>
      )}
    </div>
  );

  return (
    <div className="history-container">
      {toast && (
        <Toast
          message={toast.message}
          type={toast.type}
          onClose={() => setToast(null)}
        />
      )}

      <div className="history-header">
        <button className="back-btn" onClick={() => navigate(-1)}>
          ← Back
        </button>
        <h3>Delivery History - {history[0]?.deliveryName}</h3>
      </div>

      {history.length === 0 ? (
        <div className="no-history">
          <h4>No Deliveries Found</h4>
          <p>This delivery person has no delivery records yet.</p>
        </div>
      ) : (
        <>
          {/* Pending Deliveries Section */}
          {pendingDeliveries.length > 0 && (
            <div className="pending-section">
              <h4 className="section-title">🚨 Pending Deliveries ({pendingDeliveries.length})</h4>
              <div className="history-grid">
                {pendingDeliveries.map(d => renderDeliveryCard(d, true))}
              </div>
            </div>
          )}

          {/* Completed Deliveries Section */}
          {completedDeliveries.length > 0 && (
            <div className="completed-section">
              <h4 className="section-title">✅ Delivery History ({completedDeliveries.length})</h4>
              <div className="history-grid">
                {completedDeliveries.map(d => renderDeliveryCard(d, false))}
              </div>
            </div>
          )}
        </>
      )}
    </div>
  );
};