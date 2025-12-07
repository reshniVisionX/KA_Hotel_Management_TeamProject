import "../../Styles/Restaurant/Navbar.css";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { tokenstore } from "../../auth/tokenstore";
import logo from "../../assets/dining_service (1).jpg";
import { useDispatch } from "react-redux";
import { logout } from "../../redux/slices/authSlice";
import { useAppSelector } from "../../redux/hooks";

export const Navbar = () => {
  const navigate = useNavigate();
  const [theme, setTheme] = useState<"light" | "dark">(tokenstore.getTheme());
  const [sidebarOpen, setSidebarOpen] = useState(false);
  const dispatch = useDispatch();

  const user = useAppSelector((state) => state.auth.user);
  const displayName = user?.firstName?.trim() || "Customer";

  useEffect(() => {
    document.documentElement.setAttribute("data-theme", theme);
  }, [theme]);

  const toggleTheme = () => {
    const newTheme = theme === "light" ? "dark" : "light";
    setTheme(newTheme);
    tokenstore.setTheme(newTheme);
  };

  const menuItems = [
    { icon: "❤️", label: "Wishlist", path: "/customer/wishlist" },
    { icon: "🍽️", label: "My Orders", path: "/customer/orders" },
    { icon: "🪑", label: "My Table Bookings", path: "/customer/bookings" },
    { icon: "👤", label: "Profile & Address", path: "/customer/delivery-address" },
    { icon: "⭐", label: "Reviews", path: "/customer/reviews" }
  ];

  return (
    <>
      <header className="customer-header">
        <div className="header-left">
          <div className="brand">
            <img src={logo} alt="Logo" className="brand-logo" />
            <h1 className="brand-title">BookMyTable</h1>
          </div>
        </div>
        
        <div className="nav-center">
          <input
            type="text"
            className="nav-search"
            placeholder="Search for restaurants and foods"
          />
        </div>

        <div className="header-right">
          <span className="nav-link" onClick={() => navigate("/customer-dashboard")}>
            Home
          </span>
          <span className="nav-link" onClick={() => navigate("/cart")}>
            Cart 🛒
          </span>
          <button className="theme-toggle" onClick={toggleTheme}>
            {theme === "light" ? "🌙" : "☀️"}
          </button>
          <div className="customer-profile" onClick={() => setSidebarOpen(!sidebarOpen)}>
            <span className="customer-name">{displayName}</span>
            <div className="customer-avatar">{displayName.charAt(0).toUpperCase()}</div>
          </div>
        </div>
      </header>

      <aside className={`customer-sidebar ${sidebarOpen ? "open" : ""}`}>
        <nav className="sidebar-nav">
          <div className="nav-section">
            <h3 className="nav-section-title">My Account</h3>
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
    </>
  );
};
