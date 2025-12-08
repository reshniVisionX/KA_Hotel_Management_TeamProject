import React, { useState, useEffect } from "react";
import { Navbar } from "./Navbar";
import { DeliveryAddress } from "./DeliveryAddress";
import { useAppSelector } from "../../redux/hooks";
import { Outlet } from "react-router-dom";
import Footer from "./Footer"; 

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
     
      <DeliveryAddress 
        isOpen={showAddressModal} 
        onClose={handleAddressClose} 
      />
      <div className="customer-content">
        <Outlet />
      </div>
        <Footer />
    </div>
  );
};

export default CustomerDashboard;
