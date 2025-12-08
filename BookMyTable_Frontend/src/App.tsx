import "./App.css";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { LoginForm } from "./pages/LoginForm";
import ProtectedRoute from "./auth/ProtectedRoutes";
import CustomerDashboard from "./components/Customer/CustomerDashboard";
import AdminDashboard from "./components/Admin/AdminDashboard";
import ManagerDashboard from "./components/Manager/ManagerDashboard";
import Unauthorized from "./pages/Unauthorized";
import { RestaurantsMenuList } from "./components/Restaurant/RestaurantsMenuList";
import { Signup } from "./pages/SignUp";
import NotFound from "./pages/404Page";
import { ListRestaurants } from "./components/Restaurant/ListRestaurants";
import { DeliveryHistoryPage } from "./components/Restaurant/DeliveryHistory";
import { Cart } from "./components/Restaurant/Cart";
import { Order } from "./components/Restaurant/Order";
import { OrderTrack } from "./components/Restaurant/OrderTrack";
import { DeliveryPersonList } from "./components/Restaurant/DeliveryPerson";
import DeliveryAddressPage from "./components/UserDashboard/DeliveryAddress";
import MyOrders from "./components/UserDashboard/MyOrders";
import Wishlist from "./components/UserDashboard/Wishlist";
import { ManageRestaurant } from "./components/Restaurant/ManageRestaurant";
import MyTableBooking from "./components/UserDashboard/MyTableBooking";
import ManageReviews from "./components/UserDashboard/ManageReviews";
import { Homepage } from "./components/Restaurant/Homepage";
import RestaurantAnalyticsComponent from "./components/Admin/RestaurantAnalytics";
import AllRestaurants from "./components/Admin/AllRestaurants";
import ManagerReq from "./components/Admin/ManagerReq";
import RevenueAnalysis from "./components/Admin/RevenueAnalysis";
import AddDeliveryPerson from "./components/Admin/AddDeliveryPerson";
import Delivery from "./components/Admin/Delivery";

function App() {
  return (
    <BrowserRouter>
      <Routes>

        <Route path="/" element={<LoginForm />} />

        <Route path="/login" element={<LoginForm />} />
        <Route path="/signup" element={<Signup />} />

        <Route path="/unauthorized" element={<Unauthorized />} />

        {/* CUSTOMER ROUTES */}
        <Route element={<ProtectedRoute allowedRoles={["Customer","Manager"]} />}>
          <Route element={<CustomerDashboard />}>

            <Route path="/customer-dashboard" element={<Homepage />} />
            <Route path="/restaurants" element={<ListRestaurants />} />
            <Route path="/restaurant-menu/:id/:name" element={<RestaurantsMenuList />} />
            <Route path="/cart" element={<Cart />} />
            <Route path="/orders" element={<Order />} />
            <Route path="/orderTrack" element={<OrderTrack />} />
            <Route path="/delivery-history/:id" element={<DeliveryHistoryPage />} />
            <Route path="/manager-restaurant" element={<ManageRestaurant />} />
            <Route path="/delivery-person" element={<DeliveryPersonList />} />
            <Route path="/customer/wishlist" element={<Wishlist />} />
            <Route path="/customer/orders" element={<MyOrders />} />
            <Route path="/customer/bookings" element={<MyTableBooking />} />
            <Route path="/delivery-address" element={<DeliveryAddressPage />} />
            <Route path="/customer/reviews" element={<ManageReviews />} />

          </Route>
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
          <Route path="/manager/home" element={<Homepage />} />
        </Route>

    {/* ADMIN ROUTES */}
<Route element={<ProtectedRoute allowedRoles={["Admin"]} />}>
  <Route element={<AdminDashboard />}>
    <Route path="/admin-dashboard" element={<RevenueAnalysis />} />
    <Route path="/admin/restaurants" element={<AllRestaurants />} />
    <Route path="/admin/restaurant-analytics/:restaurantId" element={<RestaurantAnalyticsComponent />} />
    <Route path="/admin/requests" element={<ManagerReq />} />
   <Route path="/admin/analytics" element={<RevenueAnalysis />} />
   <Route path="/admin/delivery-person" element={<AddDeliveryPerson />} />
   <Route path="/admin/delivery" element={<Delivery />} />
<Route path="/admin/add-delivery-person" element={<AddDeliveryPerson />} />   
  
      
  </Route>
</Route>



        <Route path="*" element={<NotFound />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
