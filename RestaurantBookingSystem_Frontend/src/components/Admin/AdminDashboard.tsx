import React, { useState } from "react";
import { AdminNavbar } from "./AdminNavbar";
import "../../Styles/Dashboard/AdminDashboard.css";

const AdminDashboard: React.FC = () => {
  const [sidebarOpen, setSidebarOpen] = useState(false);

  return (
    <div className={`admin-container ${sidebarOpen ? 'sidebar-open' : ''}`}>
      <AdminNavbar sidebarOpen={sidebarOpen} setSidebarOpen={setSidebarOpen} />
      <main className="admin-content">
        <h1>Admin dashboard</h1>
      </main>
    </div>
  );
};

export default AdminDashboard;
