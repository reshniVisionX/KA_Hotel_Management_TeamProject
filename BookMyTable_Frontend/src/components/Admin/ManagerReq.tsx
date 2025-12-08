import { useEffect, useState } from 'react';
import { getAllManagers, updateManagerVerification } from '../../Service/admin.api';
import type { AdminManager } from '../../Types/Admin/AdminManager';
import type { UpdateVerificationRequest } from '../../Service/admin.api';
import type { AppError } from '../../Types/Dashboard/ApiError';
import Toast, { type ToastType } from '../../Utils/Toast';
import '../../Styles/Admin/ManagerReq.css';

const ManagerReq = () => {
  const [managers, setManagers] = useState<AdminManager[]>([]);
  const [loading, setLoading] = useState(true);
  const [toast, setToast] = useState<{ message: string; type: ToastType } | null>(null);

  useEffect(() => {
    fetchManagers();
  }, []);

  const fetchManagers = async () => {
    try {
      setLoading(true);
      const response = await getAllManagers();
      setManagers(response.data);
    } catch (err) {
      const error = err as AppError;
      setToast({ message: error.message, type: 'error' });
    } finally {
      setLoading(false);
    }
  };

  const handleVerificationUpdate = async (managerId: number, verificationStatus: number) => {
    try {
      const payload: UpdateVerificationRequest = { managerId, verificationStatus };
      const response = await updateManagerVerification(payload);
      setToast({ message: response.message, type: 'success' });
      fetchManagers();
    } catch (err) {
      const error = err as AppError;
      setToast({ message: error.message, type: 'error' });
    }
  };

  const getVerificationStatus = (verification: number) => {
    switch (verification) {
      case 0: return 'Unverified';
      case 1: return 'Verified';
      case 2: return 'Rejected';
      default: return 'Unknown';
    }
  };

  const unverifiedManagers = managers.filter(manager => manager.verification === 0);
  const allManagers = managers;

  if (loading) {
    return <div className="managers-loading">Loading managers...</div>;
  }

  return (
    <div className="manager-req-container">
      <h1 className="managers-title">Manager Requests</h1>
      
      {unverifiedManagers.length > 0 && (
        <div className="unverified-section">
          <h2 className="section-title">Pending Verification</h2>
          <div className="managers-grid">
            {unverifiedManagers.map((manager) => (
              <div key={manager.managerId} className="manager-card unverified">
                <div className="manager-info">
                  <h3 className="manager-name">{manager.managerName}</h3>
                  <p className="manager-email">{manager.email}</p>
                  <p className="manager-phone">{manager.phoneNumber}</p>
                  <div className="manager-details">
                    <span className="manager-id">ID: {manager.managerId}</span>
                    <span className={`verification-status ${getVerificationStatus(manager.verification).toLowerCase()}`}>
                      {getVerificationStatus(manager.verification)}
                    </span>
                  </div>
                  <p className="manager-date">Created: {new Date(manager.createdAt).toLocaleDateString()}</p>
                </div>
                <div className="manager-actions">
                  <button
                    className="verify-btn"
                    onClick={() => handleVerificationUpdate(manager.managerId, 1)}
                  >
                    Verify
                  </button>
                  <button
                    className="reject-btn"
                    onClick={() => handleVerificationUpdate(manager.managerId, 2)}
                  >
                    Reject
                  </button>
                </div>
              </div>
            ))}
          </div>
        </div>
      )}

      <div className="all-managers-section">
        <h2 className="section-title">All Managers</h2>
        <div className="managers-grid">
          {allManagers.map((manager) => (
            <div key={manager.managerId} className="manager-card">
              <div className="manager-info">
                <h3 className="manager-name">{manager.managerName}</h3>
                <p className="manager-email">{manager.email}</p>
                <p className="manager-phone">{manager.phoneNumber}</p>
                <div className="manager-details">
                  <span className="manager-id">ID: {manager.managerId}</span>
                  <span className={`verification-status ${getVerificationStatus(manager.verification).toLowerCase()}`}>
                    {getVerificationStatus(manager.verification)}
                  </span>
                </div>
                <p className="manager-date">Created: {new Date(manager.createdAt).toLocaleDateString()}</p>
              </div>
              <div className="manager-actions">
                {manager.verification === 0 && (
                  <>
                    <button
                      className="verify-btn"
                      onClick={() => handleVerificationUpdate(manager.managerId, 1)}
                    >
                      Verify
                    </button>
                    <button
                      className="reject-btn"
                      onClick={() => handleVerificationUpdate(manager.managerId, 2)}
                    >
                      Reject
                    </button>
                  </>
                )}
                {manager.verification === 1 && (
                  <button
                    className="reject-btn"
                    onClick={() => handleVerificationUpdate(manager.managerId, 2)}
                  >
                    Reject
                  </button>
                )}
                {manager.verification === 2 && (
                  <button
                    className="verify-btn"
                    onClick={() => handleVerificationUpdate(manager.managerId, 1)}
                  >
                    Verify
                  </button>
                )}
              </div>
            </div>
          ))}
        </div>
      </div>

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

export default ManagerReq;
