import React, { useState, useEffect } from "react";
import { Navbar } from "./Navbar";
import { DeliveryAddress } from "./DeliveryAddress";
import { useAppSelector } from "../../redux/hooks";

const CustomerDashboard: React.FC = () => {
  const user = useAppSelector((state) => state.auth.user);
  const [showAddressModal, setShowAddressModal] = useState(false);

  useEffect(() => {
    if (user && !user.city) {
      setShowAddressModal(true);
    }
  }, [user]);

  const handleAddressClose = () => {
    setShowAddressModal(false);
  };

  return (
    <div className="customer-container">
      <Navbar />
      <h1>Customer Page</h1>
      <DeliveryAddress 
        isOpen={showAddressModal} 
        onClose={handleAddressClose} 
      />
    </div>
  );
};

export default CustomerDashboard;
