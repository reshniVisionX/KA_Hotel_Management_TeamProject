import "../../Styles/Admin/AdminNavbar.css";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { tokenstore } from "../../auth/tokenstore";
import logo from "../../assets/dining_service (1).jpg";
import { useDispatch } from "react-redux";
import { logout } from "../../redux/slices/authSlice";
import { useAppSelector } from "../../redux/hooks";

interface AdminNavbarProps {
  sidebarOpen: boolean;
  setSidebarOpen: (open: boolean) => void;
}

export const AdminNavbar = ({ sidebarOpen, setSidebarOpen }: AdminNavbarProps) => {
  const navigate = useNavigate();
  const [theme, setTheme] = useState<"light" | "dark">(tokenstore.getTheme());
  const dispatch = useDispatch();
  const user = useAppSelector((state) => state.auth.user);
  const displayName = user?.firstName?.trim() || "Admin";

  useEffect(() => {
    document.documentElement.setAttribute("data-theme", theme);
  }, [theme]);

  const toggleTheme = () => {
    const newTheme = theme === "light" ? "dark" : "light";
    setTheme(newTheme);
    tokenstore.setTheme(newTheme);
  };

  const menuItems = [
    { icon: "👥", label: "All Users", path: "/admin/users" },
    { icon: "📋", label: "Manager Requests", path: "/admin/requests" },
    { icon: "🏪", label: "Restaurants", path: "/admin/restaurants" },
    { icon: "💰", label: "Revenue Management", path: "/admin/revenue" },
    { icon: "📊", label: "Analytics", path: "/admin/analytics" },
    { icon: "📈", label: "Reports", path: "/admin/reports" },
    { icon: "⚙️", label: "Settings", path: "/admin/settings" }
  ];

  return (
    <>
      <div className="admin-layout">
       
        <header className="admin-header">
          <div className="header-left">
            <div className="brand">
              <img src={logo} alt="Logo" className="brand-logo" />
              <h1 className="brand-title">InstandEats Admin</h1>
            </div>
          </div>
          
          <div className="header-right">
            <button className="theme-toggle" onClick={toggleTheme}>
              {theme === "light" ? "🌙" : "☀️"}
            </button>
            <div className="admin-profile" onClick={() => setSidebarOpen(!sidebarOpen)}>
              <span className="admin-name">{displayName}</span>
              <div className="admin-avatar">{displayName.charAt(0).toUpperCase()}</div>
              </div>
          </div>
        </header>

        <aside className={`admin-sidebar ${sidebarOpen ? "open" : ""}`}>
          <nav className="sidebar-nav">
            <div className="nav-section">
              <h3 className="nav-section-title">Management</h3>
              {menuItems.map((item, index) => (
                <button
                  key={index}
                  className="nav-item"
                  onClick={() => navigate(item.path)}
                >
                  <span className="nav-icon">{item.icon}</span>
                  <span className="nav-label">{item.label}</span>
                </button>
              ))}
            </div>
            
            <div className="nav-section">
              <div className="nav-divider"></div>
              <button
                className="nav-item logout-btn"
                onClick={() => {
                  dispatch(logout());
                  navigate("/login");
                }}
              >
                <span className="nav-icon">🚪</span>
                <span className="nav-label">Logout</span>
              </button>
            </div>
          </nav>
        </aside>


      </div>
    </>
  );
};
