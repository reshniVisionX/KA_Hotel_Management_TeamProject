import React, { useState } from "react";
import { ManagerNavbar } from "./ManagerNavbar";
import "../../Styles/Dashboard/ManagerDashboard.css";

const ManagerDashboard: React.FC = () => {
  const [sidebarOpen, setSidebarOpen] = useState(false);

  return (
    <div className={`manager-container ${sidebarOpen ? 'sidebar-open' : ''}`}>
      <ManagerNavbar sidebarOpen={sidebarOpen} setSidebarOpen={setSidebarOpen} />
      <main className="manager-content">
        <h1>Manager dashboard</h1>
      </main>
    </div>
  );
};

export default ManagerDashboard;
