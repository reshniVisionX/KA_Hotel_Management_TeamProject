import "./App.css";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { LoginForm } from "./pages/LoginForm";
import ProtectedRoute from "./auth/ProtectedRoutes";
import CustomerDashboard from "./components/Customer/CustomerDashboard";
import AdminDashboard from "./components/Admin/AdminDashboard";
import ManagerDashboard from "./components/Manager/ManagerDashboard";

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
         
          {/* Dashboard-pradeep routes*/}

          {/* restaurant-hajeera routes*/}

        </Route>

        {/* MANAGER ROUTES */}
        <Route element={<ProtectedRoute allowedRoles={["Manager"]} />}>
          <Route path="/manager-dashboard" element={<ManagerDashboard />} />
          <Route path="/manager/restaurant" element={<ManagerDashboard />} />
          <Route path="/manager/menu" element={<ManagerDashboard />} />
          <Route path="/manager/orders" element={<ManagerDashboard />} />
          <Route path="/manager/analytics" element={<ManagerDashboard />} />
          <Route path="/manager/revenue" element={<ManagerDashboard />} />
          <Route path="/manager/reviews" element={<ManagerDashboard />} />
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
