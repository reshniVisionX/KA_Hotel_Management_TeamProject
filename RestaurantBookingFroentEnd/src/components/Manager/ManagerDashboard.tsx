import React, { useState } from "react";
import { useLocation } from "react-router-dom";
import { ManagerNavbar } from "./ManagerNavbar";
import ManagerDashboardOverview from "./ManagerDashboardOverview";
import ManagerAnalytics from "./ManagerAnalytics";
import MenuManagement from "./MenuManagement";
import OrderManagement from "./OrderManagement";
import Revenue from "./Revenue";
import Reviews from "./Reviews";
import RestaurantManagement from "./Restaurant";

import "../../Styles/Dashboard/ManagerDashboard.css";

const ManagerDashboard: React.FC = () => {
  const [sidebarOpen, setSidebarOpen] = useState(false);
  const location = useLocation();

  const renderContent = () => {
    switch (location.pathname) {
      case "/manager/analytics":
        return <ManagerAnalytics />;
      case "/manager/menu":
        return <MenuManagement />;
      case "/manager/orders":
        return <OrderManagement />;
      case "/manager/revenue":
        return <Revenue />;
      case "/manager/reviews":
        return <Reviews />;
      case "/manager/restaurant":
        return <RestaurantManagement />;

      default:
        return <ManagerDashboardOverview />;
    }
  };

  return (
    <div className={`manager-container ${sidebarOpen ? 'sidebar-open' : ''}`}>
      <ManagerNavbar sidebarOpen={sidebarOpen} setSidebarOpen={setSidebarOpen} />
      <main className="manager-content">
        {renderContent()}
      </main>
    </div>
  );
};

export default ManagerDashboard;
