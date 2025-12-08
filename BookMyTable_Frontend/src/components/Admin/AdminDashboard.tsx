import React, { useState } from "react";
import { AdminNavbar } from "./AdminNavbar";
import "../../Styles/Dashboard/AdminDashboard.css";
import { Outlet } from "react-router-dom";
import Footer from "../Customer/Footer";

const AdminDashboard: React.FC = () => {
  const [sidebarOpen, setSidebarOpen] = useState(false);

  return (
    <div className={`admin-container ${sidebarOpen ? 'sidebar-open' : ''}`}>
      <AdminNavbar sidebarOpen={sidebarOpen} setSidebarOpen={setSidebarOpen} />
      <main className="admin-content">
        <Outlet />
      </main>
       <Footer />
    </div>
  );
};

export default AdminDashboard;
