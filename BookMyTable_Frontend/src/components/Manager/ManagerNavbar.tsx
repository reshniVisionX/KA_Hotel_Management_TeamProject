import "../../Styles/Manager/ManagerNavbar.css";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { tokenstore } from "../../auth/tokenstore";
import logo from "../../assets/dining_service (1).jpg";
import { useDispatch } from "react-redux";
import { logout } from "../../redux/slices/authSlice";
import { useAppSelector } from "../../redux/hooks";

interface ManagerNavbarProps {
  sidebarOpen: boolean;
  setSidebarOpen: (open: boolean) => void;
}

export const ManagerNavbar = ({ sidebarOpen, setSidebarOpen }: ManagerNavbarProps) => {
  const navigate = useNavigate();
  const [theme, setTheme] = useState<"light" | "dark">(tokenstore.getTheme());
  const dispatch = useDispatch();
  const user = useAppSelector((state) => state.auth.user);
  const displayName = user?.firstName?.trim() || "Manager";

  useEffect(() => {
    document.documentElement.setAttribute("data-theme", theme);
  }, [theme]);

  const toggleTheme = () => {
    const newTheme = theme === "light" ? "dark" : "light";
    setTheme(newTheme);
    tokenstore.setTheme(newTheme);
  };

  const menuItems = [
    { icon: "🏠", label: "Dashboard", path: "/manager-dashboard" },
    { icon: "🏪", label: "My Restaurant", path: "/manager/restaurant" },
    { icon: "🍽️", label: "Menu Management", path: "/manager/menu" },
    { icon: "📋", label: "Orders", path: "/manager/orders" },
    { icon: "📊", label: "Analytics", path: "/manager/analytics" },
    { icon: "💰", label: "Revenue", path: "/manager/revenue" },
    { icon: "⭐", label: "Reviews", path: "/manager/reviews" },
     { icon: "🏪", label: "Home", path: "/customer-dashboard" }
  ];

  return (
    <>
      <div className="manager-layout">
        <header className="manager-header">
          <div className="header-left">
            <div className="brand">
              <img src={logo} alt="Logo" className="brand-logo" />
              <h1 className="brand-title">InstantEats Manager</h1>
            </div>
          </div>
          
          <div className="header-right">
            <button className="theme-toggle" onClick={toggleTheme}>
              {theme === "light" ? "" : ""}
            </button>
            <div className="manager-profile" onClick={() => setSidebarOpen(!sidebarOpen)}>
              <span className="manager-name">{displayName}</span>
              <div className="manager-avatar">{displayName.charAt(0).toUpperCase()}</div>
            </div>
          </div>
        </header>

        <aside className={`manager-sidebar ${sidebarOpen ? "open" : ""}`}>
          <nav className="sidebar-nav">
            <div className="nav-section">
              <h3 className="nav-section-title">Management</h3>
              {menuItems.map((item, index) => (
                <button
                  key={index}
                  className="nav-item"
                  onClick={() => {
                    navigate(item.path);
                    setSidebarOpen(false);
                  }}
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

        {sidebarOpen && (
          <div 
            className="sidebar-overlay"
            onClick={() => setSidebarOpen(false)}
          ></div>
        )}
      </div>
    </>
  );
};