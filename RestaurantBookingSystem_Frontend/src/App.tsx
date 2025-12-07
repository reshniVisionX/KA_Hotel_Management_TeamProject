import "./App.css";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { LoginForm } from "./pages/LoginForm";
import ProtectedRoute from "./auth/ProtectedRoutes";
import CustomerDashboard from "./components/Customer/CustomerDashboard";
import AdminDashboard from "./components/Admin/AdminDashboard";
import ManagerDashboard from "./components/Manager/ManagerDashboard";
import { DeliveryAddress } from "./components/Customer/DeliveryAddress";
import DeliveryAddressPage from "./components/UserDashboard/DeliveryAddress";
import MyOrders from "./components/UserDashboard/MyOrders";
import Wishlist from "./components/UserDashboard/Wishlist";

import MyTableBooking from "./components/UserDashboard/MyTableBooking";
import ManageReviews from "./components/UserDashboard/ManageReviews";

import Unauthorized from "./pages/Unauthorized";
import { Signup } from "./pages/SignUp";
import NotFound from "./pages/404Page";

function App() {
  return (
    <BrowserRouter>
      <Routes>

        <Route path="/" element={<LoginForm />} />
        
        <Route path="/login" element={<LoginForm />} />
        <Route path="/signup" element={<Signup />} />
        
        <Route path="/unauthorized" element={<Unauthorized />} />

        {/* CUSTOMER ROUTES */}
        <Route element={<ProtectedRoute allowedRoles={["Customer"]} />}>
          <Route path="/customer-dashboard" element={<CustomerDashboard />} />
          <Route path="/deliveryAddress" element={<DeliveryAddress />} />
          
          {/* UserDashboard routes */}

          <Route path="/customer/wishlist" element={<Wishlist />} />
          <Route path="/customer/orders" element={<MyOrders />} />
          <Route path="/customer/bookings" element={<MyTableBooking />} />
          <Route path="/customer/delivery-address" element={<DeliveryAddressPage />} />
          <Route path="/customer/reviews" element={<ManageReviews />} />


          {/* Dashboard-pradeep routes*/}

          {/* restaurant-hajeera routes*/}

        </Route>

        {/* MANAGER ROUTES */}
        <Route element={<ProtectedRoute allowedRoles={["Manager"]} />}>
          <Route path="/manager-dashboard" element={<ManagerDashboard />} />
        </Route>

        {/* ADMIN ROUTES */}
        <Route element={<ProtectedRoute allowedRoles={["Admin"]} />}>
          <Route path="/admin-dashboard" element={<AdminDashboard />} />
        </Route>

        <Route path="*" element={<NotFound />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
